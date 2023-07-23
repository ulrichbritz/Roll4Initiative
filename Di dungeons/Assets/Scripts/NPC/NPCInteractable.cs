using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

namespace UB
{
    public class NPCInteractable : Interactable
    {
        [Header("Components")]
        //scripts
        private AnimationManager animManager;
        private NPCHeadLookAt npcHeadLookAt;

        //components

        [Header("NPC Details")]
        [SerializeField] private string npcName;
        [HideInInspector] public string NPCName => npcName;

        [Header("Interaction Values")]
        [SerializeField] float playerHeight = 1.8f;

        [Header("Dialogue")]
        [SerializeField] NPCConversation myConversation;


        private void Awake()
        {
            animManager = GetComponent<AnimationManager>();
            npcHeadLookAt = GetComponent<NPCHeadLookAt>();
        }

        public override void Interact(Transform interactorTransform = null)
        {
            base.Interact();

            animManager.Anim.Play("Talk");

            npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);

            ConversationManager.Instance.StartConversation(myConversation);
        }


    }
}

