using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class DodoScript : MonoBehaviour {

	[FMODUnity.EventRef]
	StudioEventEmitter soundScript;
	private float dist = 100f;


	public Transform dodo;
	public Transform skorpion;
	// Use this for initialization
	void Start () {
		soundScript = GetComponent<StudioEventEmitter> ();


	}

	// Update is called once per frame

	void Update () {

		if (!GetComponentInParent<TargetPositionController> ().dodoDead) {

			GetComponent<StudioEventEmitter> ().Play ();

			float d = Vector3.Distance (dodo.position, skorpion.position);



			if (skorpion.GetComponent<Pathfinding> ().followDodo) {

				dist = d * 1.2f;

			} else {
				dist = 100f;
			}

			soundScript.SetParameter ("Distance", dist);
		} else {
			GetComponent<StudioEventEmitter> ().Stop ();
		  
		}


	}
}
