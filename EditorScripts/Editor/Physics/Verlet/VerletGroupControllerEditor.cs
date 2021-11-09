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
        #endregion

        #region PUBLIC FUNC
        public void OnSceneGUI()
        {
            localCtrl = target as VerletGroupController;
            checkInput();
            drawHandles();
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
                VerletPoint currentPointLocal = localCtrl.getAtIndex(selectedPoint);
                currentPointLocal.Position=EditorGUILayout.Vector3Field("Position", currentPointLocal.Position);
                currentPointLocal.PrevPosition= EditorGUILayout.Vector3Field("Prev Position", currentPointLocal.PrevPosition);
                currentPointLocal.IsLocked = EditorGUILayout.Toggle("Is locked", currentPointLocal.IsLocked);
            }
            else GUILayout.Label("No point selected");
            GUILayout.EndVertical();
        }
        private void drawHandles()
        {
            for(int a=0;a<localCtrl.PointsCount;++a)
            {
                drawSelectedHandle(localCtrl.getAtIndex(a), a);
            }
        }
        private void checkInput()
        {
            leftMouseButtonDown = false;
            if(Event.current.rawType == EventType.MouseDown)
            {
                leftMouseButtonDown = true;
            }
        }
        private void checkHandleSelection()
        {
            if(leftMouseButtonDown)
            {
                if (GUIUtility.hotControl!=0)
                {
                    int id = GUIUtility.hotControl - idOffset;
                    if (localCtrl.isIndexValid(id))
                    {
                        selectedPoint = id;
                    }
                }
            }
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
            current.Position = Handles.PositionHandle(current.Position, Quaternion.identity);
            Handles.color = localCtrl.editorPrefs.spheresColor;
            current.Position = Handles.FreeMoveHandle(id+idOffset,current.Position, Quaternion.identity, localCtrl.editorPrefs.sphereSize, localCtrl.editorPrefs.snap, Handles.SphereHandleCap);
        }
        #endregion

    }
}
