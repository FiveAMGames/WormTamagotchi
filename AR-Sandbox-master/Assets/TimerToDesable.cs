using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerToDesable : MonoBehaviour {

	public float timerToDesable;
	private float timer = 0f;
	// Use this for initialization
	void Start () {
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > timerToDesable) {
			timer = 0f;
			gameObject.SetActive (false);
		}
	}
}
