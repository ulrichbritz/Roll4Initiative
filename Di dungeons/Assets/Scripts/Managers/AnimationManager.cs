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

        public void SetInBattle(bool isInBattle)
        {
            characterManager.animator.SetBool("isInBattle", isInBattle);
        }

        public void SetMovingBool(bool isMoving)
        {
            characterManager.animator.SetBool("isMoving", isMoving);
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float horizontal = horizontalValue;
            float vertical = verticalValue;

            if (isSprinting)
            {
                vertical = 2;
            }

            characterManager.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            characterManager.animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            characterManager.animator.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);

            characterManager.isPerformingAction = isPerformingAction;
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;
        }
    }
}

