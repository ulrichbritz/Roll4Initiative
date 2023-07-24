using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class Dice
    {
        public int sides;
        public int rollValue;

        public Dice(int _sides)
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
        public List<Dice> diceList;

        public DiceRoll()
        {
            diceList = new List<Dice>();
        }

        public void AddDice(int _sides)
        {
            diceList.Add(new Dice(_sides));
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

