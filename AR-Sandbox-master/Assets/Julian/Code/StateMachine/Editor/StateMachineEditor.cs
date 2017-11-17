#if UNITY_EDITOR
namespace StateMachine
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : Editor
    {
        protected SerializedProperty initialStateProp;

        protected void OnEnable()
        {
            // Get serialized references
            initialStateProp = serializedObject.FindProperty("initialState");
        }

        /// <summary>
        /// Inspector GUI event...
        /// </summary>
        public override void OnInspectorGUI()
        {
            string[] stateOptions = StatesArray();      // List of all states
            int selectedState = 0;                      // current selected state

            // Get index of selected state
            if((initialStateProp.stringValue != null) && (initialStateProp.stringValue.Length > 0))
                selectedState = Array.IndexOf(stateOptions, initialStateProp.stringValue);

            // Update serialized property values
            serializedObject.Update();

            // Draw properties
            selectedState = EditorGUILayout.Popup("Inital Game State", selectedState, stateOptions);
            initialStateProp.stringValue = stateOptions[selectedState] == "None" ? null : stateOptions[selectedState];

            // Apply property changes
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Searches all state names in this project and resturns them.
        /// </summary>
        /// <returns>Array of state names.</returns>
        protected string[] StatesArray()
        {
            string[] stateNames;
            int i = 1;

            // Reflection to gather all game states
            var type = typeof(IState);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(
                    x =>
                    type.IsAssignableFrom(x) &&
                    x.IsDefined(typeof(StateType), false) &&
                    x.IsClass &&
                    !x.IsAbstract
                );

            stateNames = new string[types.Count<Type>() + 1];
            stateNames[0] = "None";

            // List all found states except the base state
            foreach(var t in types)
            {
                stateNames[i] = t.Name;
                i++;
            }

            return stateNames;
        }
    }
}
#endif
