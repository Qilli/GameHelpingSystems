using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Procedural.MeshHelper
{
    public class BaseMeshController : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        public MeshFilter getFilter()
        {
            if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();
            return meshFilter;
        }
        public MeshRenderer getRenderer()
        {
            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null) meshRenderer = gameObject.AddComponent<MeshRenderer>();
            return meshRenderer;
        }
    }
}
