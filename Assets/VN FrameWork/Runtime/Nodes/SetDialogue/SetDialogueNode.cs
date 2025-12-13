using System;
using UnityEngine;
using VN_FrameWork;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    [Serializable]
    public class SetDialogueRuntimeNode : VisualNovelRuntimeNode
    {
        public string ActorName;
        public CharacterProfile ActorProfile;
        public ActorExpression Expression;
        public bool IsSpeaker = true;
        public Location Location;
        public string DialogueText;

        public CharacterProfile Actor2Profile;
        public ActorExpression Expression2;
        public bool IsSpeaker2 = false;
        public Location Location2;

        public SpriteEffect EntryEffect = SpriteEffect.None;
        public float EffectSpeed = 1.0f;

        public SpriteEffect EntryEffect2 = SpriteEffect.None;
        public float EffectSpeed2 = 1.0f;

        public Sprite GetActorSprite()
        {
            return ActorProfile != null ? ActorProfile.GetSprite(Expression) : null;
        }

        public Sprite GetActor2Sprite()
        {
            return Actor2Profile != null ? Actor2Profile.GetSprite(Expression2) : null;
        }
    }
}