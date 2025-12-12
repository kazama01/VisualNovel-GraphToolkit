// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    /// <summary>
    /// Executor for the <see cref="ConversationRuntimeNode"/> node.
    /// </summary>
    public class ConversationExecutor : IVisualNovelNodeExecutor<ConversationRuntimeNode>
    {
        /// <summary>
        /// Executes the conversation node, displaying the dialogue from the script text or script file and using the character profile data.
        /// </summary>
        public async Task ExecuteAsync(ConversationRuntimeNode runtimeNode, VisualNovelDirector ctx)
        {
            // Ensure the dialogue panel is active
            ctx.DialoguePanel.SetActive(true);

            // Set the character name and color from the CharacterProfile
            if (runtimeNode.CharacterProfile != null)
            {
                ctx.ActorNameText.text = runtimeNode.CharacterProfile.characterName;
                ctx.ActorNameText.color = runtimeNode.CharacterProfile.characterColor;

                if (runtimeNode.CharacterProfile.characterSprites != null && runtimeNode.CharacterProfile.characterSprites.Count > 0)
                {
                    ctx. ActorImage.sprite = runtimeNode.CharacterProfile.characterSprites[0]; // Use the first sprite as default
                    ctx.ActorImage.enabled = true;
                }
                else
                {
                    ctx.ActorImage.enabled = false;
                }
            }

            // Display the script text or load from the script file
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

            // Type out the dialogue text with a typing effect
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
