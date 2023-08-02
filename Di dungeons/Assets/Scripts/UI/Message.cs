using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private float deleteTime;

        private void Update()
        {
            deleteTime -= Time.deltaTime;

            if(deleteTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

