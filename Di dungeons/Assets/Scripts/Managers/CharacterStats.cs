using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UB
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Scripts")]
        [HideInInspector]
        public CharacterManager characterController;
        [HideInInspector]
        public AnimationManager animationManager;

        [Header("Components")]
        public TMP_Text hpText;
        public TMP_Text acText;
        public Slider hpSlider;

        [Header("Info")]
        [SerializeField] string characterName;
        public string CharacterName => characterName;

        [Header("Stats")]
        private int level = 1;
        [HideInInspector] public int Level => level;

        private int maxHP;
        [HideInInspector] public int MaxHP => maxHP;

        [HideInInspector]
        public int currentHP;

        public float MoveRange;

        [HideInInspector] public float remainingMoveRange;

        public float moveSpeed = 4;

        public float alertRange = 20f;

        public int MaxActionPoints = 3;

        [SerializeField] float jumpHeight = 2f;
        [HideInInspector] public float JumpHeight => jumpHeight;

        [HideInInspector]
        public bool isDead = false;


        [Header("Stats")]
        [SerializeField] int strength;
        [SerializeField] int intelligence;
        [SerializeField] int agility;
        [SerializeField] int charisma;
        [SerializeField] int willpower;

        [HideInInspector] public int initiativeModifier;
        [HideInInspector] public int strengthModifier;
        [HideInInspector] public int intelligenceModifier;
        [HideInInspector] public int agilityModifier;
        [HideInInspector] public int charismaModifier;
        [HideInInspector] public int willpowerModifier;

        [Header("Weapon Stats")]
        public float primaryAttackRange = 5.5f;
        public float secondaryAttackRange = 1.5f;

        private void Awake()
        {
            characterController = GetComponent<CharacterManager>();
            animationManager = GetComponent<AnimationManager>();        
        }

        private void Start()
        {
            SetInitialInfoAmounts();
        }

        private int SetMaxHP()
        {
            maxHP = 10 + (level * 5) + strength;
            return maxHP;
        }

        private float SetMoveRange()
        {
            MoveRange = 5 + Mathf.RoundToInt(agility / 5);
            return MoveRange;
        }

        public void TakeDamage(int damage)
        {
            animationManager.Anim.SetTrigger("doHurt");
            currentHP = currentHP - damage;
            UpdateHPDisplay();

            if (currentHP <= 0)
            {
                currentHP = 0;
                characterController.agent.enabled = false;

                Die();
            }
               
        }

        public virtual void Die()
        {
            animationManager.Anim.SetBool("isDead", true);
        }

        public void UpdateHPDisplay()
        {
            if(hpText != null)
            {
                hpText.text = $"HP: {currentHP}/{maxHP}";
                acText.text = $"AC: { GetComponent<CharacterManager>().GetOverallArmorCount()}";

                hpSlider.maxValue = maxHP;
                hpSlider.value = currentHP;
            }
            
        }

        public void SetInitialInfoAmounts()
        {
            maxHP = SetMaxHP();
            currentHP = maxHP;
            UpdateHPDisplay();

            MoveRange = SetMoveRange();
            ResetMoveRange();
        }

        public int GetStatInitiativeModifier()
        {
            initiativeModifier = Mathf.FloorToInt(agility / 10);
            return initiativeModifier;
        }

        public int GetStatStrengthModifier()
        {
            strengthModifier = Mathf.FloorToInt(strength / 10);
            return strengthModifier;
        }

        public int GetStatIntelligenceModifier()
        {
            intelligenceModifier = Mathf.FloorToInt(intelligence / 10);
            return intelligenceModifier;
        }

        public int GetStatAgilityModifier()
        {
            agilityModifier = Mathf.FloorToInt(agility / 10);
            return agilityModifier;
        }

        public int GetStatCharismaModifier()
        {
            charismaModifier = Mathf.FloorToInt(charisma / 10);
            return charismaModifier;
        }

        public int GetStatWillpowerModifier()
        {
            willpowerModifier = Mathf.FloorToInt(willpower / 10);
            return willpowerModifier;
        }

        public void ResetMoveRange()
        {
            remainingMoveRange = MoveRange;
        }
    }
}

