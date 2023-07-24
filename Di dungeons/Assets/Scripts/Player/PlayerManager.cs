using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UB
{
    public class PlayerManager : CharacterManager
    {
        public static PlayerManager instance;

        [Header("Components")]
        //scripts
        CharacterStats charStats;
        [HideInInspector] public PlayerAnimationManager playerAnimationManager;


        //components
        

        //Roaming
        [Header("Player Roaming Scripts")]
        InputHandler inputHandler;

        [Header("Movement")]
        [SerializeField] LayerMask movementMask;

        [Header("Interactable")]
        [SerializeField] float interactRange = 2f;
        [SerializeField] LayerMask interactableLayers;
        [HideInInspector] public Interactable interactableObject = null;

        [Header("Roaming Movement")]
        //settings
        [SerializeField] float rotationSpeed = 0.25f;

        //values
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;

        [Header("Jump")]
        private Vector3 jumpDirection;

        [Header("Interacting")]
        public bool isInteracting = false;

        public bool uiFlag = false;




        protected override void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            base.Awake();

            //scripts
            inputHandler = GetComponent<InputHandler>();
            charStats = GetComponent<CharacterStats>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();

        }

        protected override void Start()
        {
            base.Start();

            agent.enabled = false;

            //MouseControls.instance.HideCursor();
        }

        protected override void Update()
        {
            base.Update();

            if (isInBattle)
            {
                HandleBattleUpdate();
            }
            else
            {
                HandleRoamingUpdate();
            }
        }

        #region Roaming Functions
        public void HandleRoamingUpdate()
        {
            //handle movement
            HandleRoamingMovement();
        }

        public void HandleRoamingMovement()
        {
            HandleGroundedMovement();
            HandleJumpingMovement();
            HandleRoamingRotation();
            HandleFreeFallMovement();
        }

        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = inputHandler.VerticalInput;
            horizontalMovement = inputHandler.HorizontalInput;

            //clamp movements
        }

        public void HandleGroundedMovement()
        {
            if (canMove == false)
                return;

            GetVerticalAndHorizontalInputs();

            moveDirection = CameraController.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + CameraController.instance.transform.right * horizontalMovement;

            moveDirection.Normalize();
            moveDirection.y = 0;

            if (isSprinting)
            {
                characterController.Move(moveDirection * (charStats.moveSpeed * 1.5f) * Time.deltaTime);
            }
            else
            {
                if (inputHandler.moveAmount > 0.5f)
                {
                    //move at running speed
                    characterController.Move(moveDirection * (charStats.moveSpeed) * Time.deltaTime);
                }
                else if (inputHandler.moveAmount <= 0.5f)
                {
                    //walking speed
                    characterController.Move(moveDirection * (charStats.moveSpeed / 2) * Time.deltaTime);
                }
            }           
        }

        private void HandleJumpingMovement()
        {
            if (isJumping)
            {
                characterController.Move(jumpDirection * (charStats.moveSpeed) * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!isGrounded)
            {
                Vector3 freeFallDirection;
                freeFallDirection = CameraController.instance.transform.forward * inputHandler.VerticalInput;
                freeFallDirection += CameraController.instance.transform.right * inputHandler.HorizontalInput;
                freeFallDirection.y = 0;

                characterController.Move(freeFallDirection * (charStats.moveSpeed / 2) * Time.deltaTime);
            }
        }

        private void HandleRoamingRotation()
        {
            if (canRotate == false)
                return;

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = CameraController.instance.camObj.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + (CameraController.instance.camObj.transform.right * horizontalMovement);
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if(targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void HandleSprinting()
        {
            if (isPerformingAction)
            {
                isSprinting = false;
            }

            //if out of stamina set sprinting to false

            //if moving set sprinting to true
            if(moveAmount >= 0.5f)
            {
                isSprinting = true;
            }
            else
            {
                //if stationary set sprinting to false
                isSprinting = false;
            }

            
        }

        public void AttemptToPerformJump()
        {
            if (isPerformingAction)
                return;

            //stamina check

            if (isJumping)
                return;

            if (!isGrounded)
                return;

            //if using 2 handed use 2 handed jump anim

            //if using one handed weapon use 2 handed jump anim
            animationManager.PlayTargetActionAnimation("Jump_Start_01", false, false);

            isJumping = true;

            //minus stamina cost

            jumpDirection = CameraController.instance.transform.forward * inputHandler.VerticalInput;
            jumpDirection += CameraController.instance.transform.right * inputHandler.HorizontalInput;

            jumpDirection.y = 0;

            if(jumpDirection != Vector3.zero)
            {
                //jump distance based on speed
                if (isSprinting)
                {
                    jumpDirection *= 1;
                }
                else if (inputHandler.moveAmount > 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
                else if (inputHandler.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }

        public void ApplyjumpingVelocity()
        {
            //apply an upward velocity depending on forces e.g gravity
            yVelocity.y = Mathf.Sqrt(characterStats.JumpHeight * -2 * gravityForce);
        }



        //interaction
        public void CheckForInteractable()
        {
            List<Interactable> interactableList = new List<Interactable>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliders)
            {
                if(collider.TryGetComponent(out Interactable interactable))
                {
                    interactableList.Add(interactable);
                }
            }

            Interactable nearestInteractable = null;
            if (interactableList.Count > 0)
            {
                foreach (Interactable interactable in interactableList)
                {
                    if (nearestInteractable == null)
                    {
                        nearestInteractable = interactable;
                    }
                    else if ((interactable.transform.position - transform.position).sqrMagnitude < (nearestInteractable.transform.position - transform.position).sqrMagnitude)
                    {
                        nearestInteractable = interactable;
                    }
                }

                FaceTarget(nearestInteractable.gameObject.transform);
                isInteracting = true;
                MouseControls.instance.ShowCursor();
                nearestInteractable.Interact(transform);
            }
     
        }

        public Interactable GetInteractableObject()
        {
            List<Interactable> interactableList = new List<Interactable>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Interactable interactable))
                {
                    interactableList.Add(interactable);
                }
            }

            Interactable nearestInteractable = null;
            if (interactableList.Count > 0)
            {
                foreach (Interactable interactable in interactableList)
                {
                    if (nearestInteractable == null)
                    {
                        nearestInteractable = interactable;
                    }
                    else if((interactable.transform.position - transform.position).sqrMagnitude < (nearestInteractable.transform.position - transform.position).sqrMagnitude)
                    {
                        nearestInteractable = interactable;
                    }
                }               
            }

            return nearestInteractable;
        }

        public void InteractWithInteractable()
        {
            interactableObject.Interact();
        }

        void FaceTarget(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = lookRotation; //Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        #endregion





        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("BattleTrigger"))
            {
                //get battle trigger component
                BattleTrigger battleTrigger = collision.gameObject.GetComponent<BattleTrigger>();

                //Start battle enter
                StartCoroutine(BattleEntered(battleTrigger));
            }
        }

        IEnumerator BattleEntered(BattleTrigger battleTrigger)
        {
            isInBattle = true;
            agent.enabled = true;
            animationManager.UpdateAnimatorMovementParameters(0, 0, false);
            animationManager.SetInBattle(isInBattle);

            //change camera movetarget
            CameraController.instance.SetMoveTarget(transform.position);

            //stop moving
            agent.SetDestination(transform.position);
            moveTarget = transform.position;

            yield return new WaitForSeconds(3f);

            //disable battle trigger collider
            battleTrigger.DisableCollider();

            GameManager.instance.StartBattle(battleTrigger.EnemiesInBattle, this, battleTrigger);

        }

        public override void StartBattle()
        {
            base.StartBattle();

            MouseControls.instance.ShowCursor();

            agent.enabled = true;
            moveAmount = 0;
            animationManager.UpdateAnimatorMovementParameters(0, 0, false);
        }

        public override void StopBattle()
        {
            base.StopBattle();

            MouseControls.instance.HideCursor();

            agent.enabled = false;
        }

        private void OnDrawGizmos()
        {
        }
    }
}

