namespace StateManagement
{
    public interface IState
    {
        bool IsActive { get; }

        void Awake();
        void Start();
        void Update();
        void LateUpdate();
        void FixedUpdate();
        void OnDisable();
    }
}
