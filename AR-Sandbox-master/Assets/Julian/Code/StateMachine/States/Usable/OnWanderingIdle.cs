﻿namespace StateManagement
{
	// This is a demo state
	// Wrap the state inside a 'StateManagement' namespace or include it with 'using StateManagement;'

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	// Define the type(s) of the state
	// This state will be available for the following object types: 'Worm' and 'Dodo'
	[StateType(State.Worm | State.Dodo)]
	public class OnWanderingIdle : BaseState   // <- You must inherit from 'BaseState'  //Aye, Captain!
	{

		float timer = 0f;

		// Calling base constructor
		public OnWanderingIdle(object caller) : base(caller) { }   // <- Base constructor must be called!
		// Initialization happens before rendering first frame

		// Override update method
		public override void Start()
		{
			
			timer = 0f;
			//baseObject.GetComponentInChildren<Animation> ().Play("idle");


		}
		public override void Update(){
			
			timer += Time.deltaTime;
			if (timer > 4f) {
				
				baseObject.GetComponent<StateMachine> ().ChangeState ("Wandering");
				baseObject.GetComponent<Pathfinding> ().stayOnPlace = false;
				baseObject.GetComponent<Pathfinding> ().RandomWanderingTarget ();
			}
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
