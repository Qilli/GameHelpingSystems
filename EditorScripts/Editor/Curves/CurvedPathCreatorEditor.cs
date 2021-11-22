using System;
using UnityEditor;
using UnityEngine;
namespace Base.Editor.Curves
{
    [CustomEditor(typeof(Base.Curves.CurvedPathCreator))]
    public class CurvedPathCreatorEditor : UnityEditor.Editor
    {
        #region PUBLIC PARAMS

        #endregion
        #region PRIVATE PARAMS
        private Base.Curves.CurvedPathCreator creatorObj;
        private const int idOffset = 1000;
        private int selectedPoint = -1;
        private bool leftMouseButtonDown;
        private bool leftAltPressed;
        private bool rightMouseButtonDown;
        private bool leftControlPressed;
        private bool matchTangentsOnEdit = true;
        #endregion

        #region PUBLIC FUNC
        public void OnSceneGUI()
        {
            if (creatorObj == null) creatorObj = (Base.Curves.CurvedPathCreator)target;
            checkInput();
            drawCurvePoints();
            drawCurveShape();
            drawTangents();
            checkHandleSelection();
            EditorUtility.SetDirty(creatorObj);
        }

        public override void OnInspectorGUI()
        {
            if (creatorObj == null) creatorObj = (Base.Curves.CurvedPathCreator)target;
            GUILayout.BeginVertical();
            drawInspectorUI();
            GUILayout.EndVertical();
            DrawDefaultInspector();
            EditorUtility.SetDirty(creatorObj);
        }

        #endregion
        #region PRIVATE FUNC
        private void checkInput()
        {
            leftMouseButtonDown = false;

            if (Event.current.alt)
            {
                leftAltPressed = true;
            }
            else leftAltPressed = false;

            if (Event.current.control)
            {
                leftControlPressed = true;
            }
            else leftControlPressed = false;

            if (Event.current.rawType == EventType.MouseDown)
            {
                if (Event.current.button == 0) leftMouseButtonDown = true;
            }

        }
        private int getHotControlIndex()
        {
            int id = GUIUtility.hotControl - idOffset;
            if (creatorObj.getPath().isIndexValid(id))
            {
                return id;
            }
            return -1;
        }

        private void checkHandleSelection()
        {
            if (leftMouseButtonDown)
            {
                if (GUIUtility.hotControl != -1 && GUIUtility.hotControl!=selectedPoint)
                {
                    if (leftAltPressed == false && leftControlPressed == false)
                    {
                        if (leftMouseButtonDown)
                        {
                            int newIndex = getHotControlIndex();
                            if(newIndex!=-1) selectedPoint = getHotControlIndex();
                        }
                    }
                    else
                    {
                        //for sticks
                        if (leftMouseButtonDown)
                        {
                            if (leftControlPressed)
                            {
                                //add new stick
                                int id = getHotControlIndex();
                                if (id != -1 && id != selectedPoint)
                                {
                            
                                }
                            }
                            else if (leftAltPressed)
                            {
                                //erase stick
                                //add new stick
                                int id = getHotControlIndex();
                                if (id != -1 && id != selectedPoint)
                                {
                                   
                                }
                            }
                        }
                    }
                }
            }
        }
        private void drawCurveShape()
        {
            Base.Curves.CurvedPath path = creatorObj.getPath();
            for (int i = 0; i < path.getPointsCount()-1; i+=3)
            {
                Handles.DrawBezier(path.getPointAt(i), path.getPointAt(i+3), path.getPointAt(i+1), path.getPointAt(i+2), Color.white, EditorGUIUtility.whiteTexture, 1);
            }
        }
        private void drawTangents()
        {
            Base.Curves.CurvedPath path = creatorObj.getPath();
            for (int i = 0; i < path.getPointsCount() - 1; i += 3)
            {
                Handles.DrawLine(path.getPointAt(i), path.getPointAt(i + 1));
                Handles.DrawLine(path.getPointAt(i+2), path.getPointAt(i + 3));
            }
        }
        private Vector3 drawPoint(Vector3 point, int i,Color c)
        {
            if(selectedPoint==i)
            {
                Handles.color = c;
                Handles.FreeMoveHandle(0, point, Quaternion.identity, HandleUtility.GetHandleSize(point) * 0.3f, Vector3.zero, Handles.SphereHandleCap);
                return Handles.PositionHandle(point, Quaternion.identity);
            }
            else
            {
                Handles.color = c;
                Handles.FreeMoveHandle(i+idOffset, point, Quaternion.identity, HandleUtility.GetHandleSize(point) * 0.3f, Vector3.zero, Handles.SphereHandleCap);
                return point;
            }
              
        }

        private void drawCurvePoints()
        {
            Base.Curves.CurvedPath path = creatorObj.getPath();
            Vector3 point;
            for (int i = 0; i < path.getPointsCount(); i++)
            {
                point = path.getPointAt(i);
                if (i % 3 == 0)
                {
                    //anchor point
                    Vector3 newPos = drawPoint(point, i, Color.red);
                    path.setPointAt(i, newPos);
                    if (newPos != point && matchTangentsOnEdit)
                    {
                        Vector3 offset = newPos - point;
                        path.moveTangentFor(i, offset);
                    }
                }
                else
                {
                    //draw tangent
                    Vector3 newPos = drawPoint(point, i, Color.blue);
                    if(newPos!=point && matchTangentsOnEdit)
                    {
                        path.setTangetAndMatch(i,newPos);
                    }
                    else path.setPointAt(i, newPos);
                }
            }
        }
        private void drawInspectorUI()
        {
            if (GUILayout.Button("Add New Point"))
            {
                creatorObj.addNewPoint();
            }
            if (GUILayout.Button("Match Tangents"))
            {
                creatorObj.matchTangents();
            }
            if (GUILayout.Button("Remove Selected"))
            {

            }
            if (GUILayout.Button("Close Path"))
            {
                creatorObj.closePath();
            }
            if (GUILayout.Button("Reset Path"))
            {

            }
           matchTangentsOnEdit= GUILayout.Toggle(matchTangentsOnEdit, "Match tangents on edit");

        }
        #endregion
    }
}
