using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using StateManagement;

public class SoundController : MonoBehaviour {
	[FMODUnity.EventRef]
	StudioEventEmitter soundScript;
	private float dist = 100f;
	public bool dodoIsAlive = true;

	public StudioEventEmitter sandWaterEmitter;
	public Oasis oasisReference;

	public Transform dodo;
	public Transform skorpion;
	// Use this for initialization
	void Start () {
		soundScript = GetComponent<StudioEventEmitter> ();

		if (sandWaterEmitter == null)
			UnityEngine.Debug.LogError("[SoundController] No sand-water sound emitter assigned!");

		if (oasisReference == null)
			UnityEngine.Debug.LogError("[SoundController] No oasis reference assigned!");

	}
	
	// Update is called once per frame

	void Update () {
		if (dodoIsAlive){
		float d = Vector3.Distance (dodo.position, skorpion.position);

	

		if (skorpion.GetComponent<Pathfinding> ().followDodo) {
			
			dist = d * 1.2f;

		} else {
			dist = 100f;
		}

		soundScript.SetParameter ("Distance", dist);
		soundScript.SetParameter ("DodoLebt", 1f);
		}
		else soundScript.SetParameter ("DodoLebt", 0f);

		sandWaterEmitter.SetParameter ("Sand", 20f - oasisReference.WaterAmount * 20f);
	}
}
