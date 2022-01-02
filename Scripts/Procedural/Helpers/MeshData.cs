using System;
using UnityEngine;
namespace Base.Procedural.MeshHelper
{
    public class MeshData
    {
        public string meshName = "NewMesh";
        public Vector3[] positions = null;
        public Vector3[] normals = null;
        public Vector2[] uvs = null;
        public int[] triangles = null;

        public MeshData(int width, int height)
        {
            int vertexCount = width * height;
            int triCount = (width - 1) * (height - 1) * 2;
            positions = new Vector3[vertexCount];
            normals = new Vector3[vertexCount];
            triangles = new int[triCount * 3];
            uvs = new Vector2[vertexCount];
        }

        public void set(Vector3[] positions_, Vector3[] normals_, Vector2[] uvs_, int[] triangles_)
        {
            positions = positions_;
            normals = normals_;
            uvs = uvs_;
            triangles = triangles_;
        }

        public void setPosition(int index, Vector3 pos) => positions[index] = pos;
        public void setNormal(int index, Vector3 normal) => normals[index] = normal;
        public void setUV(int index, Vector2 uv) => uvs[index] = uv;
        public void setTriangleIndex(int at, int triIndex) => triangles[at] = triIndex;

        public Mesh getMesh(bool recalculateNormals = false)
        {
            Mesh m = new Mesh();
            m.vertices = positions;
            m.normals = normals;
            m.triangles = triangles;
            m.uv = uvs;
            if (recalculateNormals) m.RecalculateNormals();
            return m;
        }
    }
}