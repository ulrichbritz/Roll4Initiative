using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class Testing : MonoBehaviour
    {
        public static Testing instance;

        [SerializeField]Transform player;
        Grid grid;

        private void Start()
        {
            float moveRange = (player.GetComponent<CharacterStats>().MoveRange);
            grid = new Grid(Mathf.RoundToInt(moveRange), Mathf.RoundToInt(moveRange), 1f, new Vector3(player.position.x - (moveRange/2), player.position.y, player.position.z - (moveRange/2)));
        }

        public void MakeGrid()
        {
            float moveRange = (player.GetComponent<CharacterStats>().MoveRange);
            grid = new Grid(Mathf.RoundToInt(moveRange), Mathf.RoundToInt(moveRange), 1f, new Vector3(player.position.x - (moveRange / 2), player.position.y, player.position.z - (moveRange / 2)));
        }
       

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = GetMouseWorldPos();
                int value = grid.GetValue(position);
                grid.SetValue(position, value + 5);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(grid.GetValue(GetMouseWorldPos()));
            }
        }

        Vector3 GetMouseWorldPos()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                if (hit.collider != null)
                {
                    return hit.point;
                }
            }

            return Vector3.zero;
        }   
        

    }
}

