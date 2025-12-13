using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;
using VN_FrameWork;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector.Editor
{
    [ScriptedImporter(1, VisualNovelDirectorGraph.AssetExtension)]
    internal class VisualNovelDirectorImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var graph = GraphDatabase.LoadGraphForImporter<VisualNovelDirectorGraph>(ctx.assetPath);

            if (graph == null)
            {
                Debug.LogError($"Failed to load Visual Novel Director graph asset: {ctx.assetPath}");
                return;
            }

            var startNodeModel = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
            if (startNodeModel == null)
            {
                return;
            }

            var runtimeAsset = ScriptableObject.CreateInstance<VisualNovelRuntimeGraph>();
            var nextNodeModel = GetNextNode(startNodeModel);
            while (nextNodeModel != null)
            {
                var runtimeNodes = TranslateNodeModelToRuntimeNodes(nextNodeModel);
                runtimeAsset.Nodes.AddRange(runtimeNodes);

                nextNodeModel = GetNextNode(nextNodeModel);
            }

            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        static INode GetNextNode(INode currentNode)
        {
            var outputPort = currentNode.GetOutputPortByName(VisualNovelNode.EXECUTION_PORT_DEFAULT_NAME);
            var nextNodePort = outputPort.firstConnectedPort;
            return nextNodePort?.GetNode();
        }

        static List<VisualNovelRuntimeNode> TranslateNodeModelToRuntimeNodes(INode nodeModel)
        {
            var returnedNodes = new List<VisualNovelRuntimeNode>();
            switch (nodeModel)
            {
                case SetBackgroundNode setBackgroundNodeModel:
                    returnedNodes.Add(new SetBackgroundRuntimeNode
                    {
                        BackgroundSprite = GetInputPortValue<Sprite>(setBackgroundNodeModel.GetInputPortByName(SetBackgroundNode.IN_PORT_BACKGROUND_NAME))
                    });
                    break;

                case SetDialogueNode setDialogueNodeModel:
                    var runtimeNode = new SetDialogueRuntimeNode();

                    var speaker = GetInputPortValue<SetDialogueNode.Speaker>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_SPEAKER_NAME));
                    runtimeNode.DialogueText = GetInputPortValue<string>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_DIALOGUE_NAME));

                    // Actor 1
                    runtimeNode.ActorProfile = GetInputPortValue<CharacterProfile>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ACTOR_PROFILE_NAME));
                    var posePreset = GetInputPortValue<PosePreset>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_POSE_PRESET_NAME));

                    if (posePreset != null)
                    {
                        runtimeNode.Expression = posePreset.expression;
                        runtimeNode.Location = posePreset.location;
                        runtimeNode.EntryEffect = posePreset.entryEffect;
                        runtimeNode.EffectSpeed = posePreset.effectSpeed;
                    }

                    // Override with connected values
                    if (setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_EXPRESSION_NAME).isConnected)
                        runtimeNode.Expression = GetInputPortValue<ActorExpression>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_EXPRESSION_NAME));
                    if (setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_LOCATION_NAME).isConnected)
                        runtimeNode.Location = GetInputPortValue<Location>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_LOCATION_NAME));
                    if (setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ENTRY_EFFECT_NAME).isConnected)
                        runtimeNode.EntryEffect = GetInputPortValue<SpriteEffect>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ENTRY_EFFECT_NAME));
                    if (setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_EFFECT_SPEED_NAME).isConnected)
                        runtimeNode.EffectSpeed = GetInputPortValue<float>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_EFFECT_SPEED_NAME));

                    runtimeNode.IsSpeaker = speaker == SetDialogueNode.Speaker.Character1;
                    if (runtimeNode.IsSpeaker && runtimeNode.ActorProfile != null)
                    {
                        runtimeNode.ActorName = runtimeNode.ActorProfile.characterName;
                    }

                    // Actor 2
                    runtimeNode.Actor2Profile = GetInputPortValue<CharacterProfile>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ACTOR_2_PROFILE_NAME));
                    var posePreset2 = GetInputPortValue<PosePreset>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_POSE_PRESET_2_NAME));

                    if (posePreset2 != null)
                    {
                        runtimeNode.Expression2 = posePreset2.expression;
                        runtimeNode.Location2 = posePreset2.location;
                        runtimeNode.EntryEffect2 = posePreset2.entryEffect;
                        runtimeNode.EffectSpeed2 = posePreset2.effectSpeed;
                    }
                    
                    // Override with connected values for Actor 2
                    if (setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ACTOR_2_EXPRESSION_NAME).isConnected)
                        runtimeNode.Expression2 = GetInputPortValue<ActorExpression>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ACTOR_2_EXPRESSION_NAME));
                    if (setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ACTOR_2_LOCATION_NAME).isConnected)
                        runtimeNode.Location2 = GetInputPortValue<Location>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ACTOR_2_LOCATION_NAME));
                    if (setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ENTRY_EFFECT_2_NAME).isConnected)
                        runtimeNode.EntryEffect2 = GetInputPortValue<SpriteEffect>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_ENTRY_EFFECT_2_NAME));
                    if (setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_EFFECT_SPEED_2_NAME).isConnected)
                        runtimeNode.EffectSpeed2 = GetInputPortValue<float>(setDialogueNodeModel.GetInputPortByName(SetDialogueNode.IN_PORT_EFFECT_SPEED_2_NAME));

                    runtimeNode.IsSpeaker2 = speaker == SetDialogueNode.Speaker.Character2;
                    if (runtimeNode.IsSpeaker2 && runtimeNode.Actor2Profile != null)
                    {
                        runtimeNode.ActorName = runtimeNode.Actor2Profile.characterName;
                    }
                    
                    returnedNodes.Add(runtimeNode);
                    returnedNodes.Add(new WaitForInputRuntimeNode());
                    break;

                case WaitForInputNode _:
                    returnedNodes.Add(new WaitForInputRuntimeNode());
                    break;

                default:
                    throw new ArgumentException($"Unsupported node model type: {nodeModel.GetType()}");
            }

            return returnedNodes;
        }

        static T GetInputPortValue<T>(IPort port)
        {
            if (port.isConnected)
            {
                var connectedNode = port.firstConnectedPort.GetNode();
                if (connectedNode is IVariableNode variableNode && variableNode.variable.TryGetDefaultValue(out T variableValue))
                {
                    return variableValue;
                }
                if (connectedNode is IConstantNode constantNode && constantNode.TryGetValue(out T constantValue))
                {
                    return constantValue;
                }
            }
            
            port.TryGetValue(out T value);
            return value;
        }
    }
}