// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    public class SetDialogueExecutor : IVisualNovelNodeExecutor<SetDialogueRuntimeNode>, IVisualNovelNodeExecutor<SetDialogueRuntimeNodeWithPreviousActor>
    {
        public async Task ExecuteAsync(SetDialogueRuntimeNode runtimeNode, VisualNovelDirector ctx)
        {
            Sprite actorSprite = runtimeNode.GetActorSprite();
            await ExecuteDialogue(runtimeNode, ctx);
        }

        public async Task ExecuteAsync(SetDialogueRuntimeNodeWithPreviousActor runtimeNode, VisualNovelDirector ctx)
        {
            // This node type doesn't have actor information, so we can't apply effects.
            await TypeTextWithSkipAsync(runtimeNode.DialogueText, ctx);
        }

        private static async Task ExecuteDialogue(SetDialogueRuntimeNode runtimeNode, VisualNovelDirector ctx)
        {
            if (string.IsNullOrEmpty(runtimeNode.DialogueText))
            {
                ctx.DialoguePanel.SetActive(false);
                return;
            }

            ctx.DialoguePanel.SetActive(true);
            ctx.ActorNameText.text = runtimeNode.ActorName;
            
            if (runtimeNode.IsSpeaker && runtimeNode.ActorProfile != null)
            {
                ctx.ActorNameText.color = runtimeNode.ActorProfile.characterColor;
            }
            else if (runtimeNode.IsSpeaker2 && runtimeNode.Actor2Profile != null)
            {
                ctx.ActorNameText.color = runtimeNode.Actor2Profile.characterColor;
            }

            // Disable all locations first
            foreach (var location in ctx.ActorLocationList)
                location.enabled = false;

            Image actorImage = null;
            if (runtimeNode.GetActorSprite() != null && runtimeNode.LocationIndex >= 0 && runtimeNode.LocationIndex < ctx.ActorLocationList.Count)
            {
                actorImage = ctx.ActorLocationList[runtimeNode.LocationIndex];
                actorImage.enabled = true;
                actorImage.sprite = runtimeNode.GetActorSprite();

                SpriteAnimator animator = actorImage.GetComponent<SpriteAnimator>();
                if (animator == null)
                {
                    animator = actorImage.gameObject.AddComponent<SpriteAnimator>();
                }

                if (runtimeNode.IsSpeaker && runtimeNode.EntryEffect != SetDialogueRuntimeNode.SpriteEffect.None)
                {
                    animator.PlayEffect(runtimeNode.EntryEffect, runtimeNode.EffectSpeed);
                }
            }
            
            Image actor2Image = null;
            if (runtimeNode.GetActor2Sprite() != null && runtimeNode.Location2Index >= 0 && runtimeNode.Location2Index < ctx.ActorLocationList.Count)
            {
                actor2Image = ctx.ActorLocationList[runtimeNode.Location2Index];
                actor2Image.enabled = true;
                actor2Image.sprite = runtimeNode.GetActor2Sprite();

                SpriteAnimator animator = actor2Image.GetComponent<SpriteAnimator>();
                if (animator == null)
                {
                    animator = actor2Image.gameObject.AddComponent<SpriteAnimator>();
                }

                if (runtimeNode.IsSpeaker2 && runtimeNode.EntryEffect != SetDialogueRuntimeNode.SpriteEffect.None)
                {
                    animator.PlayEffect(runtimeNode.EntryEffect, runtimeNode.EffectSpeed);
                }
            }

            await TypeTextWithSkipAsync(runtimeNode.DialogueText, ctx);

            if (actorImage != null && runtimeNode.IsSpeaker && runtimeNode.ExitEffect != SetDialogueRuntimeNode.SpriteEffect.None)
            {
                SpriteAnimator animator = actorImage.GetComponent<SpriteAnimator>();
                animator.PlayEffect(runtimeNode.ExitEffect, runtimeNode.EffectSpeed);
            }
            
            if (actor2Image != null && runtimeNode.IsSpeaker2 && runtimeNode.ExitEffect != SetDialogueRuntimeNode.SpriteEffect.None)
            {
                SpriteAnimator animator = actor2Image.GetComponent<SpriteAnimator>();
                animator.PlayEffect(runtimeNode.ExitEffect, runtimeNode.EffectSpeed);
            }
        }

        private static async Task TypeTextWithSkipAsync(string dialogueText, VisualNovelDirector ctx)
        {
            var label = ctx.DialogueText;
            var delayPerCharSeconds = ctx.GlobalTextDelayPerCharacter;
            var inputProvider = ctx.InputProvider;

            label.text = "";
            var builder = new System.Text.StringBuilder();

            var insideRichTag = false;

            var skipInputDetected = inputProvider.InputDetected();

            foreach (var c in dialogueText)
            {
                if (c == '<')
                    insideRichTag = true;

                builder.Append(c);

                if (c == '>')
                    insideRichTag = false;

                if (insideRichTag || char.IsWhiteSpace(c))
                {
                    label.text = builder.ToString();
                    continue;
                }

                label.text = builder.ToString();

                var timer = 0f;
                while (timer < delayPerCharSeconds)
                {
                    if (skipInputDetected.IsCompleted)
                    {
                        label.text = dialogueText;
                        await Task.Yield(); // Wait a frame to allow the exit effect to start
                        return;
                    }
                    timer += Time.deltaTime;
                    await Task.Yield();
                }
            }

            label.text = dialogueText;
            await Task.Yield(); // Wait a frame to allow the exit effect to start
        }
    }
}