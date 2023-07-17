using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class RotateTwoDirectionsOverTime : MonoBehaviour
    {
        [SerializeField] float rotateSpeed;

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + (rotateSpeed * Time.deltaTime), transform.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
        }
    }
}

