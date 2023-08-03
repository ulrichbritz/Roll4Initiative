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
        TextBoard textBoard;
        WeaponSlotManager weaponSlotManager;


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
        public List<GameObject> d20;

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
        }

        protected virtual void Start()
        {
            //scripts
            characterStats = GetComponent<CharacterStats>();
            animationManager = GetComponent<AnimationManager>();
            playerInventory = GetComponent<PlayerInventory>();
            equipmentManager = GetComponent<EquipmentManager>();
            if (GetComponent<AIBrain>() != null)
                aiBrain = GetComponent<AIBrain>();
            textBoard = TextBoard.instance;
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

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

                    if (Vector3.Distance(transform.position, moveTarget) < .2f)
                    {
                        transform.position = moveTarget;
                        agent.SetDestination(transform.position);
                        isMoving = false;
                        animationManager.SetMovingBool(isMoving);
                        animationManager.UpdateAnimatorMovementParameters(0, 0, false);

                        textBoard.CreateUpdateMessage($"{gameObject.name} spent {moveRangeToSpend} movement", Color.blue);
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

            float posX = Mathf.RoundToInt(transform.position.x + 0.5f) - 0.5f;
            float posZ = Mathf.RoundToInt(transform.position.z + 0.5f) - 0.5f;
            transform.position = new Vector3(posX, transform.position.y, posZ);

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

        public IEnumerator DoPrimaryAttack()
        {

            int enemyAC = allTargets[currentTarget].GetComponent<CharacterManager>().GetOverallArmorCount();

            textBoard.CreateUpdateMessage($"{gameObject.name} uses primary attack {allTargets[currentTarget].name}", Color.blue);

            CameraController.instance.SetActionView();

            Roller.instance.StartDiceRolling(d20);

            yield return new WaitForSeconds(3.5f);

            if(Roller.instance.totalValue > enemyAC)
            {
                textBoard.CreateUpdateMessage($"{gameObject.name} rolls {Roller.instance.totalValue} to hit and HITS!", Color.cyan);
                WeaponItem usedWeapon = equipmentManager.CurrentEquipment[(int)EquipmentSlot.Weapon] as WeaponItem;
                Roller.instance.StartDiceRolling(usedWeapon.weaponDiceList);

                yield return new WaitForSeconds(3.5f);

                usedWeapon.damage = Roller.instance.totalValue;
                weaponSlotManager.SetRightHandDamageColliderDamage(Roller.instance.totalValue);
                textBoard.CreateUpdateMessage($"{gameObject.name} rolls {Roller.instance.totalValue} and deals {Roller.instance.totalValue} DAMAGE", Color.cyan);

                animationManager.Anim.CrossFade(equipmentManager.rightWeapon.light_attack_01, 0.2f);
            }
            else
            {
                textBoard.CreateUpdateMessage($"{gameObject.name} rolls {Roller.instance.totalValue} to hit and MISSES!", new Color(251, 134, 55));
            }

            StartCoroutine(PlayerActionMenu.instance.WaitToEndActionCo(1.5f, 1));


            // allTargets[currentTarget].GetComponent<CharacterStats>().TakeDamage(Mathf.RoundToInt(characterStats.damage));              
        }

        public void DoSecondaryAttack()
        {
            CameraController.instance.SetActionView();

           // allTargets[currentTarget].GetComponent<CharacterStats>().TakeDamage(Mathf.RoundToInt(characterStats.damage));
        }

        public int GetOverallArmorCount()
        {
            int armorCount = 0;

            foreach(Equipment equipment in equipmentManager.CurrentEquipment)
            {
                if(equipment != null)
                    armorCount += equipment.armorModifier;
            }

            return armorCount;
        }

        public int GetOverallStrengthModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.strengthModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatStrengthModifier() + equipmentMod);
        }

        public void GetOverAllInitiativeModifier()
        {

        }

        public int GetOverallIntelligenceModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.intelligenceModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatIntelligenceModifier() + equipmentMod);
        }

        public int GetOverallAgilityModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.agilityModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatAgilityModifier() + equipmentMod);
        }

        public int GetOverallCharismaModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.charismaModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatCharismaModifier() + equipmentMod);
        }

        public int GetOverallWillpowerModifier()
        {
            int equipmentMod = 0;

            foreach (Equipment equipment in equipmentManager.CurrentEquipment)
            {
                equipmentMod += equipment.willpowerModifier;
            }

            return Mathf.FloorToInt(characterStats.GetStatWillpowerModifier() + equipmentMod);
        }






        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, groundCheckSphereRaduis);
        }

    }
}
