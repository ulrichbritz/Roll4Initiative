using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UB
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("StateCheckBool")]
        public bool isInBattle = false;

        [Header("isAI")]
        public bool isAI = true;

        [Header("Scrips")]
        [HideInInspector] public CharacterStats characterStats;
        [HideInInspector] public AnimationManager animationManager;
        [HideInInspector] private EquipmentManager equipmentManager;
        public CharacterStats CharacterStats => characterStats;
        private AIBrain aiBrain;
        public AIBrain AIBrain => aiBrain;
        PlayerInventory playerInventory;


        [Header("Components")]
        [HideInInspector]
        public NavMeshAgent agent;
        public GameObject hitPoint;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterController characterController;

        [Header("Movement")]
        [HideInInspector] public Vector3 moveTarget;
        bool isMoving;
        int moveRangeToSpend;

        [Header("Stats")]
        public int Initiative;

        [Header("Actions")]
        [HideInInspector]
        public List<CharacterManager> allTargets = new List<CharacterManager>();
        public List<CharacterManager> primaryAttackTargets = new List<CharacterManager>();
        public List<CharacterManager> secondaryAttackTargets = new List<CharacterManager>();
        [HideInInspector]
        public int currentTarget;

        [Header("Flags")]
        [HideInInspector] public bool isPerformingAction;
        [HideInInspector] public bool canRotate;
        [HideInInspector] public bool canMove;
        [HideInInspector] public bool applyRootMotion = false;
        [HideInInspector] public bool isSprinting = false;
         public bool isGrounded = false;
         public bool isJumping = false;

        [Header("Ground Check, Falling and Jumping")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRaduis = 0.5f;
        [SerializeField] protected Vector3 yVelocity; //force that pulls our character up or down (jump or fall)
        [SerializeField] protected float groundedYVelocity = -20f; //force at which char is sticking to ground while grounded
        [SerializeField] protected float fallStartYVelocity = -5f;
        protected bool fallingVelocirtyHasBeenSet = false;
        protected float inAirTimer = 0;

        protected virtual void Awake()
        {
            //components
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            characterController = GetComponent<CharacterController>();

            //scripts
            characterStats = GetComponent<CharacterStats>();
            animationManager = GetComponent<AnimationManager>();
            playerInventory = GetComponent<PlayerInventory>();
            equipmentManager = GetComponent<EquipmentManager>();
            if (GetComponent<AIBrain>() != null)
                aiBrain = GetComponent<AIBrain>();
        }

        protected virtual void Start()
        {
            moveTarget = transform.position;

            agent.speed = characterStats.moveSpeed;
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);

            HandleGroundCheck();
        }

        public void HandleGroundCheck()
        {
            isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRaduis, groundLayer);

            if (isGrounded)
            {
                //if not attempting to jump or move up run this
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocirtyHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                //if not jumping and falling velocity not set yet
                if (!isJumping && !fallingVelocirtyHasBeenSet)
                {
                    fallingVelocirtyHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer += Time.deltaTime;
                animator.SetFloat("inAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            //always a down force 
            if(!isInBattle)                         //fix because we want the enemy to fall down holes if blown back
                characterController.Move(yVelocity * Time.deltaTime);
        }

        public void HandleBattleUpdate()
        {
            //moving to point
            if (transform.position != moveTarget)
            {
                if (isMoving)
                {
                    CameraController.instance.SetMoveTarget(transform.position);

                    if (Vector3.Distance(transform.position, moveTarget) < .1f)
                    {
                        transform.position = moveTarget;
                        agent.SetDestination(transform.position);
                        isMoving = false;
                        animationManager.SetMovingBool(isMoving);
                        animationManager.UpdateAnimatorMovementParameters(0, 0, false);

                        GameManager.instance.FinishedMovement(moveRangeToSpend);
                        moveRangeToSpend = 0;
                    }
                }
            }

            if (isMoving == false && characterStats.isDead == false)
            {
                if (allTargets.Count > 0)
                {
                    transform.LookAt(allTargets[currentTarget].transform.position);
                }
            }
        }

        public void MoveToPoint(Vector3 pointToMoveTo)
        {
            moveRangeToSpend = Mathf.RoundToInt(Vector3.Distance(transform.position, pointToMoveTo));

            moveTarget = pointToMoveTo;

            agent.SetDestination(moveTarget);
            isMoving = true;
            animationManager.SetMovingBool(isMoving);
            animationManager.UpdateAnimatorMovementParameters(0, 1, false);
        }

        public void RollForInitiative()
        {
            Initiative = Random.Range(1, 20);
        }

        public virtual void StartBattle()
        {
            isInBattle = true;
            animationManager.SetInBattle(isInBattle);
            moveTarget = transform.position;
            animationManager.UpdateAnimatorMovementParameters(0, 0, false);
            RollForInitiative();
        }

        public virtual void StopBattle()
        {
            isInBattle = false;
            animationManager.SetInBattle(isInBattle);
            MoveGrid.instance.HideMovePoints();
            PlayerActionMenu.instance.HideActionCountUI();
            PlayerActionMenu.instance.HideMenus();
        }

        public void GetAllTargetRange()
        {
            allTargets.Clear();
            primaryAttackTargets.Clear();
            secondaryAttackTargets.Clear();

            if(isAI == false)
            {
                foreach(CharacterManager cc in GameManager.instance.allChars)
                {
                    if (cc.isAI)
                    {
                        allTargets.Add(cc);                      

                        if (Vector3.Distance(transform.position, cc.transform.position) < characterStats.primaryAttackRange)
                        {
                            primaryAttackTargets.Add(cc);
                        }
                        if (Vector3.Distance(transform.position, cc.transform.position) < characterStats.secondaryAttackRange)
                        {
                            secondaryAttackTargets.Add(cc);
                        }
                    }
                }
            }
            else
            {
                foreach (CharacterManager cc in GameManager.instance.allChars)
                {
                    if (cc.isAI == false)
                    {
                        allTargets.Add(cc);

                        if (Vector3.Distance(transform.position, cc.transform.position) < characterStats.primaryAttackRange)
                        {
                            primaryAttackTargets.Add(cc);
                        }
                        if (Vector3.Distance(transform.position, cc.transform.position) < characterStats.secondaryAttackRange)
                        {
                            secondaryAttackTargets.Add(cc);
                        }
                    }
                }
            }

            if(currentTarget >= allTargets.Count)
            {
                currentTarget = 0;
            }

        }

        public void DoPrimaryAttack()
        {
            CameraController.instance.SetActionView();

            animationManager.Anim.CrossFade(equipmentManager.rightWeapon.light_attack_01, 0.2f);

           // allTargets[currentTarget].GetComponent<CharacterStats>().TakeDamage(Mathf.RoundToInt(characterStats.damage));              
        }

        public void DoSecondaryAttack()
        {
            CameraController.instance.SetActionView();

           // allTargets[currentTarget].GetComponent<CharacterStats>().TakeDamage(Mathf.RoundToInt(characterStats.damage));
        }






        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, groundCheckSphereRaduis);
        }

    }
}
