using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UB
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Battle Variables")]
        private BattleTrigger battleTriggerInUse;
        public CharacterControllerManager activeCharacter;
        public List<CharacterControllerManager> allChars = new List<CharacterControllerManager>();
        private int currentChar;
        private int totalActionPoints;
        int actionPointsRemaining;
        public int ActionPointsRemaining => actionPointsRemaining;
        public GameObject targetMarker;

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

        private void Start()
        {
            //StartBattle();
        }

        private void Update()
        {

        }

        //In Battle
        public void StartBattle(List<CharacterControllerManager> battleTriggerEnemies, CharacterControllerManager player, BattleTrigger battleTrigger)
        {
            PlayerActionMenu playerActionMenu = PlayerActionMenu.instance;
            playerActionMenu.ShowActionCountUI();

            CameraController.instance.SetCameraState(CameraState.CameraBattleState);

            battleTriggerInUse = battleTrigger;

            allChars.Clear();
            allChars.AddRange(battleTriggerEnemies);
            allChars.Add(player);

            foreach (CharacterControllerManager characterController in allChars)
            {
                characterController.isInBattle = true;
                characterController.RollForInitiative();
            }
            allChars = allChars.OrderByDescending(ch => ch.Initiative).ToList();

            activeCharacter = allChars[0];
            CameraController.instance.SetMoveTarget(activeCharacter.transform.position);

            currentChar = -1;
            EndTurn();
        }

        public void FinishedMovement(int moveRangeToSpend)
        {
            SpendMoveRange(moveRangeToSpend);
        }

        public void SpendActionPoints(int actionCost)
        {
            actionPointsRemaining -= actionCost;
            PlayerActionMenu playerActionMenu = PlayerActionMenu.instance;
            playerActionMenu.UpdateActionPointText();

            if (actionPointsRemaining <= 0)
            {
                MoveGrid moveGrid = MoveGrid.instance;

                if (activeCharacter.isAI == false)
                {
                    moveGrid.ShowPointsInRange(activeCharacter.CharacterStats.remainingMoveRange, activeCharacter.transform.position);
                    playerActionMenu.ShowActionMenu();
                }
                else
                {
                    EndTurn();
                }
            }
            else
            {
                GetRanges();
                MoveGrid moveGrid = MoveGrid.instance;

                if (activeCharacter.isAI == false)
                {
                    moveGrid.ShowPointsInRange(activeCharacter.CharacterStats.remainingMoveRange, activeCharacter.transform.position);
                    playerActionMenu.ShowActionMenu();
                }
                else
                {
                    playerActionMenu.HideMenus();
                    activeCharacter.AIBrain.ChooseAction();
                }
            }

            CameraController.instance.SetMoveTarget(activeCharacter.transform.position);
        }

        public void SpendMoveRange(int spentMovement)
        {
            var charStats = activeCharacter.CharacterStats;

            charStats.remainingMoveRange -= spentMovement;
            if (charStats.remainingMoveRange < 0)
                charStats.remainingMoveRange = 0;

            SpendActionPoints(0);
        }

        public void EndTurn()
        {
            currentChar++;

            if (currentChar >= allChars.Count)
            {
                currentChar = 0;
            }

            activeCharacter = allChars[currentChar];

            CameraController.instance.SetMoveTarget(activeCharacter.transform.position);

            totalActionPoints = activeCharacter.CharacterStats.MaxActionPoints;
            actionPointsRemaining = totalActionPoints;

            activeCharacter.CharacterStats.ResetMoveRange();

            PlayerActionMenu playerActionMenu = PlayerActionMenu.instance;
            playerActionMenu.UpdateActionPointText();

            GetRanges();

            if (activeCharacter.isAI == false)
            {
                MoveGrid.instance.ShowPointsInRange(activeCharacter.CharacterStats.remainingMoveRange, activeCharacter.transform.position);
                playerActionMenu.ShowActionMenu();
            }
            else
            {
                playerActionMenu.HideMenus();
                activeCharacter.AIBrain.ChooseAction();
            }
        }

        public IEnumerator AISkipCo()
        {
            yield return new WaitForSeconds(1f);
            EndTurn();
        }

        public void GetRanges()
        {
            activeCharacter.GetAllTargetRange();
            PlayerActionMenu.instance.CheckAttackTargets();
        }

        public void CheckForBattleOver()
        {
            int friendlyChars = 0;
            int enemyChars = 0;

            foreach (CharacterControllerManager cc in allChars)
            {
                if (cc.isAI)
                    enemyChars++;
                else
                    friendlyChars++;
            }

            if (enemyChars <= 0 && friendlyChars >= 1)
            {
                BattleOverPlayerWin();
            }
            else if (friendlyChars <= 0 && enemyChars >= 1)
            {
                BattleOverEnemyWin();
            }
        }

        private void BattleOverPlayerWin()
        {
            foreach (CharacterControllerManager cc in allChars)
            {
                if (cc.isAI == false)
                {
                    cc.StopBattle();
                }
            }

            HideMenusAndMoveGrid();
            CameraController.instance.SetCameraState(CameraState.CameraRoamingState);
        }

        private void BattleOverEnemyWin()
        {
            foreach (CharacterControllerManager cc in allChars)
            {
                cc.StopBattle();
            }

            HideMenusAndMoveGrid();
            CameraController.instance.SetCameraState(CameraState.CameraRoamingState);
        }

        private void HideMenusAndMoveGrid()
        {
            PlayerActionMenu playerActionMenu = PlayerActionMenu.instance;
            MoveGrid moveGrid = MoveGrid.instance;

            playerActionMenu.HideMenus();
            moveGrid.HideMovePoints();
            playerActionMenu.HideActionCountUI();
        }
    }

}
