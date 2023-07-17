using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(GameManager gameManager);
    }
}

