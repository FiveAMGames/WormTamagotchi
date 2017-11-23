namespace StateManagement
{
    // This is a demo state
    // Wrap the state inside a 'StateManagement' namespace or include it with 'using StateManagement;'

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // Define the type(s) of the state
    // This state will be available for the following object types: 'Worm' and 'Dodo'
    [StateType(State.Worm | State.Dodo)]
    public class Jump : BaseState   // <- You must inherit from 'BaseState'
    {
        protected float height = 5f;
        protected float speed = 5f;
        protected float time = 0f;

        // Calling base constructor
        public Jump(object caller) : base(caller) { }   // <- Base constructor must be called!
                                                        // Initialization happens before rendering first frame

        // Override update method
        public override void Update()
        {
            // Do some jumping
            baseObject.transform.position = new Vector3(0f, Mathf.Abs(Mathf.Sin(time)), 0f);

            time += Time.deltaTime * speed;

            if(time > 180f * Mathf.Deg2Rad)
                time -= 180f * Mathf.Deg2Rad;
        }

        // You can also override other 'MonoBehaviour' methods:
        /*
        public override void Awake() {}         // Called on loading state
        public override void Start() {}         // Called in the first frame this state is active
        public override void LateUpdate() {}    // Called at the end of every frame
        public override void FixedUpdate() {}   // Called every physics step
        public override void OnDisable() {}     // Called before this state gets unloaded
        */
    }
}
