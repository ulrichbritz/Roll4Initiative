using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        private CameraState state;

        [Header("Components")]
        //scripts
        PlayerManager playerManager;

        //components
        public Transform cameraSystemObj;
        public Transform cameraPivotObj;
        public Transform camObj;

        [Header("Roaming CameraSettings")]
        //roaming
        [SerializeField] private float cameraSmoothSpeed = 1;
        [SerializeField] private float upAndDownRotationSpeed = 220f;
        [SerializeField] private float leftAndRightRotationSpeed = 220f;
        [SerializeField] private float minPivot = -30f; //lowest able to look down
        [SerializeField] private float maxPivot = 60f; //highest able to look up

        [SerializeField] Vector3 roamingCameraSystemTransform;
        [SerializeField] Vector3 roamingCameraPivotTransform;
        [SerializeField] Vector3 roamingCamTransform;

        [SerializeField] float cameraCollisionRaduis = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Roaming Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; //used for collision, moves cam to this pos
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPos;
        private float targetCameraZPos;


        //old


        public float actionCamViewAngle = 30f;
        private float targetCamViewAngle;
        private bool isActionView;

        private Vector3 normCamPos;
        private Vector3 actionCamPos;


        [Header("In Battle")]
        [SerializeField] float moveSpeed, manualMoveSpeed = 5f;
        private Vector3 moveTarget;

        private Vector2 moveInput;

        private float targetRotation;
        [SerializeField] float battleRotateSpeed;

        private float currentAngle;

        [Header("Roaming")]
        [SerializeField] private Vector3 offset;
        private Transform roamingTarget;
        [SerializeField] float smoothTime;
        private Vector3 currentVelociry = Vector3.zero;
        [SerializeField] float roamingRotateSpeed = 2.5f;


        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            //scripts
            playerManager = PlayerManager.instance;

            moveTarget = transform.position;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetCameraState(CameraState.CameraRoamingState);
            /*
            //for battle
            targetCamViewAngle = 45f;
            normCamPos = cam.localPosition;
            actionCamPos = new Vector3(cam.localPosition.x, cam.localPosition.y - 3.5f, cam.localPosition.z + 2f);

            //for roaming
            roamingTarget = PlayerController.instance.transform;
            */
        }

        private void LateUpdate()
        {
            if(state == CameraState.CameraRoamingState)
            {
                HandleRoamingUpdate();
            }
            else if(state == CameraState.CameraBattleState)
            {
                HandleBattleUpdate();
            }
            else if(state == CameraState.CameraInteractionState)
            {
                HandleInteractionUpdate();
            }

            HandleCameraCollisions();
           
        }

        public void SetCameraState(CameraState camState)
        {
            state = camState;
            
            if(state == CameraState.CameraRoamingState)
            {
                cameraSystemObj.transform.position = roamingCameraSystemTransform;
                cameraPivotObj.transform.position = roamingCameraPivotTransform;
                camObj.transform.position = roamingCamTransform;
                cameraZPos = camObj.transform.localPosition.z;
                print("changed camera to roaming state");
            }
        }

        private void HandleCameraCollisions()
        {
            targetCameraZPos = cameraZPos;

            RaycastHit hit;
            Vector3 direction = camObj.transform.position - cameraPivotObj.transform.position;
            direction.Normalize();

            //check if object infront of cam
            if (Physics.SphereCast(cameraPivotObj.position, cameraCollisionRaduis, direction, out hit, Mathf.Abs(targetCameraZPos), 0))
            {
                //if there is , get distance from it
                float distanceFromHitObj = Vector3.Distance(cameraPivotObj.position, hit.point);
                targetCameraZPos = -(distanceFromHitObj - cameraCollisionRaduis);
            }

            //if target pos is less that collision raduis, subtract our collision raduis making it snap back
            if(Mathf.Abs(targetCameraZPos) < cameraCollisionRaduis)
            {
                targetCameraZPos = -cameraCollisionRaduis;
            }

            //apply final pos using lerp over time
            cameraObjectPosition.z = Mathf.Lerp(camObj.transform.localPosition.z, targetCameraZPos, 0.2f);
            camObj.transform.localPosition = cameraObjectPosition;
        }

        private void HandleRoamingUpdate()
        {
            if(playerManager != null)
            {
                HandleRoamingFollowPlayer();
                HandleRoamingRotation();
            }         
        }

        private void HandleRoamingFollowPlayer()
        {
            Vector3 targetCameraPos = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.fixedDeltaTime);
            transform.position = targetCameraPos;
        }

        private void HandleRoamingRotation()
        {
            //locked on rotations

            //normal rotations
            leftAndRightLookAngle += (InputHandler.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.fixedDeltaTime;
            upAndDownLookAngle -= (InputHandler.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.fixedDeltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minPivot, maxPivot);


            Vector3 cameraRotation;
            Quaternion targetRotation;

            //rotate this gameobject (cameraSystem) left and right
            cameraRotation = Vector3.zero;
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //rotate the pivot gameobject up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotObj.localRotation = targetRotation;
        }

        private void HandleBattleUpdate()
        {
            if (moveTarget != transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            }

            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.y = Input.GetAxis("Vertical");
            moveInput.Normalize();

            if (moveInput != Vector2.zero)
            {
                transform.position += ((transform.forward * (moveInput.y * manualMoveSpeed)) + (transform.right * (moveInput.x * manualMoveSpeed))) * Time.deltaTime;

                moveTarget = transform.position;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetMoveTarget(GameManager.instance.activeCharacter.transform.position);
                currentAngle = 0;
            }

            if (Input.GetKey(KeyCode.E))
            {
                currentAngle++;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                currentAngle--;
            }

            if (isActionView == false)
            {
                targetRotation = currentAngle + 45f;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, targetRotation, 0f), battleRotateSpeed * Time.deltaTime);

            camObj.localRotation = Quaternion.Slerp(camObj.localRotation, Quaternion.Euler(targetCamViewAngle, 0f, 0f), battleRotateSpeed * Time.deltaTime);
        }

        private void HandleInteractionUpdate()
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, PlayerController.instance.transform.eulerAngles.y, 0f), roamingRotateSpeed * Time.deltaTime);
        }

        public void SetMoveTarget(Vector3 newTarget)
        {
            moveTarget = newTarget;

           // targetCamViewAngle = 45f;
           // cam.localPosition = normCamPos;
            isActionView = false;
        }

        public void SetActionView()
        {
            var activeChar = GameManager.instance.activeCharacter;

            moveTarget = activeChar.transform.position;

            targetRotation = activeChar.transform.rotation.eulerAngles.y;

            targetCamViewAngle = actionCamViewAngle;
            camObj.transform.localPosition = actionCamPos;

            isActionView = true;
        }
    }

    public enum CameraState { CameraRoamingState, CameraBattleState, CameraInteractionState}

}
