using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class RotateOverTime : MonoBehaviour
    {
        [SerializeField] float rotateSpeed;

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + (rotateSpeed * Time.deltaTime), 0f);
        }
    }
}

