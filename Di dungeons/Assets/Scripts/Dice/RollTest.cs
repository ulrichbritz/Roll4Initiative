using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UB
{
    public class RollTest : MonoBehaviour
    {
        [SerializeField] List<TextMeshProUGUI> texts;
        [SerializeField] TextMeshProUGUI totalValue;

        DiceRoll diceRoll;

        private void Start()
        {
            diceRoll = new DiceRoll();
            for(int i = 0; i < 5; i++)
            {
                diceRoll.AddDice(6);
            }

            for (int i = 0; i < texts.Count; i++)
            {
                if (i < diceRoll.diceList.Count)
                {
                    texts[i].text = diceRoll.diceList[i].sides.ToString();
                }
            }
        }

        public void Roll()
        {
            diceRoll.Roll();

            UpdateText();
        }

        private void UpdateText()
        {
            for(int i = 0; i < texts.Count; i++)
            {
                if(i < diceRoll.diceList.Count)
                {
                    texts[i].text = diceRoll.diceList[i].rollValue.ToString();
                }
            }

            totalValue.text = $"Total Value = " + diceRoll.TotalValue().ToString();
        }

        public void ReRoll(int _diceNum)
        {
            diceRoll.RollDice(_diceNum);

            UpdateText();
        }
    }
}

