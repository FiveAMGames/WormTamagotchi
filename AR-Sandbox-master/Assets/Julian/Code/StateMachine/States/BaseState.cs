namespace StateMachine
{
    using UnityEngine;

    public abstract class BaseState : IState
    {
        protected MonoBehaviour baseObject;
        protected bool active = false;

        public BaseState(object caller)
        {
            this.baseObject = caller as MonoBehaviour;
        }

        public bool IsActive
        {
            get { return active; }
        }

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

        public virtual void FixedUpdate()
        {
            // Empty...
        }

        public virtual void OnDisable()
        {
            active = false;
        }
    }
}
