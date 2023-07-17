using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class Billboard : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.rotation = CameraController.instance.transform.rotation;
        }
    }
}

