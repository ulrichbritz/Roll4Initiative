using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    public class UtilsClass
    {
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
    }

    public static class MeshUtils
    {
        private static readonly Vector3 Vector3zero = Vector3.zero;
        private static readonly Vector3 Vector3one = Vector3.one;
        private static readonly Vector3 Vector3yDown = new Vector3(0, -1);

        public static Quaternion[] cachedQuaternionEulerArr;

        private static void CacheQuaternionEuler()
        {
            if (cachedQuaternionEulerArr != null)
                return;

            for (int i = 0; i < 360; i++)
            {
                cachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
            }
        }

        public static Quaternion GetQuaternionEuler(float rotFloat)
        {
            int rot = Mathf.RoundToInt(rotFloat);
            rot = rot % 360;

            if (rot < 0)
                rot += 360;

            if (cachedQuaternionEulerArr == null)
                CacheQuaternionEuler();

            return cachedQuaternionEulerArr[rot];
        }

        public static Mesh CreateEmptyMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[0];
            mesh.uv = new Vector2[0];
            mesh.triangles = new int[0];
            return mesh;
        }

        public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
        {
            vertices = new Vector3[4 * quadCount];
            uvs = new Vector2[4 * quadCount];
            triangles = new int[6 * quadCount];
        }

        public static Mesh CreateMesh(Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
        {
            return AddToMesh(null, pos, rot, baseSize, uv00, uv11);
        }

        public static Mesh AddToMesh(Mesh mesh, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
        {
            /*
            if(mesh == null)
            {
                mesh = CreateEmptyMesh();
            }

            Vector3[] vertices = new Vector3[4 + mesh.vertices.Length];
            Vector2[] uvs = new Vector2[4 + mesh.uv.Length];
            int[] triangles = new int[6 + mesh.triangles.Length];

            mesh.vertices.CopyTo(vertices, 0);
            mesh.uv.CopyTo(uvs, 0);
            mesh.triangles.CopyTo(triangles, 0);

            int index = vertices.Length / 4 - 1;

            //relocate vertices
            int vIndex = index * 4;
            int vIndex0 = vIndex;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            baseSize *= 0.5f;

            bool skewed = baseSize.x != baseSize.y;

            if (skewed)
            {
                vertices[vIndex0] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, baseSize.y);
                vertices[vIndex1] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y);
                vertices[vIndex2] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y);
                vertices[vIndex3] = pos + GetQuaternionEuler(rot) * baseSize;
            }
            else
            {
                vertices[vIndex0] = pos + GetQuaternionEuler(rot - 270) * baseSize;
                vertices[vIndex0] = pos + GetQuaternionEuler(rot - 180) * baseSize;
                vertices[vIndex0] = pos + GetQuaternionEuler(rot - 90) * baseSize;
                vertices[vIndex0] = pos + GetQuaternionEuler(rot - 0) * baseSize;
            }

            //relocate UVs
            uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
            uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
            uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
            uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
            */
            return null;
        }
        public static void AddQuad(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 GridPos, Vector3 QuadSize, Vector2 Uv)
        {
            vertices[index * 4] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);
            vertices[(index * 4) + 1] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
            vertices[(index * 4) + 2] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
            vertices[(index * 4) + 3] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);

            Debug.Log(vertices[0]);
            Debug.Log(vertices[1]);
            Debug.Log(vertices[2]);
            Debug.Log(vertices[3]);

            uvs[(index * 4)] = Uv;
            uvs[(index * 4) + 1] = Uv;
            uvs[(index * 4) + 2] = Uv;
            uvs[(index * 4) + 3] = Uv;

            triangles[(index * 6) + 0] = (index * 4) + 0;
            triangles[(index * 6) + 1] = (index * 4) + 1;
            triangles[(index * 6) + 2] = (index * 4) + 2;
            triangles[(index * 6) + 3] = (index * 4) + 2;
            triangles[(index * 6) + 4] = (index * 4) + 3;
            triangles[(index * 6) + 5] = (index * 4) + 0;
        }

        public static void CreateEmptyMeshData(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
        {
            vertices = new Vector3[quadCount * 4];
            uvs = new Vector2[quadCount * 4];
            triangles = new int[quadCount * 6];
        }


    }

    }

