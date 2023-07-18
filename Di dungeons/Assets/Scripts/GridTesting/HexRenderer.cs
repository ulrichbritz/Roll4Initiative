using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UB
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HexRenderer : MonoBehaviour
    {
        private Mesh m_Mesh;
        private MeshFilter m_MeshFilter;
        private MeshRenderer m_MeshRenderer;

        public Material material;

        private void Awake()
        {
            m_MeshFilter = GetComponent<MeshFilter>();
            m_MeshRenderer = GetComponent<MeshRenderer>();

            m_Mesh = new Mesh();
            m_Mesh.name = "Hex";

            m_MeshFilter.mesh = m_Mesh;
            m_MeshRenderer.material = material;
        }
    }
}

