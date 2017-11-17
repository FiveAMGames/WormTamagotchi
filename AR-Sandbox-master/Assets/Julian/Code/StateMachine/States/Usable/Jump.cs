namespace StateMachine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [StateType(State.Object | State.Player)]
    public class Jump : BaseState
    {
        protected float height = 5f;
        protected float speed = 5f;
        protected float time = 0f;

        // Calling base constructor
        public Jump(object caller) : base(caller) {}

        // Override update
        public override void Update()
        {
            baseObject.transform.position = new Vector3(0f, Mathf.Abs(Mathf.Sin(time)), 0f);

            time += Time.deltaTime * speed;

            if(time > 180f * Mathf.Deg2Rad)
                time -= 180f * Mathf.Deg2Rad;
        }
    }
}
