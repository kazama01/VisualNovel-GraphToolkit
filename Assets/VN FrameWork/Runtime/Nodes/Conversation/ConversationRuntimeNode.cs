// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

// 12/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using VN_FrameWork;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    /// <summary>
    /// The serializable data representing a runtime node in the visual novel graph that handles conversation scripts.
    /// </summary>
    [Serializable]
    public class ConversationRuntimeNode : VisualNovelRuntimeNode
    {
        public string ScriptText;
        public TextAsset ScriptFile;
        public CharacterProfile CharacterProfile;
        public string SelectedExpression; // The selected expression from the dropdown
    }
}