namespace StateMachine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BaseState : IState
    {
        protected bool active = false;

        public virtual void Awake()
        {
            active = true;
        }

        public virtual void Start()
        {
            // Empty...
        }

        public virtual void Update()
        {
            // Empty...
        }

        public virtual void LateUpdate()
        {
            // Empty...
        }

        public virtual void OnDisable()
        {
            active = false;
        }

        public bool IsActive
        {
            get { return active; }
        }
    }
}
