


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
	public class FollowTarget : BaseState   // <- You must inherit from 'BaseState'  //Aye, Captain!
	{
		
		public GameObject targetToFollow;

		// Calling base constructor
		public FollowTarget(object caller) : base(caller) { }   // <- Base constructor must be called!
		// Initialization happens before rendering first frame

		// Override update method
		public override void Start()
		{
			// Do some dodo following


			//baseObject.GetComponent<Pathfinding> ().speed = 10f;
			baseObject.GetComponent<Pathfinding> ().onWandering = false;
			//baseObject.GetComponentInChildren<Animation> ().Play("run");

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

