using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class PlayerAnimationManager : AnimationManager
    {
        PlayerManager playerManager;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()
        {
            if (playerManager.applyRootMotion)
            {
                Vector3 velocity = playerManager.animator.deltaPosition;
                playerManager.characterController.Move(velocity);
                playerManager.transform.rotation *= playerManager.animator.deltaRotation;
            }
        }
    }
}

