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
        public string ActorName;
        public CharacterProfile ActorProfile; // Reference to the ScriptableObject
        public CharacterProfile.ActorExpression ActorExpression; // Enum for expressions
        public int LocationIndex;
        public string DialogueText;

        public Sprite GetActorSprite()
        {
            if (ActorProfile != null && (int)ActorExpression >= 0 && (int)ActorExpression < ActorProfile.characterSprites.Count)
            {
                return ActorProfile.characterSprites[(int)ActorExpression];
            }
            return null;
        }
    }
}