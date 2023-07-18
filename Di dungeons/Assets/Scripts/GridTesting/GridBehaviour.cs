using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class GridBehaviour : MonoBehaviour
    {
        public bool findDistance = false;
        public int rows;
        public int columns;
        public int scale = 1;
        public GameObject gridPrefab;
        public Vector3 leftBottomLocation = new Vector3(0, 0, 0);

        public GameObject[,] gridArray;
        public int startX = 0;
        public int startY = 0;
        public int endX = 2;
        public int endY = 2;
        public List<GameObject> path = new List<GameObject>();


        private void Awake()
        {

            gridArray = new GameObject[columns, rows];

            if (gridPrefab)
            {
                GenerateGrid();
            }
            else
            {
                Debug.Log("Missing Grid Prefab");
            }
        }

        private void Update()
        {
            if (findDistance)
            {
                SetDistance();
                SetPath();
                findDistance = false;
            }
        }

        void GenerateGrid()
        {
            for(int x = 0; x < columns; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * x, leftBottomLocation.y, leftBottomLocation.z + scale * y), Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform);
                    obj.GetComponent<GridStats>().x = x;
                    obj.GetComponent<GridStats>().y = y;

                    gridArray[x, y] = obj;
                }
            }
        }

        void SetDistance()
        {
            InitialSetup();
            int x = startX;
            int y = startY;
            int[] testArray = new int[rows * columns];
            for (int step = 1; step < rows * columns; step++)
            {
                foreach(GameObject obj in gridArray)
                {
                    if(obj && obj.GetComponent<GridStats>().visited == step - 1)
                    {
                        TestFourDirections(obj.GetComponent<GridStats>().x, obj.GetComponent<GridStats>().y, step);
                    }
                }
            }
        }

        void SetPath()
        {
            int step;
            int x = endX;
            int y = endY;
            List<GameObject> tempList = new List<GameObject>();
            path.Clear();
            if(gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStats>().visited > 0)
            {
                path.Add(gridArray[x, y]);
                step = gridArray[x, y].GetComponent<GridStats>().visited - 1;
            }
            else
            {
                Debug.Log("Cant reach desired location");
                return;
            }

            for(int i = step; step > -1; step--)
            {
                if (TestDirection(x, y, step, 1))
                {
                    tempList.Add(gridArray[x, y + 1]);
                }
                if (TestDirection(x, y, step, 2))
                {
                    tempList.Add(gridArray[x + 1, y]);
                }
                if (TestDirection(x, y, step, 3))
                {
                    tempList.Add(gridArray[x, y - 1]);
                }
                if (TestDirection(x, y, step, 4))
                {
                    tempList.Add(gridArray[x - 1, y]);
                }

                GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
                path.Add(tempObj);
                x = tempObj.GetComponent<GridStats>().x;
                y = tempObj.GetComponent<GridStats>().y;
                tempList.Clear();
            }      
        }

        void InitialSetup()
        {
            foreach(GameObject obj in gridArray)
            {
                obj.GetComponent<GridStats>().visited = -1;
            }
            gridArray[startX, startY].GetComponent<GridStats>().visited = 0;
        }

        bool TestDirection(int x, int y, int step, int direction)
        {
            //int direction tells us which case to use
            switch (direction)
            {
                case 1: //up
                    if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStats>().visited == step)
                        return true;
                    else return false;
                case 2: //right
                    if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStats>().visited == step)
                        return true;
                    else return false;
                case 3: //down
                    if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().visited == step)
                        return true;
                    else return false;
                case 4: //left
                    if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStats>().visited == step)
                        return true;
                    else return false;

            }
            return false;
        }

        void TestFourDirections(int x, int y, int step)
        {
            if(TestDirection(x, y, -1, 1))
            {
                SetVisited(x, y + 1, step);
            }

            if(TestDirection(x,y, -1, 2))
            {
                SetVisited(x + 1, y, step);
            }

            if(TestDirection(x, y, -1, 3))
            {
                SetVisited(x, y - 1, step);
            }

            if(TestDirection(x, y, -1, 4))
            {
                SetVisited(x - 1, y, step);
            }
        }

        void SetVisited(int x, int y, int step)
        {
            if (gridArray[x, y])
                gridArray[x, y].GetComponent<GridStats>().visited = step;
        }

        GameObject FindClosest(Transform targetLocation, List<GameObject> list)
        {
            float currentDistance = scale* rows * columns;
            int indexNumber = 0;
            for(int i = -1; i < list.Count; i++)
            {
                if(Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
                {
                    currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                    indexNumber = i;
                }
            }

            return list[indexNumber];
        }
    }
}

