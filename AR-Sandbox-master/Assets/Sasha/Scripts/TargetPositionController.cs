using UnityEngine;
using System.Collections;

public class TargetPositionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		transform.rotation = Quaternion.LookRotation (movement);


		transform.Translate (movement * 10f * Time.deltaTime, Space.World);
	}
}
