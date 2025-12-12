using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VN_FrameWork;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    public class VisualNovelDirector : MonoBehaviour
    {
        [Header("Graph")]
        public VisualNovelRuntimeGraph RuntimeGraph;

        [Header("Scene References")]
        public Image BackgroundImage;
        public List<Image> ActorLocationList;
        public GameObject DialoguePanel;
        public TextMeshProUGUI DialogueText;
        public TextMeshProUGUI ActorNameText;

        [Header("Settings")]
        public float GlobalFadeDuration = 0.5f;
        public float GlobalTextDelayPerCharacter = 0.03f;

        [Header("Input")]
        public MonoBehaviour InputComponent;
        public IVisualNovelInputProvider InputProvider => InputComponent as IVisualNovelInputProvider;

        private async void Start()
        {
            var setBackgroundExecutor = new SetBackgroundExecutor();
            var setDialogueExecutor = new SetDialogueExecutor();
            var waitForInputExecutor = new WaitForInputExecutor();

            foreach (var node in RuntimeGraph.Nodes)
            {
                switch (node)
                {
                    case SetBackgroundRuntimeNode bgNode:
                        await setBackgroundExecutor.ExecuteAsync(bgNode, this);
                        break;
                    case SetDialogueRuntimeNode dialogueNode:
                        await setDialogueExecutor.ExecuteAsync(dialogueNode, this);
                        break;
                    case SetDialogueRuntimeNodeWithPreviousActor dialogueNode:
                        await setDialogueExecutor.ExecuteAsync(dialogueNode, this);
                        break;
                    case WaitForInputRuntimeNode waitNode:
                        await waitForInputExecutor.ExecuteAsync(waitNode, this);
                        break;
                    default:
                        Debug.LogError($"No executor found for node type: {node.GetType()}");
                        break;
                }
            }
        }
    }
}
