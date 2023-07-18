using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class NPCInteractable : Interactable
    {
        private AnimationManager animManager;

        private void Awake()
        {
            animManager = GetComponent<AnimationManager>();
        }

        public override void Interact()
        {
            base.Interact();

            animManager.Anim.Play("Talk");
        }
    }
}

