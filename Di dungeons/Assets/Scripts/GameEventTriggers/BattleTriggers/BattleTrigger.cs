using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class BattleTrigger : MonoBehaviour
    {
        [Header("Enemies")]
        [SerializeField] List<CharacterController> enemiesInBattle;
        public List<CharacterController> EnemiesInBattle => enemiesInBattle;

        [Header("Components")]
        [SerializeField] Collider triggerCollider;

        private void Awake()
        {

        }


        /*

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                DisableCollider();

                CharacterController playerController = collision.gameObject.GetComponent<CharacterController>();
                GameManager.instance.StartBattle(enemiesInBattle, playerController, this);
            }
        }
        */

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

