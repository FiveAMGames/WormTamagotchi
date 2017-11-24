#if UNITY_EDITOR
namespace StateManagement
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
        protected SerializedProperty stateTypeProp;
        protected SerializedProperty initialStateProp;
        protected State selectedType;
        protected State previousType;

        private string[] stateOptions;              // All states for a specific type (names for dropdown)
        private int selectedState = 0;              // Initial selected state (index for dropdown)
        private int currentState = 0;               // Current selected state (index for dropdown)
        private int previousState = 0;              // Previous selected state (for detecting changes)

        /// <summary>
        /// Called on enable.
        /// </summary>
        protected void OnEnable()
        {
            // Get serialized references
            stateTypeProp = serializedObject.FindProperty("stateType");
            initialStateProp = serializedObject.FindProperty("initialState");

            selectedType = (State)(1 << stateTypeProp.enumValueIndex);
            previousType = selectedType;

            // List of all states
            stateOptions = StatesArray(selectedType);
        }

        /// <summary>
        /// Inspector GUI event...
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Reload states on type change
            if(selectedType != previousType)
            {
                stateOptions = StatesArray(selectedType);
                previousType = selectedType;
            }

            // Get index of selected state
            if((initialStateProp.stringValue != null) && (initialStateProp.stringValue.Length > 0))
                selectedState = Array.IndexOf(stateOptions, initialStateProp.stringValue);
            else
                selectedState = 0;

            // Update serialized property values
            serializedObject.Update();

            // Draw properties

            // Type selection
            if(!EditorApplication.isPlaying)
            {
                // Can only change state type while NOT in playmode
                selectedType = (State)EditorGUILayout.EnumPopup("Type", (Enum)selectedType);
                stateTypeProp.enumValueIndex = BitPosition((uint)selectedType);

                // Can only change initial state while NOT in playmode
                // Available states
                selectedState = EditorGUILayout.Popup("Inital Game State", selectedState, stateOptions);

                // Changed type: new type does not have this state -> reset
                if(selectedState < 0)
                    selectedState = 0;

                // Serialize initial state
                initialStateProp.stringValue = stateOptions[selectedState] == "None" ? null : stateOptions[selectedState];
            }
            else
            {
                EditorGUILayout.LabelField("Type", selectedType.ToString());

                // Get index of current running state
                if(((StateMachine)target).CurrentState != null)
                    currentState = Array.IndexOf(stateOptions, ((StateMachine)target).CurrentState.GetType().Name);

                // Able to change current state while in playmode
                currentState = EditorGUILayout.Popup("Current Game State", currentState, stateOptions);

                if(currentState != previousState)
                {
                    // Change state on runtime
                    ((StateMachine)target).ChangeState(stateOptions[currentState] == "None" ? null : stateOptions[currentState]);
                    previousState = currentState;
                }
            }

            // Apply property changes
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Searches all state names in this project and resturns them.
        /// </summary>
        /// <returns>Array of state names.</returns>
        /// <param name="state">Type for filtering states.</param>
        protected string[] StatesArray(State state)
        {
            List<string> stateNames;
            StateType stateType;

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

            stateNames = new List<string>(1);
            stateNames.Add("None");

            // List all found states except the base state
            foreach(var t in types)
            {
                stateType = (StateType)Attribute.GetCustomAttribute(t, typeof(StateType));

                if((stateType.Type & state) == state)
                {
                    stateNames.Add(t.Name);
                }
            }

            return stateNames.ToArray();
        }

        /// <summary>
        /// Gets the position of the first set bit in a 32 bit integer register.
        /// </summary>
        /// <returns>The position.</returns>
        /// <param name="input">Integer input.</param>
        private int BitPosition(uint input)
        {
            int i = 0;

            while((input & 1) == 0)
            {
                i++;
                input = input >> 1;

                if(i >= 32)
                    return 0;
            }
			Debug.Log (i);
            return i;
        }
    }
}
#endif
