namespace StateManagement
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class StateType : Attribute
    {
        protected State type;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine.StateType"/> attribute.
        /// </summary>
        /// <param name="type">Type.</param>
        public StateType(State type)
        {
            this.type = type;
        }

        public State Type
        {
            get { return type; }
        }

        /// <summary>
        /// Compares a state type to the underlying class type.
        /// </summary>
        /// <returns><c>true</c>, if the underlying class type matches, <c>false</c> otherwise.</returns>
        /// <param name="cType">The state type comparing to.</param>
        public bool CompareType(State cType)
        {
            if((type & cType) > 0)
                return true;

            return false;
        }
    }

    /// <summary>
    /// State types.
    /// </summary>
    [Flags]
    [Serializable]
    public enum State : uint
    {
        GameMode    = 0x01,
        Worm        = 0x02,
        Dodo        = 0x04,
        Phenomenon  = 0x08
    }
}
