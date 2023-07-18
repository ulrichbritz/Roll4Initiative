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

        public override void Interact(Transform interactorTransform = null)
        {
            base.Interact();

            if(interactorTransform != null)
                transform.LookAt(interactorTransform);

            animManager.Anim.Play("Talk");
        }


    }
}

