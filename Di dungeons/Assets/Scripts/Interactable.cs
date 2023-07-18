using UnityEngine;

namespace UB
{
    public class Interactable : MonoBehaviour
    {
        public float raduis = 3f;
        public Transform interactionPoint;

        bool isFocus = false;
        Transform player;

        bool hasInteracted = false;

        public virtual void Interact()
        {
            Debug.Log("Interacted with " + gameObject.name);

            CameraController.instance.SetCameraState(CameraState.CameraInteractionState);
        }

        private void Awake()
        {
            if(interactionPoint == null)
            {
                interactionPoint = transform;
            }
        }

        private void Update()
        {
            if (isFocus && !hasInteracted)
            {
                float distance = Vector3.Distance(player.position, interactionPoint.position);
                if(distance <= raduis)
                {
                    Interact();
                    hasInteracted = true;
                }
            }
        }

        public void OnFocused (Transform playerTransform)
        {
            isFocus = true;
            player = playerTransform;
            hasInteracted = false;
        }

        public void OnDefocused()
        {
            isFocus = false;
            player = null;
            hasInteracted = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionPoint.position, raduis);
        }
    }
}

