using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class DataInteractionHandler : MonoBehaviour
    {
        public void EndInteraction()
        {
            MouseControls.instance.HideCursor();
            PlayerManager.instance.isInteracting = false;
        }
    }
}

