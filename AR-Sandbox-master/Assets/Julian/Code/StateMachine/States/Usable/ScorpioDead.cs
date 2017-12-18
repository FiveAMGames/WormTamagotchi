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
	public class ScorpioDead : BaseState   // <- You must inherit from 'BaseState'  //Aye, Captain!
	{

		public bool stillDead = true;

		// Calling base constructor
		public ScorpioDead(object caller) : base(caller) { }   // <- Base constructor must be called!
		// Initialization happens before rendering first frame

		// Override update method
		public override void Start()
		{
			baseObject.GetComponent<Pathfinding> ().boom.gameObject.SetActive (true);
			baseObject.GetComponent<Pathfinding> ().deadScorpio = true;
			baseObject.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = false;
			if (baseObject.GetComponent<Pathfinding> ().sandTrail.activeSelf) {
				baseObject.GetComponent<Pathfinding> ().sandTrail.SetActive (false);
			}



		}
		public override void OnDisable(){
			
			baseObject.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = true;
			baseObject.GetComponent<Pathfinding> ().deadScorpio = false;
			baseObject.GetComponent<Pathfinding> ().boom.gameObject.SetActive (false);

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
