// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using System.Collections.Generic;
using UnityEngine;
using VN_FrameWork;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector.Editor
{
    [Serializable]
    internal class ConversationNode : VisualNovelNode
    {
        public CharacterProfile SelectedCharacterProfile;
        public string SelectedExpression;

        public const string IN_PORT_SCRIPT_NAME = "Script";
        public const string IN_PORT_SCRIPT_FILE_NAME = "ScriptFile";
        public const string IN_PORT_LOCATION_INDEX = "LocationIndex";
        public const string IN_PORT_EXPRESSION = "Expression";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionPorts(context);

            context.AddInputPort<string>(IN_PORT_SCRIPT_NAME)
                .WithDisplayName("Script")
                .Build();

            context.AddInputPort<TextAsset>(IN_PORT_SCRIPT_FILE_NAME)
                .WithDisplayName("Script File")
                .Build();

            context.AddInputPort<CharacterProfile>("CharacterProfile")
                .WithDisplayName("Character Profile")
                .Build();

            context.AddInputPort<string>(IN_PORT_EXPRESSION)
                .WithDisplayName("Expression")
                .Build();
            
        }

        /// <summary>
        /// Creates the runtime node for this ConversationNode.
        /// </summary>
        public VisualNovelRuntimeNode CreateRuntimeNode()
        {
            var runtimeNode = new ConversationRuntimeNode
            {
                ScriptText = GetPortValue<string>(IN_PORT_SCRIPT_NAME),
                ScriptFile = GetPortValue<TextAsset>(IN_PORT_SCRIPT_FILE_NAME),
                CharacterProfile = GetPortValue<CharacterProfile>("CharacterProfile"),
                SelectedExpression = GetPortValue<string>("Expression"),
            };

            return runtimeNode;
        }

        /// <summary>
        /// Helper method to get the value of an input port.
        /// </summary>
        private T GetPortValue<T>(string portName)
        {
            var port = GetInputPortByName(portName); // Use GetInputPortByName to retrieve the port
            if (port != null && port.TryGetValue(out T value)) // Use TryGetValue to get the value
            {
                return value;
            }

            return default;
        }

        /// <summary>
        /// Provides a dropdown for selecting expressions from the CharacterProfile.
        /// </summary>
        public IEnumerable<string> GetExpressionOptions()
        {
            if (SelectedCharacterProfile != null && SelectedCharacterProfile.characterSprites != null)
            {
                foreach (var sprite in SelectedCharacterProfile.characterSprites)
                {
                    yield return sprite.name; // Add sprite names to the dropdown
                }
            }
        }
    }
}