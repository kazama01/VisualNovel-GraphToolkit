// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Collections.Generic;
using UnityEngine;

namespace VN_FrameWork
{
    [CreateAssetMenu(fileName = "CharacterProfile", menuName = "VN_FrameWork/CharacterProfile", order = 1)]
    public class CharacterProfile : ScriptableObject
    {
        public string characterName;
        public Color characterColor = Color.white;
        public List<Sprite> characterSprites;

        public enum ActorExpression
        {
            Neutral,
            Happy,
            Sad,
            Angry,
            Surprised,
            Thinking
        }
    }
}