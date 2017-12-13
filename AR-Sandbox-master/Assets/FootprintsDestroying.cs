using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintsDestroying : MonoBehaviour {

	public float deadTimer = 2;

	private float timer;
	public GameObject FootprintR;
	GameObject footR;
	// Use this for initialization
	void Start () {
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		Debug.Log (timer);
		if (timer > 2f && footR == null && FootprintR != null) {

			if (!GameObject.Find ("Dodo").GetComponent<TargetPositionController>().onWater){

			footR = Instantiate (FootprintR, GameObject.Find ("Dodo").transform) as GameObject;
			footR.transform.SetParent (null);
		  }
		}


		if (timer > deadTimer) {
			
			GetComponent<SpriteRenderer> ().color = new Color (GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, GetComponent<SpriteRenderer> ().color.a - 0.05f);
			float alpha = GetComponent<SpriteRenderer> ().color.a;
			if (alpha <= 0f) {
				Destroy (gameObject);
			}
		}
	}
	public void Destroy(){
		Destroy (gameObject);
	}
}
