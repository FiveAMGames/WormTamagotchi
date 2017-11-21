using UnityEngine;
using System.Collections;

public class TargetPositionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate (Vector3.back * 10* Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate (Vector3.forward *10* Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate (Vector3.right *10* Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate (Vector3.left *10* Time.deltaTime);
		}


	}

}
