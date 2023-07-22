using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UB
{
    public class PlayerInteractUI : MonoBehaviour
    {
        [Header("Components")]
        //scripts
        private PlayerManager playerManager;

        [Header("UI")]
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI interactText;

        private void Start()
        {
            playerManager = PlayerManager.instance;
        }

        private void Update()
        {
            //fix later for efficiency
            if(playerManager.GetInteractableObject() != null)
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

