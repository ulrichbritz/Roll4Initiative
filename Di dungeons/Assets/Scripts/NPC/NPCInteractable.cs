using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class NPCInteractable : Interactable
    {
        [Header("Components")]
        private AnimationManager animManager;

        [Header("NPC Details")]
        [SerializeField] private string npcName;
        [HideInInspector] public string NPCName => npcName;


        private void Awake()
        {
            animManager = GetComponent<AnimationManager>();
            interactableText = "Interact with +" + npcName;
        }

        public override void Interact(Transform interactorTransform = null)
        {
            base.Interact();

            if(interactorTransform != null)
                transform.LookAt(interactorTransform);

            animManager.Anim.Play("Talk");
        }


    }
}

