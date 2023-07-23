using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class DataInteractionHandler : MonoBehaviour
    {
        public void EndInteraction()
        {
            PlayerManager.instance.isInteracting = false;
        }
    }
}

