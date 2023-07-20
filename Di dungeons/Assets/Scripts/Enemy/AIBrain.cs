using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UB
{
    public class AIBrain : MonoBehaviour
    {
        [Header("Scripts")]
        private CharacterManager characterController;

        [Header("Variables")]
        [SerializeField] float waitBeforeActing = 1f;

        public List<CharacterManager> targetsToPickFrom = new List<CharacterManager>();
        public List<CharacterManager> allPlayers = new List<CharacterManager>();

        private void Awake()
        {
            //scripts
            characterController = GetComponent<CharacterManager>();
        }

        public void ChooseAction()
        {
            Debug.Log(name + " is choosing an action");

            StartCoroutine(ChooseCo());
        }

        private IEnumerator ChooseCo()
        {
            yield return new WaitForSeconds(waitBeforeActing);

            bool actionTaken = false;

            //getting all player characters
            allPlayers.Clear();
            foreach(CharacterManager cc in GameManager.instance.allChars)
            {
                if(cc.isAI == false)
                {
                    if(Vector3.Distance(transform.position, cc.transform.position) < characterController.CharacterStats.alertRange)
                    {
                        allPlayers.Add(cc);
                    }                    
                }
            }

            characterController.GetAllTargetRange();

            if(characterController.primaryAttackTargets.Count > 0)
            {
                Debug.Log("Is using melee attack");

                targetsToPickFrom.Clear();
                targetsToPickFrom = characterController.primaryAttackTargets.OrderByDescending(ch => ch.CharacterStats.currentHP).ToList();
                CharacterManager useThisTarget = targetsToPickFrom[0];
                int targetCount = 0;
                foreach(CharacterManager cc in characterController.allTargets)
                {
                    if(characterController.allTargets[targetCount])
                    {
                        characterController.currentTarget = targetCount;

                        break;
                    }

                    targetCount++;
                }

                PlayerActionMenu.instance.PrimaryAttack();
                actionTaken = true;
            }
            else if(characterController.secondaryAttackTargets.Count > 0)
            {
                Debug.Log("is using ranged attack");

                targetsToPickFrom.Clear();
                targetsToPickFrom = characterController.secondaryAttackTargets.OrderByDescending(ch => ch.CharacterStats.currentHP).ToList();
                CharacterManager useThisTarget = targetsToPickFrom[0];
                int targetCount = 0;
                foreach (CharacterManager cc in characterController.allTargets)
                {
                    if (characterController.allTargets[targetCount] == useThisTarget)
                    {
                        characterController.currentTarget = targetCount;
                        break;
                    }

                    targetCount++;
                }

                PlayerActionMenu.instance.SecondaryAttack();
                actionTaken = true;
            }
            else if(allPlayers.Count > 0 && characterController.CharacterStats.remainingMoveRange >= 1)
            {
                int nearestPlayer = 0;

                for(int i = 1; i < allPlayers.Count; i++)
                {
                    if(Vector3.Distance(transform.position, allPlayers[nearestPlayer].transform.position) > Vector3.Distance(transform.position, allPlayers[i].transform.position))
                    {
                        nearestPlayer = i;
                    }
                }

                List<MovePoint> potentialMovePoints = new List<MovePoint>();
                int selectedMovePoint = 0;

                potentialMovePoints = MoveGrid.instance.GetMovePointsInRange(characterController.CharacterStats.remainingMoveRange, transform.position);

                float closestDistance = 1000f;

                for(int i = 0; i < potentialMovePoints.Count; i++)
                {
                    if (Vector3.Distance(allPlayers[nearestPlayer].transform.position, potentialMovePoints[i].transform.position) < closestDistance)
                    {
                        closestDistance = Vector3.Distance(allPlayers[nearestPlayer].transform.position, potentialMovePoints[i].transform.position);
                        selectedMovePoint = i;
                    }
                }

                characterController.MoveToPoint(potentialMovePoints[selectedMovePoint].transform.position);

                actionTaken = true;
            }
            else if(characterController.CharacterStats.remainingMoveRange >= 1)
            {
                List<MovePoint> randomPotentialMovePoints = new List<MovePoint>();
                randomPotentialMovePoints = MoveGrid.instance.GetMovePointsInRange(characterController.CharacterStats.remainingMoveRange, transform.position);

                int selectedMovePoint = Random.Range(0, randomPotentialMovePoints.Count);

                characterController.MoveToPoint(randomPotentialMovePoints[selectedMovePoint].transform.position);

                actionTaken = true;
            }

            if(actionTaken == false)
            {
                //skip turn
                GameManager.instance.EndTurn();
            }
        }

    }
}

