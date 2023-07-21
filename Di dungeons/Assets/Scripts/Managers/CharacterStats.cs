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
        public Slider hpSlider;

        [Header("Info")]
        [HideInInspector]
        public bool isDead = false;
        [SerializeField] string characterName;
        public string CharacterName => characterName;

        public int level;

        [HideInInspector]
        public int maxHP;

        [HideInInspector]
        public int currentHP;

        public float MoveRange;

        //[HideInInspector]
        public float remainingMoveRange;

        public float moveSpeed = 4;

        public float alertRange = 20f;

        public int MaxActionPoints = 3;

        [SerializeField] float jumpHeight = 2f;
        [HideInInspector] public float JumpHeight => jumpHeight; 


        [Header("Stats")]
        [SerializeField] int strength;

        [SerializeField] int dexterity;

        [SerializeField] int intelligence;

        [Header("Weapon Stats")]
        public float primaryAttackRange = 5.5f;
        public float secondaryAttackRange = 1.5f;

        private void Awake()
        {
            characterController = GetComponent<CharacterManager>();
            animationManager = GetComponent<AnimationManager>();

            ResetInfoAmounts();
        }

        private void Start()
        {
            
        }

        private int SetMaxHP()
        {
            maxHP = 10 + (level * 5) + strength;
            return maxHP;
        }

        private float SetMoveRange()
        {
            MoveRange = 5 + Mathf.RoundToInt(dexterity / 5);
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
            hpText.text = $"{currentHP}/{maxHP}";

            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        public void ResetInfoAmounts()
        {
            maxHP = SetMaxHP();
            currentHP = maxHP;
            UpdateHPDisplay();

            MoveRange = SetMoveRange();
            ResetMoveRange();
        }

        public void ResetMoveRange()
        {
            remainingMoveRange = MoveRange;
        }
    }
}

