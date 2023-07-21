using UnityEngine;

namespace UB
{
    public class Interactable : MonoBehaviour
    {
        public float raduis = 3f;
        public Transform interactionPoint;
        public string interactableText;

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


        

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionPoint.position, raduis);
        }
    }
}

