using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UB
{
    public class PlayerActionMenu : MonoBehaviour
    {
        public static PlayerActionMenu instance;

        [Header("Menus")]
        [SerializeField] GameObject actionCountUI;
        [SerializeField] GameObject actionMenu;

        [Header("ActionButtons")]
        [SerializeField] Button primaryActionButton;
        [SerializeField] Button secondaryActionButton;
        [SerializeField] Button nextTargetButton;

        [SerializeField] TextMeshProUGUI actionPointText;
        [SerializeField] TextMeshProUGUI movementRangeText;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public void UpdateActionPointText()
        {
            actionPointText.text = $"{GameManager.instance.ActionPointsRemaining}";
            movementRangeText.text = $"{GameManager.instance.activeCharacter.CharacterStats.remainingMoveRange}";
        }

        public void HideMenus()
        {
            actionMenu.SetActive(false);
            GameManager.instance.targetMarker.SetActive(false);
        }

        public void ShowActionMenu()
        {
            actionMenu.SetActive(true);
        }

        public void ShowActionCountUI()
        {
            actionCountUI.SetActive(true);
        }

        public void HideActionCountUI()
        {
            actionCountUI.SetActive(false);
        }

        public void PrimaryAttack()
        {
            HideMenus();
            MoveGrid.instance.HideMovePoints();

            StartCoroutine(GameManager.instance.activeCharacter.DoPrimaryAttack());

            GameManager.instance.targetMarker.SetActive(false);

            StartCoroutine(WaitToEndActionCo(6f, 1));

            primaryActionButton.enabled = false;
        }

        public void SecondaryAttack()
        {
            HideMenus();
            MoveGrid.instance.HideMovePoints();

            GameManager.instance.activeCharacter.DoSecondaryAttack();

            GameManager.instance.targetMarker.SetActive(false);

            StartCoroutine(WaitToEndActionCo(3f, 1));

            secondaryActionButton.enabled = false;
        }

        public void Ability1()
        {

        }

        public void Ability2()
        {

        }

        public void Ability3()
        {

        }

        public void Ability4()
        {

        }

        public void NextTarget()
        {

        }

        public void EndTurn()
        {
            MoveGrid.instance.HideMovePoints();
            GameManager.instance.EndTurn();
        }

        public void CheckAttackTargets()
        {
            var activeCharacter = GameManager.instance.activeCharacter;
            var targetMarker = GameManager.instance.targetMarker;

            //bonus action for bag

            #region Basic Attack Actions

            print(GameManager.instance.ActionPointsRemaining);
            //check if has attack actions
            if (GameManager.instance.ActionPointsRemaining <= 0)
            {
                //out of attack actions
                primaryActionButton.enabled = false;
                secondaryActionButton.enabled = false;
            }
            else
            {
                //Has attack actions remaining

                if (activeCharacter.allTargets.Count > 0)
                {
                    targetMarker.transform.position = activeCharacter.allTargets[activeCharacter.currentTarget].transform.position;
                    targetMarker.SetActive(true);
                }

                //melee attack
                if (activeCharacter.primaryAttackTargets.Count > 0)
                {
                    if (Vector3.Distance(activeCharacter.transform.position, activeCharacter.allTargets[activeCharacter.currentTarget].transform.position) < activeCharacter.CharacterStats.primaryAttackRange)
                    {
                        primaryActionButton.enabled = true;
                    }
                    else
                    {
                        primaryActionButton.enabled = false;
                    }
                }
                else
                {
                    primaryActionButton.enabled = false;
                }


                //ranged attack
                if (activeCharacter.secondaryAttackTargets.Count > 0)
                {
                    if (Vector3.Distance(activeCharacter.transform.position, activeCharacter.allTargets[activeCharacter.currentTarget].transform.position) < activeCharacter.CharacterStats.secondaryAttackRange)
                    {
                        Vector3 targetPoint = activeCharacter.allTargets[activeCharacter.currentTarget].hitPoint.transform.position;
                        Vector3 spawnPoint = activeCharacter.hitPoint.transform.position;
                        Vector3 shootDirection = (targetPoint - spawnPoint).normalized;

                        Debug.DrawRay(spawnPoint, shootDirection * activeCharacter.CharacterStats.secondaryAttackRange, Color.red, 1f);
                        RaycastHit hit;
                        if (Physics.Raycast(spawnPoint, shootDirection, out hit, activeCharacter.CharacterStats.secondaryAttackRange))
                        {
                            if (hit.collider.gameObject == activeCharacter.allTargets[activeCharacter.currentTarget].gameObject)
                            {
                                secondaryActionButton.enabled = true;
                            }
                            else
                            {
                                secondaryActionButton.enabled = false;
                            }
                        }
                    }
                    else
                    {
                        secondaryActionButton.enabled = false;
                    }

                }
                else
                {
                    secondaryActionButton.enabled = false;
                }
            }

            

            #endregion


            if (activeCharacter.allTargets.Count > 0)
            {
                nextTargetButton.enabled = true;
            }
            else
            {
                nextTargetButton.enabled = false;
                targetMarker.SetActive(false);
            }
        }

        public IEnumerator WaitToEndActionCo(float timeToWait, int turnPointsToSpend)
        {
            yield return new WaitForSeconds(timeToWait);

            GameManager.instance.SpendActionPoints(turnPointsToSpend);
        }

        public void TargetNextCharacter()
        {
            var activeCharacter = GameManager.instance.activeCharacter;
            var targetMarker = GameManager.instance.targetMarker;

            if(activeCharacter.allTargets.Count > 0)
            {
                activeCharacter.currentTarget++;

                if (activeCharacter.currentTarget >= activeCharacter.allTargets.Count)
                {
                    activeCharacter.currentTarget = 0;
                }

                targetMarker.transform.position = activeCharacter.allTargets[activeCharacter.currentTarget].transform.position;

                targetMarker.SetActive(true);
            }

            CheckAttackTargets();
        }
    }
}

