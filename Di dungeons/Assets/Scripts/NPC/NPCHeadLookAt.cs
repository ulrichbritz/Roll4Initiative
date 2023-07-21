using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace UB
{
    public class NPCHeadLookAt : MonoBehaviour
    {
        [SerializeField] private Rig rig;
        [SerializeField] private Transform headLookAtTransform;

        private bool isLookingAtPosition;

        private void Update()
        {
            float targetWeight = isLookingAtPosition ? 1f : 0f;
            float lerpSpeed = 2f;
            rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * lerpSpeed);
        }

        public void LookAtPosition(Vector3 lookAtPosition)
        {
            isLookingAtPosition = true;
            headLookAtTransform.position = lookAtPosition;
        }
    }
}

