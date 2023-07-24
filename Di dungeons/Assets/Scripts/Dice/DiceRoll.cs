using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class Di
    {
        [SerializeField] List<GameObject> diModels;

        public int sides;
        public int rollValue;
        public GameObject diModel;

        public Di(int _sides)
        {
            sides = _sides;               
        }

        public void Roll()
        {
            rollValue = UnityEngine.Random.Range(1, sides + 1);
        }
    }

    public class DiceRoll
    {
        public List<Di> diceList;

        public DiceRoll()
        {
            diceList = new List<Di>();
        }

        public void AddDice(int _sides)
        {
            diceList.Add(new Di(_sides));
        }

        public void Roll()
        {
            for (int i = 0; i < diceList.Count; i++)
            {
                diceList[i].Roll();
            }
        }

        public void RollDice(int _diceNum)
        {
            diceList[_diceNum].Roll();
        }

        public int TotalValue()
        {
            int v = 0;

            for (int i = 0; i < diceList.Count; i++)
            {
                v += diceList[i].rollValue;
            }

            return v;
        }
    }
}

