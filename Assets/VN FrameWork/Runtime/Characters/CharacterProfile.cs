
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
    }
}
