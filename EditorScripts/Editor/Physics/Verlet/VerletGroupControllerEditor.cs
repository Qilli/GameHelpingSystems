using UnityEngine;
using UnityEditor;
using Base.Physics.Verlet;
using System;

namespace Base.Editor.Physics.Verlet
{
    [CustomEditor(typeof(VerletGroupController))]
    public class VerletGroupControllerEditor : UnityEditor.Editor
    {
        #region PUBLIC PARAMS
        #endregion
        #region PRIVATE PARAMS
        private const int idOffset = 1000;
        private int selectedPoint = -1;
        private VerletGroupController localCtrl;
        private bool leftMouseButtonDown;
        private bool leftAltPressed;
        private bool rightMouseButtonDown;
        private bool leftControlPressed;
        #endregion

        #region PUBLIC FUNC
        public void OnSceneGUI()
        {
            localCtrl = target as VerletGroupController;
            checkInput();
            drawHandles();
            drawSticks();
            checkHandleSelection();
            EditorUtility.SetDirty(localCtrl);
        }
        public override void OnInspectorGUI()
        {
            if(localCtrl==null)
            {
                localCtrl = target as VerletGroupController;
            }
            DrawDefaultInspector();
            drawActionButtons();
            drawSelectedPoint();
            EditorUtility.SetDirty(localCtrl);
        }

        #endregion
        #region PRIVATE FUNC
        private void drawSelectedPoint()
        {
            GUILayout.BeginVertical();
            if (selectedPoint != -1)
            {
                //draw data of point
                VerletPoint currentPointLocal = localCtrl.getPointAtIndex(selectedPoint);
                currentPointLocal.Position=EditorGUILayout.Vector3Field("Position", currentPointLocal.Position);
                currentPointLocal.PrevPosition= EditorGUILayout.Vector3Field("Prev Position", currentPointLocal.PrevPosition);
                currentPointLocal.IsLocked = EditorGUILayout.Toggle("Is locked", currentPointLocal.IsLocked);
            }
            else GUILayout.Label("No point selected");
            GUILayout.EndVertical();
        }

        private void drawSticks()
        {
            for(int a=0;a<localCtrl.SticksCount;++a)
            {
                if(selectedPoint!=-1 && localCtrl.isStickWithPoint(a,selectedPoint))
                {
                    drawStick(localCtrl.getStickAt(a), a, true);
                }
                else drawStick(localCtrl.getStickAt(a),a);
            }
        }

        private void drawStick(VerletStick vstick, int a,bool isActive=false)
        {
            Handles.color = isActive?localCtrl.editorPrefs.activeStickColor:localCtrl.editorPrefs.stickColor;
            Handles.DrawLine(vstick.point0.Position, vstick.point1.Position,localCtrl.editorPrefs.stickSize);
        }

        private void drawHandles()
        {
            for(int a=0;a<localCtrl.PointsCount;++a)
            {
                drawSelectedHandle(localCtrl.getPointAtIndex(a), a);
            }
        }
        private void checkInput()
        {
            leftMouseButtonDown = false;

            if (Event.current.alt)
            {
                leftAltPressed = true;
            }else leftAltPressed = false;

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
        private void checkHandleSelection()
        {
            if(leftMouseButtonDown)
            {
                if (GUIUtility.hotControl!=0)
                {
                    if (leftAltPressed == false && leftControlPressed == false)
                    {
                        if (leftMouseButtonDown)
                        {
                           selectedPoint= getHotControlIndex();
                        }
                    }
                    else
                    {
                        //for sticks
                        if(leftMouseButtonDown)
                        {
                            if(leftControlPressed)
                            {
                                //add new stick
                                int id = getHotControlIndex();
                                if(id!= -1 && id !=selectedPoint)
                                {
                                    //check if the stick doesnt already exist
                                    if(!localCtrl.stickForPointsExist(selectedPoint,id))
                                    {
                                        //add new stick
                                        localCtrl.addNewStick(selectedPoint, id);
                                        EditorUtility.SetDirty(localCtrl);
                                    }
                                }
                            }
                            else if(leftAltPressed)
                            {
                                //erase stick
                                //add new stick
                                int id = getHotControlIndex();
                                if (id != -1 && id != selectedPoint)
                                {
                                    //check if the stick doesnt already exist
                                    if (localCtrl.stickForPointsExist(selectedPoint, id))
                                    {
                                        //add new stick
                                        localCtrl.removeStick(selectedPoint, id);
                                        EditorUtility.SetDirty(localCtrl);
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        private int getHotControlIndex()
        {
            int id = GUIUtility.hotControl - idOffset;
            if (localCtrl.isPointIndexValid(id))
            {
                return id;
            }return -1;
        }

        private void drawActionButtons()
        {
            EditorGUILayout.BeginVertical();
            if(GUILayout.Button("Add new point"))
            {
                selectedPoint=localCtrl.addNewPoint();
            }
            if (GUILayout.Button("Remove Selected Point"))
            {
                if(selectedPoint!=-1)
                {
                    localCtrl.removePointAt(selectedPoint);
                    selectedPoint = -1;
                }
            }
            EditorGUILayout.EndVertical();
        }
        private void drawSelectedHandle(VerletPoint current, int id)
        {
            Vector3 lastPos = current.Position;
            current.Position = Handles.PositionHandle(current.Position, Quaternion.identity);
            Handles.color = selectedPoint == id ? localCtrl.editorPrefs.selectedSphereColor:localCtrl.editorPrefs.spheresColor;
            current.Position = Handles.FreeMoveHandle(id+idOffset,current.Position, Quaternion.identity, localCtrl.editorPrefs.sphereSize, localCtrl.editorPrefs.snap, Handles.SphereHandleCap);
            if(lastPos!=current.Position)
            {
                //recalculate sticks lengths
                localCtrl.recalculateStickLengths();
            }
        }
        #endregion

    }
}
