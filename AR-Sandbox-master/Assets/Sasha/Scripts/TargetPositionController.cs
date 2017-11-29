using UnityEngine;
using System.Collections;


public class TargetPositionController : MonoBehaviour {
	public float speed = 10f;
	Grid grid;
	public Pathfinding pathfinfingScript;
	// Use this for initialization
	void Start () {
		grid = GameObject.Find("A*").GetComponent<Grid> ();

	}
	
	// Update is called once per frame
	void Update () {
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		if (movement != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation (movement);
		}


		transform.Translate (movement * speed * Time.deltaTime, Space.World);

		SandCheck ();
	}

	void SandCheck(){
		
			if (grid.PositionTarget (transform.position).layer == Node.TerrainLayer.Sand) {
				pathfinfingScript.DodoOnSand = true;

			} else {
			
				
			}
		}

}
