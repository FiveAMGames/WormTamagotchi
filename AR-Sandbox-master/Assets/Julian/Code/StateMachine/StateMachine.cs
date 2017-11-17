namespace StateMachine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class StateMachine : MonoBehaviour
    {
        [SerializeField] protected string initialState;
        protected Dictionary<string, IState> allStates;

        /// <summary>
        /// Gets the current state.
        /// </summary>
        public IState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <param name="state">State name of the new state.</param>
        public void ChangeState(string state)
        {
            IState newState;

            if(allStates.TryGetValue(state, out newState))
            {
                if(State != null)
                    State.OnDisable();

                newState.Awake();
                State = newState;
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
            State.Update();
        }

        /// <summary>
        /// Late update.
        /// </summary>
        protected void LateUpdate()
        {
            State.LateUpdate();
        }

        /// <summary>
        /// Fixed update.
        /// </summary>
        protected void FixedUpdate()
        {
            State.FixedUpdate();
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
