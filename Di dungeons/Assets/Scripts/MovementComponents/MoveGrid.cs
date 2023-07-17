using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class MoveGrid : MonoBehaviour
    {
        public static MoveGrid instance;

        [SerializeField] MovePoint startPoint;

        public Vector2Int spawnRange;

        [SerializeField] LayerMask whatIsNotWalkable, whatIsObstacle;

        [SerializeField] float obstacleCheckRange;

        [SerializeField] List<MovePoint> allMovePoints = new List<MovePoint>();

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

            //in awake to prevent scripts from accessing before set up
            GenerateMoveGrid();

            HideMovePoints();
        }

        private void Start()
        {

        }

        public void GenerateMoveGrid()
        {
            for (int x = -spawnRange.x; x <= spawnRange.x; x++)
            {
                for (int y = -spawnRange.y; y <= spawnRange.y; y++)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + new Vector3(x, 10f, y), Vector3.down, out hit, 20f, whatIsNotWalkable))
                    {
                        if (Physics.OverlapSphere(hit.point, obstacleCheckRange, whatIsObstacle).Length == 0)
                        {
                            MovePoint newPoint = Instantiate(startPoint, hit.point, transform.rotation);
                            newPoint.transform.SetParent(transform);

                            allMovePoints.Add(newPoint);
                        }
                    }
                }
            }

            startPoint.gameObject.SetActive(false);
        }

        public void HideMovePoints()
        {
            foreach (MovePoint movePoint in allMovePoints)
            {
                movePoint.gameObject.SetActive(false);
            }
        }

        public void ShowPointsInRange(float moveRange, Vector3 centerPoint)
        {
            HideMovePoints();

            transform.position = GameManager.instance.activeCharacter.transform.position;

            foreach (MovePoint movePoint in allMovePoints)
            {
                if (Vector3.Distance(centerPoint, movePoint.transform.position) <= moveRange)
                {
                    movePoint.gameObject.SetActive(true);

                    foreach (CharacterController cc in GameManager.instance.allChars)
                    {
                        if (Vector3.Distance(cc.transform.position, movePoint.transform.position) < 0.5)
                        {
                            movePoint.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        public List<MovePoint> GetMovePointsInRange(float moveRange, Vector3 centerPoint)
        {
            List<MovePoint> foundPoints = new List<MovePoint>();

            transform.position = GameManager.instance.activeCharacter.transform.position;

            foreach (MovePoint movePoint in allMovePoints)
            {
                if (Vector3.Distance(centerPoint, movePoint.transform.position) <= moveRange)
                {
                    bool shouldAdd = true;

                    foreach (CharacterController cc in GameManager.instance.allChars)
                    {
                        if (Vector3.Distance(cc.transform.position, movePoint.transform.position) < 0.5)
                        {
                            shouldAdd = false;
                        }
                    }

                    if(shouldAdd == true)
                    {
                        foundPoints.Add(movePoint);
                    }
                }
            }

            return foundPoints;
        }
    }
}

