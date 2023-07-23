using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UB
{
    public class PlayerInteractUI : MonoBehaviour
    {
        public static PlayerInteractUI instance;

        [Header("Components")]
        //scripts
        private PlayerManager playerManager;

        [Header("UI")]
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI interactText;

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

        private void Start()
        {
            playerManager = PlayerManager.instance;
        }

        private void Update()
        {
            //fix later for efficiency
            
        }

        public void SeeIfNearestInteractable(PlayerManager playerManager)
        {
            if (playerManager.GetInteractableObject() != null && playerManager.isInteracting == false)
            {
                Show(playerManager.GetInteractableObject());
            }
            else
            {
                Hide();
            }
        }

        public void Show(Interactable interactable)
        {
            container.SetActive(true);
            interactText.text = interactable.GetInteractText();
        }

        public void Hide()
        {
            container.SetActive(false);
        }
    }
}

