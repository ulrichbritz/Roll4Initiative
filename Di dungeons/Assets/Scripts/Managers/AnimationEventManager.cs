using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class AnimationEventManager : MonoBehaviour
    {
        PlayerManager playerManager;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
        }
        public void ApplyJumpingVelocityFromEvent()
        {
            playerManager.ApplyjumpingVelocity();
        }
    }
}

