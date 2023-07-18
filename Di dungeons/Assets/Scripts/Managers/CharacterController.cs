using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UB
{
    public class CharacterController : MonoBehaviour
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

        [Header("Movement")]
        [HideInInspector] public Vector3 moveTarget;
        bool isMoving;
        int moveRangeToSpend;

        [Header("Stats")]
        public int Initiative;

        [Header("Actions")]
        [HideInInspector]
        public List<CharacterController> allTargets = new List<CharacterController>();
        public List<CharacterController> primaryAttackTargets = new List<CharacterController>();
        public List<CharacterController> secondaryAttackTargets = new List<CharacterController>();
        [HideInInspector]
        public int currentTarget;

        protected virtual void Awake()
        {
            //components
            agent = GetComponent<NavMeshAgent>();

            //scripts
            characterStats = GetComponent<CharacterStats>();
            animationManager = GetComponent<AnimationManager>();
            playerInventory = GetComponent<PlayerInventory>();
            equipmentManager = GetComponent<EquipmentManager>();
            if (GetComponent<AIBrain>() != null)
                aiBrain = GetComponent<AIBrain>();
        }

        private void Start()
        {
            moveTarget = transform.position;

            agent.speed = characterStats.moveSpeed;
        }

        private void Update()
        {

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
                        animationManager.Anim.SetBool("isMoving", isMoving);

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
            animationManager.Anim.SetBool("isMoving", isMoving);
        }

        public void RollForInitiative()
        {
            Initiative = Random.Range(1, 20);
        }

        public void StopBattle()
        {
            isInBattle = false;
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
                foreach(CharacterController cc in GameManager.instance.allChars)
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
                foreach (CharacterController cc in GameManager.instance.allChars)
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

            animationManager.Anim.SetTrigger("doRangedAttack");

           // allTargets[currentTarget].GetComponent<CharacterStats>().TakeDamage(Mathf.RoundToInt(characterStats.damage));
        }

    }

}
