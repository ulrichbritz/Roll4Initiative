using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class BattleTrigger : MonoBehaviour
    {
        [Header("Enemies")]
        [SerializeField] List<CharacterManager> enemiesInBattle;
        public List<CharacterManager> EnemiesInBattle => enemiesInBattle;

        [Header("Components")]
        [SerializeField] Collider triggerCollider;

        private void Awake()
        {

        }

        public void DisableCollider()
        {
            triggerCollider.enabled = false;
        }

        public void OnPlayerWon()
        {
            Destroy(gameObject);
        }

        public void OnPlayerLost()
        {
            //make collider active again after player resets
        }

    }
}

