using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class EnemyManager : CharacterManager
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();

            if (isInBattle)
            {
                HandleBattleUpdate();
            }
        }

        public override void StartBattle()
        {
            base.StartBattle();
        }

    }
}

