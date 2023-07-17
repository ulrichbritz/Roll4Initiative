using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        [Header("Components")]
        public Transform cam;

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

            moveTarget = transform.position;
        }

        private void Start()
        {
            //for battle
            targetCamViewAngle = 45f;
            normCamPos = cam.localPosition;
            actionCamPos = new Vector3(cam.localPosition.x, cam.localPosition.y - 3.5f, cam.localPosition.z + 2f);

            //for roaming
            roamingTarget = PlayerController.instance.transform;
        }

        private void LateUpdate()
        {
            if (PlayerController.instance.isInBattle)
            {
                HandleBattleUpdate();
            }
            else
            {
                HandleRoamingUpdate();
            }

           
        }

        private void HandleRoamingUpdate()
        {
            Vector3 targetPos = roamingTarget.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelociry, smoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, roamingTarget.eulerAngles.y , 0f), roamingRotateSpeed * Time.deltaTime);
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

            cam.localRotation = Quaternion.Slerp(cam.localRotation, Quaternion.Euler(targetCamViewAngle, 0f, 0f), battleRotateSpeed * Time.deltaTime);
        }

        public void SetMoveTarget(Vector3 newTarget)
        {
            moveTarget = newTarget;

            targetCamViewAngle = 45f;
            cam.localPosition = normCamPos;
            isActionView = false;
        }

        public void SetActionView()
        {
            var activeChar = GameManager.instance.activeCharacter;

            moveTarget = activeChar.transform.position;

            targetRotation = activeChar.transform.rotation.eulerAngles.y;

            targetCamViewAngle = actionCamViewAngle;
            cam.transform.localPosition = actionCamPos;

            isActionView = true;
        }
    }

}
