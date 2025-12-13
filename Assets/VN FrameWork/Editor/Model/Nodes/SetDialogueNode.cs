// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using VN_FrameWork;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector.Editor
{
    /// <summary>
    /// Represents the Set Dialogue Node in the Visual Novel Director tool.
    /// </summary>
    /// <remarks>
    /// Is converted to a <see cref="SetDialogueRuntimeNode"/> for the runtime.
    /// </remarks>
    [Serializable]
    internal class SetDialogueNode : VisualNovelNode
    {
        public const string IN_PORT_ACTOR_NAME_NAME = "ActorName";
        public const string IN_PORT_ACTOR_PROFILE_NAME = "ActorProfile"; // Updated to ActorProfile
        public const string IN_PORT_LOCATION_NAME = "ActorLocation";
        public const string IN_PORT_DIALOGUE_NAME = "Dialogue";
        public const string IN_PORT_EXPRESSION_NAME = "ActorExpression";
        public const string IN_PORT_ENTRY_EFFECT_NAME = "EntryEffect";
        public const string IN_PORT_EXIT_EFFECT_NAME = "ExitEffect";
        public const string IN_PORT_EFFECT_SPEED_NAME = "EffectSpeed";

        public enum Location
        {
            Left = 0,
            Right = 1
        }

        /// <summary>
        /// Defines the output for the node.
        /// </summary>
        /// <param name="context">The scope to define the node.</param>
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionPorts(context);

            context.AddInputPort<string>(IN_PORT_ACTOR_NAME_NAME)
                .WithDisplayName("Actor Name")
                .Build();
            context.AddInputPort<CharacterProfile>(IN_PORT_ACTOR_PROFILE_NAME) // Updated to CharacterProfile
                .WithDisplayName("Actor Profile") // Updated display name
                .Build();
            context.AddInputPort<Location>(IN_PORT_LOCATION_NAME)
                .WithDisplayName("Actor Location")
                .Build();
            context.AddInputPort<CharacterProfile.ActorExpression>(IN_PORT_EXPRESSION_NAME)
                .WithDisplayName("Actor Expression")
                .Build();
            context.AddInputPort<string>(IN_PORT_DIALOGUE_NAME)
                .Build();
            context.AddInputPort<SetDialogueRuntimeNode.SpriteEffect>(IN_PORT_ENTRY_EFFECT_NAME)
                .WithDisplayName("Entry Effect")
                .Build();
            context.AddInputPort<SetDialogueRuntimeNode.SpriteEffect>(IN_PORT_EXIT_EFFECT_NAME)
                .WithDisplayName("Exit Effect")
                .Build();
            context.AddInputPort<float>(IN_PORT_EFFECT_SPEED_NAME)
                .WithDisplayName("Effect Speed")
                .WithDefaultValue(1.0f)
                .Build();
        }
    }
}