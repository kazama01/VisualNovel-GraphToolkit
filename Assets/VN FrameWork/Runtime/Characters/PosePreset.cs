using UnityEngine;

namespace VN_FrameWork
{
    [CreateAssetMenu(fileName = "PosePreset", menuName = "VN_FrameWork/PosePreset", order = 2)]
    public class PosePreset : ScriptableObject
    {
        public ActorExpression expression;
        public Location location;
        public SpriteEffect entryEffect;
        public float effectSpeed;
    }
}