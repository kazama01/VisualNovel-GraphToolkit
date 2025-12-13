// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using VN_FrameWork;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    /// <summary>
    /// The serializable data representing a runtime node in the visual novel graph that sets the dialogue text and actor information.
    /// </summary>
    [Serializable]
    public class SetDialogueRuntimeNode : VisualNovelRuntimeNode
    {
        public enum SpriteEffect
        {
            None,
            FadeIn,
            FadeOut,
            Jump,
            Shake,
            ExitLeft,
            ExitRight,
            SlideDown,
            Bop
        }

        public string ActorName;
        public CharacterProfile ActorProfile; // Reference to the ScriptableObject
        public CharacterProfile.ActorExpression ActorExpression; // Enum for expressions
        public bool IsSpeaker = true;
        public int LocationIndex;
        public string DialogueText;
        
        public CharacterProfile Actor2Profile; // Reference to the second ScriptableObject
        public CharacterProfile.ActorExpression Actor2Expression; // Enum for expressions for the second actor
        public bool IsSpeaker2 = false;
        public int Location2Index;

        public SpriteEffect EntryEffect = SpriteEffect.None;
        public float EffectSpeed = 1.0f;
        
        public SpriteEffect EntryEffect2 = SpriteEffect.None;
        public float EffectSpeed2 = 1.0f;


        public Sprite GetActorSprite()
        {
            if (ActorProfile != null && (int)ActorExpression >= 0 && (int)ActorExpression < ActorProfile.characterSprites.Count)
            {
                return ActorProfile.characterSprites[(int)ActorExpression];
            }
            return null;
        }
        
        public Sprite GetActor2Sprite()
        {
            if (Actor2Profile != null && (int)Actor2Expression >= 0 && (int)Actor2Expression < ActorProfile.characterSprites.Count)
            {
                return Actor2Profile.characterSprites[(int)Actor2Expression];
            }
            return null;
        }
    }
}