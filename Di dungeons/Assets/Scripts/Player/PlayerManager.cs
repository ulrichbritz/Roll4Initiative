using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [HideInInspector] public CharacterController characterController;

        //Roaming
        [Header("Player Roaming Scripts")]
        InputHandler inputHandler;

        [Header("Movement")]
        [SerializeField] LayerMask movementMask;

        [Header("Interactable")]
        Transform target;
        public Interactable focus;

        [Header("Roaming Movement")]
        //settings
        [SerializeField] float rotationSpeed = 0.25f;

        //values
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        



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
            characterController = GetComponent<CharacterController>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();

        }

        private void Update()
        {
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
            HandleRoamingRotation();
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
                characterController.Move(moveDirection * (charStats.moveSpeed / 16) * Time.fixedDeltaTime);
            }
            else
            {
                if (inputHandler.moveAmount > 0.5f)
                {
                    //move at running speed
                    characterController.Move(moveDirection * (charStats.moveSpeed / 8) * Time.fixedDeltaTime);
                }
                else if (inputHandler.moveAmount <= 0.5f)
                {
                    //walking speed
                    characterController.Move(moveDirection * (charStats.moveSpeed / 12) * Time.fixedDeltaTime);
                }
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
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);
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





        //old
        public void StartMove(Vector2 mousePos)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, float.MaxValue, movementMask))
            {
                agent.SetDestination(hit.point);
            }
        }

        public void Followtarget(Interactable newTarget)
        {
            agent.stoppingDistance = newTarget.raduis;
            agent.updateRotation = false;

            target = newTarget.interactionPoint;
        }

        public void StopFollowingTarget()
        {
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;

            target = null;
        }

        public void SetFocus(Interactable newFocus)
        {
            if(newFocus != focus)
            {
                if(focus != null)
                {
                    focus.OnDefocused();
                }
                    

                focus = newFocus;
                Followtarget(focus);
            }
         
            newFocus.OnFocused(transform);         
        }

        public void RemoveFocus()
        {
            CameraController.instance.SetCameraState(CameraState.CameraRoamingState);

            if (focus != null)
                focus.OnDefocused();

            focus = null;
            StopFollowingTarget();
        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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
            //change camera movetarget
            CameraController.instance.SetMoveTarget(transform.position);

            //stop moving
            agent.SetDestination(transform.position);
            moveTarget = transform.position;
            animationManager.Anim.SetBool("isMoving", false);

            isInBattle = true;

            yield return new WaitForSeconds(3f);

            //disable battle trigger collider
            battleTrigger.DisableCollider();

            GameManager.instance.StartBattle(battleTrigger.EnemiesInBattle, this, battleTrigger);

        }
    }
}

