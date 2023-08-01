using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class PlayerHeadTargetCentered : MonoBehaviour
    {
        PlayerManager playerManager;

        [SerializeField] private Vector3 normalPos;

        private Transform mainCameraTransform;
        [SerializeField] private float maxDistance = 10f; // Maximum distance to place the target

        private void Start()
        {
            playerManager = PlayerManager.instance;
            // Find the main camera by tag (or you can assign it directly in the Inspector)
            mainCameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (!playerManager.isInBattle)
            {
                // Perform a raycast from the camera position along its forward direction
                if (Physics.Raycast(mainCameraTransform.position, mainCameraTransform.forward, out RaycastHit hit, maxDistance))
                {
                    // If the raycast hits something, move the target GameObject to the hit point
                    transform.position = hit.point;
                }
                else
                {
                    // If the raycast doesn't hit anything, move the target GameObject to the maximum distance
                    transform.position = mainCameraTransform.position + mainCameraTransform.forward * maxDistance;
                }
            }
            else
            {
                transform.position = normalPos;
            }
            
        }
    }
}

