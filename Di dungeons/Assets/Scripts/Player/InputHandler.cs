using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UB
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;

        PlayerControls playerControls;

        [Header("Scripts")]
        PlayerController characterController;

        [Header("Movement Inputs")]
        Vector2 movementInput;
        [SerializeField] float horizontalInput;
        [HideInInspector] public float HorizontalInput => horizontalInput;
        [SerializeField] float verticalInput;
        [HideInInspector] public float VerticalInput => verticalInput;
        public float moveAmount;

        [Header("Camera Input")]
        Vector2 cameraMovementInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;


        [Header("Action Inputs")]
        bool toggleInventoryInput = false;
        bool toggleEquipmentInput = false;




        //old
        Vector2 mousePos;
        bool moveInput = false;

        

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

            characterController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            //DontDestroyOnLoad(gameObject);

            //SceneManager.activeSceneChanged += OnSceneChange;

            //instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if(newScene.buildIndex == 0 /*world/game scene*/)
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }

        public void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                //movement
                playerControls.PlayerMovement.Movement.performed += inputAction => movementInput = inputAction.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += inputAction => cameraMovementInput = inputAction.ReadValue<Vector2>();
                //playerControls.PlayerMovement.Movement.performed += inputAction => moveInput = true;

                //actions
                playerControls.PlayerActions.ToggleInventory.performed += inputAction => toggleInventoryInput = true;
                playerControls.PlayerActions.ToggleEquipment.performed += inputAction => toggleEquipmentInput = true;
            }
            
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void OnDestroy()
        {
            //sceneManager.activescenechanged -= onscenechanged
            
        }

        private void Update()
        {
            //movement
            HandlePlayerMovementInput();
            HandleCameraMovementInput();

            //actions
            HandleToggleInventoryInput();
            HandleToggleEquipmentInput();
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            //returns absolute number (always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + (Mathf.Abs(horizontalInput)));

            if(moveAmount <= 0.5f && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1;
            }
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraMovementInput.y;
            cameraHorizontalInput = cameraMovementInput.x;
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

