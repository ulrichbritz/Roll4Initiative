using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class AnimationManager : MonoBehaviour
    {
        [Header("Scripts")]
        CharacterManager characterManager;

        [Header("Components")]
        Animator anim;
        [HideInInspector]
        public Animator Anim => anim;

        protected virtual void Awake()
        {
            //scripts
            characterManager = GetComponent<CharacterManager>();

            //components
            anim = GetComponentInChildren<Animator>();

        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            characterManager.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.fixedDeltaTime);
            characterManager.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.fixedDeltaTime);
        }
    }
}

