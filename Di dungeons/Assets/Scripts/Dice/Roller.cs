using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UB
{
    public class Roller : MonoBehaviour
    {
        public static Roller instance;

        [SerializeField] GameObject rollingSystemHolder;

        private List<GameObject> diceBeingRolled;

        [SerializeField] Transform diceHolder;
        [SerializeField] TextMeshProUGUI totalValueText;

        DiceRoll diceRoll;

        public int totalValue = 0;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void StartDiceRolling(List<GameObject> diceToRollList)
        {
            rollingSystemHolder.SetActive(true);

            foreach (DiceInformation child in diceHolder.GetComponentsInChildren<DiceInformation>())
            {
                Destroy(child.gameObject);
            }

            totalValue = 0;
            totalValueText.text = "";
            diceRoll = new DiceRoll();

            diceBeingRolled = new List<GameObject>();

            for(int i = 0; i < diceToRollList.Count; i++)
            {
                diceRoll.AddDice(diceToRollList[i].GetComponent<DiceInformation>().diceSides);
                GameObject thisDice = Instantiate(diceToRollList[i], diceHolder);
                diceBeingRolled.Add(thisDice);
            }

            for (int i = 0; i < diceBeingRolled.Count; i++)
            {
                if (i < diceRoll.diceList.Count)
                {
                    diceBeingRolled[i].GetComponent<DiceInformation>().valueText.text = "?";
                }
            }

            StartCoroutine(Roll());

        }

        private IEnumerator Roll()
        {
            yield return new WaitForSeconds(1f);

            diceRoll.Roll();

            UpdateText();

            totalValue = diceRoll.TotalValue();

            yield return new WaitForSeconds(2f);

            rollingSystemHolder.SetActive(false);
        }

        /*
        public void Roll()
        {
            diceRoll.Roll();

            UpdateText();
        }

        */

        private void UpdateText()
        {
            for(int i = 0; i < diceBeingRolled.Count; i++)
            {
                if(i < diceRoll.diceList.Count)
                {
                    diceBeingRolled[i].GetComponent<DiceInformation>().valueText.text = diceRoll.diceList[i].rollValue.ToString();
                }
            }

            totalValueText.text = $"Total Value = " + diceRoll.TotalValue().ToString();
        }

        public void ReRoll(int _diceNum)
        {
            diceRoll.RollDice(_diceNum);

            UpdateText();
        }
    }
}

