using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Base.AI.Behaviours.Editor
{
    [CustomPropertyDrawer(typeof(EditorSharedVariable))]
    public class EditorSharedVariableProperty : PropertyDrawer
    {
        private int selectedIndex = 0;
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            object variable = property.serializedObject.targetObject;

            EditorSharedVariable sharedVariable =  Base.Common.EditorHelpers.GetTargetObjectOfProperty(property) as EditorSharedVariable;
            BehaviourTreeTask objectVariable = variable as BehaviourTreeTask;

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (objectVariable.treeOwner == null) return;

            string[] possibleNames = objectVariable.treeOwner.Blackboard.getNamesOfVariablesByType(sharedVariable.type);
            selectedIndex = 0;
            for(int a=0;a<possibleNames.Length;++a)
            {
                if(possibleNames[a] == sharedVariable.name)
                {
                    selectedIndex = a;
                    break;
                }
            }

            // Calculate rect
            var nameRect = new Rect(position.x, position.y, 150, position.height);
            //choose from possible list
            int newIndex=EditorGUI.Popup(nameRect, selectedIndex, possibleNames);
            if(possibleNames.Length>0)
                {
                sharedVariable.name = possibleNames[newIndex];
                selectedIndex = newIndex;
            }


            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
