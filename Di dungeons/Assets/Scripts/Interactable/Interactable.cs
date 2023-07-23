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


        

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionPoint.position, raduis);
        }
    }
}

