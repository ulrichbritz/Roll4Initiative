using UnityEngine;

namespace UB
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private string interactText;

        public float raduis = 3f;
        public Transform interactionPoint;

        bool isFocus = false;
        Transform player;

        bool hasInteracted = false;

        PlayerManager playerManager;

        public virtual void Interact(Transform interactorTransform = null)
        {
            Debug.Log("Interacted with " + gameObject.name);

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
            
        }

        public string GetInteractText()
        {
            return interactText;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<PlayerManager>() != null)
            {
                playerManager = other.gameObject.GetComponent<PlayerManager>();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(playerManager != null)
            {
                PlayerInteractUI.instance.SeeIfNearestInteractable(playerManager);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.GetComponent<PlayerManager>() == playerManager)
            {
                playerManager = null;
            }
        }




        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionPoint.position, raduis);
        }
    }
}

