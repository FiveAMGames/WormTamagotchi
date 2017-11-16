namespace StateMachine
{
    public interface IState
    {
        void Awake();
        void Start();
        void Update();
        void LateUpdate();
        void OnDisable();

        bool IsActive { get; }
    }
}
