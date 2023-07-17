using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class EnemyController : CharacterController
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (isInBattle)
            {
                HandleBattleUpdate();
            }
        }
    }
}

