using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UB
{
    public class Grid
    {
        public const int HEAT_MAP_MAX_VALUE = 100;
        public const int HEAT_MAP_MIN_VALUE = 0;

        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        private int width;
        private int height;
        private float cellSize;
        private int[,] gridArray;
        private Vector3 originPosition;

        public Grid(int _width, int _height, float _cellSize, Vector3 _originPosition)
        {
            width = _width;
            height = _height;
            cellSize = _cellSize;
            originPosition = _originPosition;

            gridArray = new int[width, height];
            
            for(int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    //UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y), 30, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 100);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            SetValue(2, 1, 56);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 1, y) * cellSize + originPosition;
        }

        private void GetXY(Vector3 worldPos, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPos - originPosition).x);
            y = Mathf.FloorToInt((worldPos - originPosition).z);
        }

        public void SetValue(int x, int y, int value)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            }

            if (OnGridValueChanged != null)
            {
                OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            }
        }

        public void SetValue(Vector3 worldPos, int value)
        {
            int x, y;
            GetXY(worldPos, out x, out y);
            SetValue(x, y, value);
        }

        public int GetValue(int x, int y)
        {
            if(x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return 0;
            }
        }

        public int GetValue(Vector3 worldPos)
        {
            int x, y;
            GetXY(worldPos, out x, out y);
            return GetValue(x, y);
        }

    }
}

