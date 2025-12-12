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
    public class SetDialogueExecutor : IVisualNovelNodeExecutor<SetDialogueRuntimeNode>, IVisualNovelNodeExecutor<SetDialogueRuntimeNodeWithPreviousActor>, IVisualNovelNodeExecutor<ConversationRuntimeNode>
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

        public async Task ExecuteAsync(ConversationRuntimeNode runtimeNode, VisualNovelDirector ctx)
        {
            // Read the dialogue text from the script text box or script file
            string dialogueText = runtimeNode.ScriptText;
            if (runtimeNode.ScriptFile != null)
            {
                dialogueText = runtimeNode.ScriptFile.text;
            }

            if (string.IsNullOrEmpty(dialogueText))
            {
                ctx.DialoguePanel.SetActive(false);
                return;
            }

            // Split the dialogue into lines
            var lines = dialogueText.Split(new[] { "\\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var match = DialogueFormatRegex.Match(line);
                if (!match.Success)
                {
                    Debug.LogWarning($"Invalid dialogue format: {line}");
                    continue;
                }

                var name = match.Groups["name"].Value.Trim();
                var expression = match.Groups["expression"].Success ? match.Groups["expression"].Value.Trim() : null;
                var text = match.Groups["text"].Value.Trim();

                // Set character profile data
                if (runtimeNode.CharacterProfile != null)
                {
                    ctx.ActorNameText.text = name;
                    ctx.ActorNameText.color = runtimeNode.CharacterProfile.characterColor;

                    if (runtimeNode.CharacterProfile.characterSprites != null && runtimeNode.CharacterProfile.characterSprites.Count > 0)
                    {
                        var sprite = runtimeNode.CharacterProfile.characterSprites.Find(s => s.name.Equals(expression, StringComparison.OrdinalIgnoreCase));
                        if (sprite != null)
                        {
                            ctx.ActorLocationList[0].sprite = sprite; // Use the first location for the actor
                            ctx.ActorLocationList[0].enabled = true;
                        }
                        else
                        {
                            ctx.ActorLocationList[0].enabled = false;
                        }
                    }
                }

                // Play the dialogue line
                await TypeTextWithSkipAsync(text, ctx);
            }
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