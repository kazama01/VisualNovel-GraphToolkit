// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    public class SetDialogueExecutor : IVisualNovelNodeExecutor<SetDialogueRuntimeNode>, IVisualNovelNodeExecutor<SetDialogueRuntimeNodeWithPreviousActor>
    {
        private static readonly Regex DialogueFormatRegex = new Regex(@"^(?<name>[^()]+)(?:\\((?<expression>[^\\)]+)\\))?:\\s*(?<text>.+)$");

        public async Task ExecuteAsync(SetDialogueRuntimeNode runtimeNode, VisualNovelDirector ctx)
        {
            await ExecuteDialogue(runtimeNode.ActorName, runtimeNode.ActorSprite, runtimeNode.DialogueText, runtimeNode.LocationIndex, ctx);
        }

        public async Task ExecuteAsync(SetDialogueRuntimeNodeWithPreviousActor runtimeNode, VisualNovelDirector ctx)
        {
            await ExecuteDialogue(null, null, runtimeNode.DialogueText, -1, ctx);
        }

       

        private static async Task ExecuteDialogue(string actorName, Sprite actorSprite, string dialogueText, int locationIndex, VisualNovelDirector ctx)
        {
            if (string.IsNullOrEmpty(dialogueText))
            {
                ctx.DialoguePanel.SetActive(false);
                return;
            }

            ctx.DialoguePanel.SetActive(true);
            ctx.ActorNameText.text = actorName;

            foreach (var location in ctx.ActorLocationList)
                location.enabled = false;

            Image actorImage = null;
            if (actorSprite != null && locationIndex >= 0 && locationIndex < ctx.ActorLocationList.Count)
            {
                actorImage = ctx.ActorLocationList[locationIndex];
                actorImage.enabled = true;
                actorImage.sprite = actorSprite;
            }

            await TypeTextWithSkipAsync(dialogueText, ctx);
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
                        return;
                    }
                    timer += Time.deltaTime;
                    await Task.Yield();
                }
            }

            label.text = dialogueText;
        }
    }
}