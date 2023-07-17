using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace UB
{
    public class InputHandler : MonoBehaviour
    {

        PlayerControls playerControls;

        [Header("Scripts")]
        PlayerController characterController;

        [Header("Movement Inputs")]
        Vector2 mousePos;
        bool moveInput = false;

        [Header("Action Inputs")]
        bool toggleInventoryInput = false;
        bool toggleEquipmentInput = false;

        private void Awake()
        {
            characterController = GetComponent<PlayerController>();
        }

        public void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                //movement
                playerControls.PlayerMovement.Movement.performed += inputAction => moveInput = true;

                //actions
                playerControls.PlayerActions.ToggleInventory.performed += inputAction => toggleInventoryInput = true;
                playerControls.PlayerActions.ToggleEquipment.performed += inputAction => toggleEquipmentInput = true;
            }
            
            playerControls.Enable();
        }

        private void Update()
        {
            //movement
            HandleMovementInput();

            //actions
            HandleToggleInventoryInput();
            HandleToggleEquipmentInput();
        }

        private void HandleMovementInput()
        {
            if (moveInput)
            {
                moveInput = false;

                //check if in battle
                if (characterController.isInBattle)
                    return;

                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                //remove player current focus
                characterController.RemoveFocus();

                //get mouse co-ords
                mousePos = playerControls.PlayerMovement.MousePosition.ReadValue<Vector2>();

                //check if clicked on interactable
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    //if hit interactable, set as focus
                    if (interactable != null)
                    {
                        characterController.SetFocus(interactable);
                    }
                    else
                    {
                        characterController.StartMove(mousePos);
                    }
                }                     
            }
        }

        private void HandleToggleInventoryInput()
        {
            if (toggleInventoryInput)
            {
                toggleInventoryInput = false;

                if (characterController.isInBattle)
                    return;

                //maybe remove this
                characterController.RemoveFocus();

                UIManager.instance.ToggleInventory();
            }      
        }

        private void HandleToggleEquipmentInput()
        {
            if (toggleEquipmentInput)
            {
                toggleEquipmentInput = false;

                if (characterController.isInBattle)
                    return;

                //maybe remove this
                characterController.RemoveFocus();

                UIManager.instance.ToggleEquipment();
            }
        }


    }
}

