namespace StateMachine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class StateMachine : MonoBehaviour
    {
        [SerializeField] protected State stateType;
        [SerializeField] protected string initialState;
        protected Dictionary<string, IState> allStates;

        /// <summary>
        /// Gets the current state.
        /// </summary>
        public IState CurrentState
        {
            get;
            protected set;
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <param name="state">State name of the new state.</param>
        public void ChangeState(string state)
        {
            IState newState;

            // Clear state
            if(state == null)
            {
                if(CurrentState != null)
                    CurrentState.OnDisable();

                CurrentState = null;
            }
            // Change state
            else if(allStates.TryGetValue(state, out newState))
            {
                if(CurrentState != null)
                    CurrentState.OnDisable();

                newState.Awake();
                CurrentState = newState;
                newState.Start();
            }
            else
            {
                Debug.LogWarningFormat("[GameState] GameState '{0:G}' does not exist. Cannot switch to new state.", state);
            }
        }

        /// <summary>
        /// Awake method.
        /// </summary>
        protected void Awake()
        {
            // Gather all states
            PrefillStateDictionary();
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        protected void Start()
        {
            if((initialState != null) && (initialState.Length > 0))
                ChangeState(initialState);
        }

        /// <summary>
        /// Update.
        /// </summary>
        protected void Update()
        {
            if(CurrentState != null)
                CurrentState.Update();
        }

        /// <summary>
        /// Late update.
        /// </summary>
        protected void LateUpdate()
        {
            if(CurrentState != null)
                CurrentState.LateUpdate();
        }

        /// <summary>
        /// Fixed update.
        /// </summary>
        protected void FixedUpdate()
        {
            if(CurrentState != null)
                CurrentState.FixedUpdate();
        }

        /// <summary>
        /// Prefills the game states dictionary.
        /// </summary>
        private void PrefillStateDictionary()
        {
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

            allStates = new Dictionary<string, IState>(types.Count<Type>() - 1);

            foreach(var t in types)
            {
                // Ignore base state
                if(t.Name != "BaseState")
                    allStates.Add(t.Name, (IState)Activator.CreateInstance(t, new[] { this }));
            }
        }
    }
}
