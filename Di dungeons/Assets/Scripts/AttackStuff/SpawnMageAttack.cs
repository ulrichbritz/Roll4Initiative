using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class SpawnMageAttack : MonoBehaviour
    {
        [SerializeField] float speed = 10f;

        Vector3 targetPoint;

        private void Awake()
        {
            targetPoint = GameManager.instance.activeCharacter.allTargets[GameManager.instance.activeCharacter.currentTarget].hitPoint.transform.position;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

            if(Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

