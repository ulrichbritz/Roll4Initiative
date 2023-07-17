using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class PlayerController : CharacterController
    {
        public static PlayerController instance;

        //Roaming
        [Header("Player Roaming Scripts")]
        InputHandler inputHandler;

        [Header("Movement")]
        [SerializeField] LayerMask movementMask;

        [Header("Interactable")]
        Transform target;
        public Interactable focus;
        


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

            inputHandler = GetComponent<InputHandler>();
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
            //animation
            if(agent.velocity.magnitude > 0)
            {
                animationManager.Anim.SetBool("isMoving", true);
            }
            else
            {
                animationManager.Anim.SetBool("isMoving", false);
            }

            //targetfollowing
            if(target != null)
            {
                agent.SetDestination(target.transform.position);
                FaceTarget();
            }
        }

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
                    focus.OnDefocused();

                focus = newFocus;
                Followtarget(focus);
            }
         
            newFocus.OnFocused(transform);         
        }

        public void RemoveFocus()
        {
            if(focus != null)
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

