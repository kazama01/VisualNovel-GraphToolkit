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
        public const string IN_PORT_ACTOR_PROFILE_NAME = "ActorProfile";
        public const string IN_PORT_POSE_PRESET_NAME = "PosePreset";
        public const string IN_PORT_LOCATION_NAME = "ActorLocation";
        public const string IN_PORT_DIALOGUE_NAME = "Dialogue";
        public const string IN_PORT_EXPRESSION_NAME = "ActorExpression";
        public const string IN_PORT_ENTRY_EFFECT_NAME = "EntryEffect";
        public const string IN_PORT_EFFECT_SPEED_NAME = "EffectSpeed";
        
        public const string IN_PORT_ACTOR_2_PROFILE_NAME = "Actor2Profile";
        public const string IN_PORT_POSE_PRESET_2_NAME = "PosePreset2";
        public const string IN_PORT_ACTOR_2_EXPRESSION_NAME = "Actor2Expression";
        public const string IN_PORT_ACTOR_2_LOCATION_NAME = "Actor2Location";
        public const string IN_PORT_ENTRY_EFFECT_2_NAME = "EntryEffect2";
        public const string IN_PORT_EFFECT_SPEED_2_NAME = "EffectSpeed2";
        public const string IN_PORT_SPEAKER_NAME = "Speaker";
        
        public enum Speaker
        {
            Character1,
            Character2
        }

        /// <summary>
        /// Defines the output for the node.
        /// </summary>
        /// <param name="context">The scope to define the node.</param>
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddInputOutputExecutionPorts(context);

            context.AddInputPort<CharacterProfile>(IN_PORT_ACTOR_PROFILE_NAME)
                .WithDisplayName("Actor Profile")
                .Build();
            context.AddInputPort<PosePreset>(IN_PORT_POSE_PRESET_NAME)
                .WithDisplayName("Pose Preset")
                .Build();
            context.AddInputPort<Location>(IN_PORT_LOCATION_NAME)
                .WithDisplayName("Actor Location")
                .WithDefaultValue(Location.Left)
                .Build();
            context.AddInputPort<ActorExpression>(IN_PORT_EXPRESSION_NAME)
                .WithDisplayName("Actor Expression")
                .WithDefaultValue(ActorExpression.Neutral)
                .Build();
            context.AddInputPort<string>(IN_PORT_DIALOGUE_NAME)
                .Build();
            context.AddInputPort<SpriteEffect>(IN_PORT_ENTRY_EFFECT_NAME)
                .WithDisplayName("Entry Effect")
                .WithDefaultValue(SpriteEffect.None)
                .Build();
            context.AddInputPort<float>(IN_PORT_EFFECT_SPEED_NAME)
                .WithDisplayName("Effect Speed")
                .WithDefaultValue(1.0f)
                .Build();
            
            context.AddInputPort<CharacterProfile>(IN_PORT_ACTOR_2_PROFILE_NAME)
                .WithDisplayName("Actor 2 Profile")
                .Build();
            context.AddInputPort<PosePreset>(IN_PORT_POSE_PRESET_2_NAME)
                .WithDisplayName("Pose Preset 2")
                .Build();
            context.AddInputPort<ActorExpression>(IN_PORT_ACTOR_2_EXPRESSION_NAME)
                .WithDisplayName("Actor 2 Expression")
                .WithDefaultValue(ActorExpression.Neutral)
                .Build();
            context.AddInputPort<Location>(IN_PORT_ACTOR_2_LOCATION_NAME)
                .WithDisplayName("Actor 2 Location")
                .WithDefaultValue(Location.Right)
                .Build();
            context.AddInputPort<SpriteEffect>(IN_PORT_ENTRY_EFFECT_2_NAME)
                .WithDisplayName("Entry Effect 2")
                .WithDefaultValue(SpriteEffect.None)
                .Build();
            context.AddInputPort<float>(IN_PORT_EFFECT_SPEED_2_NAME)
                .WithDisplayName("Effect Speed 2")
                .WithDefaultValue(1.0f)
                .Build();
            context.AddInputPort<Speaker>(IN_PORT_SPEAKER_NAME)
                .WithDisplayName("Speaker")
                .Build();
        }
    }
}