using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.Procedural.Terrain
{
    public class TerrainDynamicSegments : MonoBehaviour
    {

        [Header("Data")]
        public Dictionary<Vector2, TerrainSegment> currentSegments = new Dictionary<Vector2, TerrainSegment>();
        public int mapSegmentSizeGlobal = 100;
        [Tooltip("Vision range max")]
        public float viewDistance = 100;
        [Tooltip("Position of the observer we base are culling on")]
        public Vector3 observerPosition;

        private List<TerrainSegment> lastSegments = new List<TerrainSegment>();
        private int offsetToSearch = 0;
        private int mapSegmentSize = 100;

        #region PUBLIC FUNCTIONS
        public virtual void initSegmentSystem()
        {
            mapSegmentSize = mapSegmentSizeGlobal - 1;
            offsetToSearch = Mathf.RoundToInt(viewDistance / mapSegmentSize);
        }
        public virtual void updateSegments()
        {
            Vector2 observerInSegmentSpace = worldToSegmentsPosition(observerPosition);
            foreach (TerrainSegment segment in lastSegments) segment.IsVisible = false;
            lastSegments.Clear();
            Vector2 currentPoint = Vector2.zero;
            //add visible segments
            for (int column = -offsetToSearch; column <= offsetToSearch; column++)
            {
                for (int row = -offsetToSearch; row <= offsetToSearch; row++)
                {
                    currentPoint.x = observerInSegmentSpace.x + column;
                    currentPoint.y = observerInSegmentSpace.y + row;
                    //check if already exist if yes just turn on
                    if (currentSegments.TryGetValue(currentPoint, out TerrainSegment s))
                    {
                        s.updateSegment(observerPosition, viewDistance);
                        if (s.IsVisible == true)
                        {
                            lastSegments.Add(s);
                        }
                    }
                    else
                    {
                        //new to create a new segment
                        currentSegments.Add(currentPoint, createNewSegment(currentPoint));
                    }

                }
            }

        }

        public void Update()
        {
            updateSegments();
        }
        public void Start()
        {
            initSegmentSystem();
        }
        #endregion
        #region PRIVATE FUNCTIONS
        private Vector2 worldToSegmentsPosition(Vector3 worldPos)
        {
            int x = Mathf.RoundToInt(worldPos.x / mapSegmentSize);
            int z = Mathf.RoundToInt(worldPos.z / mapSegmentSize);
            return new Vector2(x, z);
        }
        private TerrainSegment createNewSegment(Vector2 currentPoint)
        {
            return new TerrainSegment(currentPoint, mapSegmentSize);
        }
        #endregion
        #region DEBUG
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(observerPosition, Vector3.one);
            Gizmos.DrawWireSphere(observerPosition, viewDistance);
        }
        #endregion

        [System.Serializable]
        public class TerrainSegment
        {
            public Vector2 segmentPosition;
            public GameObject meshObject;
            public Bounds bounds;
            public bool IsVisible
            {
                get => meshObject.activeInHierarchy; set => meshObject.SetActive(value);
            }

            public TerrainSegment(Vector2 pos, int size)
            {
                segmentPosition = pos * size;
                Vector3 vec3Pos= new Vector3(segmentPosition.x, 0, segmentPosition.y);
                meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                bounds = new Bounds(vec3Pos, Vector3.one * size);
                meshObject.transform.position = vec3Pos;
                meshObject.transform.localScale = Vector3.one * size / 10.0f; ;
                IsVisible = false;
            }

            public void updateSegment(Vector3 observerPos, float maxViewDistance)
            {
                float distance = Mathf.Sqrt(bounds.SqrDistance(observerPos));
                //Debug.Log("bound definition: " + bounds.ToString());
               // Debug.Log("Squared distance: " + distance + "for position: " + observerPos + " plane pos: " + meshObject.transform.position);
                if (distance < maxViewDistance)
                {
                    IsVisible = true;
                }
                else IsVisible = false;
            }
        }
    }
}
