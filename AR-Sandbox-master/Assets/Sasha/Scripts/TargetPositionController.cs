using UnityEngine;
using System.Collections;


public class TargetPositionController : MonoBehaviour
{
	public float speed = 10f;
	private float currentSpeed;
	Grid grid;
	public Pathfinding pathfinfingScript;


	// Use this for initialization


	private bool onWater = false;

	float timerForIdle = 3f;
	private float timer = 0f;


	void Start ()
	{
		
		grid = GameObject.Find ("A*").GetComponent<Grid> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		if (movement != Vector3.zero) {
			GetComponent<Rigidbody> ().rotation = Quaternion.LookRotation (movement);

			GetComponentInChildren<Animator> ().SetBool ("Walk", true);


		} else {
			
			GetComponentInChildren<Animator> ().SetBool ("Walk", false);
			
		
		}





		GetComponent<Rigidbody> ().velocity = movement * currentSpeed;




		SandCheck ();


		 


	}








	void SandCheck ()
	{
		Node.TerrainLayer layer = grid.PositionTarget (transform.position).layer;
		if (layer == Node.TerrainLayer.Sand) {
			pathfinfingScript.DodoOnSand = true;
			currentSpeed = speed; 
			onWater = false;

		} else if (layer == Node.TerrainLayer.Water) {
			onWater = true;
			currentSpeed = speed / 2f;

		} else if (layer == Node.TerrainLayer.Grass) {

			currentSpeed = speed;
			onWater = false;

		} else if (layer == Node.TerrainLayer.Mountain) {

			currentSpeed = speed / 2f;
			onWater = false;
		}
	}

}
