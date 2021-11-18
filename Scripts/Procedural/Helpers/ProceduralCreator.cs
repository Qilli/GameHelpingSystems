using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Procedural.Creator
{
    public static class ProceduralCreator
    {
        public enum UVProjectionType
        {
            Angular,
            Top
        }
        public static Mesh CreateQuadMesh(float width, float length,string meshName="QuadMesh")
        {
            Mesh mesh = new Mesh();
            mesh.name = meshName;
            
            Vector3[] positions =
            {
                new Vector3(-width*0.5f,0,-length*0.5f),
                new Vector3(-width * 0.5f, 0, length * 0.5f),
                new Vector3(width * 0.5f, 0, length * 0.5f),
                new Vector3(width * 0.5f, 0, -length * 0.5f)
            };

            Vector3[] normals =
            {
                new Vector3(0,1,0),
                new Vector3(0,1,0),
                new Vector3(0,1,0),
                new Vector3(0,1,0)
            };
            Vector2[] uvs =
            {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,0)
            };

            int[] indices = { 0, 1, 2, 0, 2, 3 };
            mesh.vertices = positions;
            mesh.triangles = indices;
            mesh.normals = normals;
            mesh.uv = uvs;
            return mesh;
        }
        public static Mesh CreateRing(UVProjectionType projType,float innerRadius,float thickness,int angularSegments=3,string meshName="RingMesh")
        {
            angularSegments = Mathf.Clamp(angularSegments, 3, 32);
            Mesh mesh = new Mesh();
            mesh.name = meshName;
            int verticesCount = angularSegments * 2 + 2;
            Vector3[] vertices = new Vector3[verticesCount];
            Vector3[] normals = new Vector3[verticesCount];
            Vector2[] uvs = new Vector2[verticesCount];
            for(int a=0;a< verticesCount; a+=2)
            {
                float perc = (a / ((float)vertices.Length-2));
                float angleRad = perc * Mathf.PI *2;
                Vector3 dir = getUnitVectorByAngle(angleRad);
                vertices[a] = transformVector(dir*innerRadius, Quaternion.identity, Vector3.zero);
                vertices[a+1] = transformVector(dir * (innerRadius+thickness), Quaternion.identity, Vector3.zero);
                normals[a] = Vector3.up;
                normals[a + 1] = Vector3.up;
                switch (projType)
                {
                    case UVProjectionType.Angular:
                        uvs[a].x = perc;
                        uvs[a].y = 0;
                        uvs[a + 1].x = perc;
                        uvs[a + 1].y = 1;
                        break;
                    case UVProjectionType.Top:
                        uvs[a].x = dir.x * (innerRadius / (innerRadius + thickness)) * 0.5f + 0.5f;
                        uvs[a].y = dir.z * (innerRadius / (innerRadius + thickness)) * 0.5f + 0.5f;
                        uvs[a+1].x = dir.x * 0.5f + 0.5f;
                        uvs[a+1].y = dir.z * 0.5f + 0.5f;
                        break;
                }
             
            }
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            int[] triangles = new int[angularSegments * 2 * 3];
            int index = 0;
            int temp = 0;
            for (int a=0;a<angularSegments;a++)
            {
                temp = a *2;
                triangles[index] =  temp;
                triangles[index+1] = (temp+3) ;
                triangles[index+2] = temp+1;
                index += 3;
                triangles[index] = temp;
                triangles[index + 1] = (temp+2);
                triangles[index + 2] = (temp + 3) ;
                index += 3;
            }

            mesh.triangles = triangles;
            return mesh;

        }
        public static Mesh CreateFullScreenQuad(string meshName="ScreenQuadMesh")
        {
            Mesh m = new Mesh();
            Vector3[] positions = new Vector3[]
            {
                new Vector3(-1,-1,1),
                new Vector3(-1,1,1),
                new Vector3(1,1,1),
                new Vector3(1,-1,1)
            };
            int[] indices =
            {
                0,1,2,
                1,2,3
            };
            Vector2[] uvs =
            {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,0)
            };
            m.vertices = positions;
            m.triangles = indices;
            m.uv = uvs;
            return m;
        }
        public static Mesh CreateMeshFromHeightMap(Texture2D heightmap,float segmentWidth=1, float segmentLength=1,float heightMultiplier=1,bool smoothed=true,string meshName="HeightmapMesh")
        {
            Mesh m = new Mesh();
            m.name=meshName;
            Vector3[] vertices = null;
            Vector2[] uvs = null;
            int[] indices=null;
            int vertexCount=0;
            int triCount=0;
            if(smoothed)
            {
                vertexCount=heightmap.width * heightmap.height;
                triCount=(heightmap.width-1)*(heightmap.height-1)*2;
                vertices = new Vector3[vertexCount];
                uvs = new Vector2[vertexCount];
                indices = new int[triCount*3];
                for (var i = 0; i < vertexCount; i++)
                {
                    int column = i % heightmap.width;
                    int row = i/heightmap.width;
                    vertices[i].x=column*segmentWidth;
                    vertices[i].y=heightmap.GetPixel(column,row).r * heightMultiplier;
                    vertices[i].z = row* segmentLength;
                    uvs[i].x = (float)column/(float)(heightmap.width-1);
                    uvs[i].y = (float)row/(float)(heightmap.height-1);
                }
                int index=0;
                int vertexIndex=0;
               
                //triangles
                for (int i = 0; i < heightmap.height-1; i++)
                {
                    for (int j = 0; j < heightmap.width-1; j++)
                    {
                    vertexIndex= i * heightmap.width + j;
                    indices[index++]=vertexIndex;
                    indices[index++]=vertexIndex+ heightmap.width;
                    indices[index++]=vertexIndex + 1;

                    indices[index++]=vertexIndex+1;
                    indices[index++]=vertexIndex+ heightmap.width;
                    indices[index++]=vertexIndex + heightmap.width + 1;

                    }             
                }

                m.vertices=vertices;
                m.uv=uvs;
                m.triangles=indices;
                m.RecalculateNormals();
            }
            return m;
        }
        private static Vector3 getUnitVectorByAngle(float angRad)
        {
            return new Vector3(Mathf.Cos(angRad), 0, Mathf.Sin(angRad));
        }
        private static Vector3 transformVector(in Vector3 inVec,Quaternion rot,Vector3 translation)
        {
            return translation+rot*inVec;
        }
    }
}
