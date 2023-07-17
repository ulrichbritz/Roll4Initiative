using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class EnemyStats : CharacterStats
    {
        public override void Die()
        {
            base.Die();

            hpText.gameObject.SetActive(false);
            hpSlider.gameObject.SetActive(false);
            GameManager.instance.allChars.Remove(characterController);
            isDead = true;

            StartCoroutine(DoCheckForBattleOver());
        }

        private IEnumerator DoCheckForBattleOver()
        {
            yield return new WaitForSeconds(3f);

            GameManager.instance.CheckForBattleOver();
        }
    }

    
}

