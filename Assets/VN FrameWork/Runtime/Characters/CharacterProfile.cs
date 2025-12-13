using System.Collections.Generic;
using UnityEngine;
using VN_FrameWork;

namespace VN_FrameWork
{
    [System.Serializable]
    public class Expression
    {
        public ActorExpression expression;
        public Sprite sprite;
    }

    [CreateAssetMenu(fileName = "CharacterProfile", menuName = "VN_FrameWork/CharacterProfile", order = 1)]
    public class CharacterProfile : ScriptableObject
    {
        public string characterName;
        public Color characterColor = Color.white;
        public List<Expression> expressions = new List<Expression>();

        public Sprite GetSprite(ActorExpression expression)
        {
            return expressions.Find(e => e.expression == expression)?.sprite;
        }
    }
}