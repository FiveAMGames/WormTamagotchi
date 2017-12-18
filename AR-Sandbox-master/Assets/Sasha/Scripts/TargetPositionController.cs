using UnityEngine;
using System.Collections;
using FMOD;
using FMODUnity;
using UnityEngine.UI;
using StateManagement;

public class TargetPositionController : MonoBehaviour
{
	public float speed = 10f;
	public float AppleTimer = 15f;
	private float _appleTimer = 0f;
	public Image[] appleImages;
	public Sprite appleRed;
	public Sprite appleGrey;

	private float currentSpeed;
	Grid grid;

	public GameObject waterParticles;

	public Pathfinding pathfinfingScript;

	public GameObject Sandfootprint;
	public GameObject Waterfootprints;

	public GameObject appleSound;

	public GameObject apple;
	private int appleCount = 0;
	public Text score;
	private GameObject currentApple;
	public bool dodoDead = false;
	// Use this for initialization


	[HideInInspector] public bool onWater = false;

	public float timerForFootsteps = 0.8f;
	private float timer = 0f;



	void Start ()
	{
		
		grid = GameObject.Find ("A*").GetComponent<Grid> ();
		currentApple = Instantiate(apple, new Vector3 (Random.Range(10f, 150f), apple.transform.position.y, Random.Range (10f, 100f)), apple.transform.rotation) as GameObject;

	}
	
	// Update is called once per frame
	void Update ()
	{
		_appleTimer += Time.deltaTime;
		if (_appleTimer > AppleTimer && appleCount>4) {
			SetApple ();
		}

		if (Input.GetKeyDown(KeyCode.I)){
			GameObject.Find ("Worm").GetComponent<Pathfinding> ().deadScorpio = false;
			GameObject.Find ("Worm").GetComponent<StateMachine> ().ChangeState ("WormNotAtSand");
			GameObject.Find ("Worm").GetComponent<Pathfinding> ().deadDodo = false;
			Camera.main.GetComponent<SoundController> ().dodoIsAlive = true;
			dodoDead = false;
			GetComponentInChildren<Animator> ().SetBool ("Dead", false);
			appleCount = 0;
			_appleTimer = 0f;
			for (int i =0; i<5;i++){
				appleImages [i].sprite = appleGrey;
			}
			if (currentApple == null){
				currentApple = Instantiate(apple, new Vector3 (Random.Range(10f, 150f), apple.transform.position.y, Random.Range (10f, 100f)), apple.transform.rotation) as GameObject;
			}
			else	SetApple ();

		}

		if (!dodoDead) {
			float moveHorizontal = Input.GetAxisRaw ("Horizontal");
			float moveVertical = Input.GetAxisRaw ("Vertical");

			Vector3 movement = new Vector3 (-moveHorizontal, 0.0f, -moveVertical);   //flip for the projector
			if (movement != Vector3.zero) {
				GetComponent<Rigidbody> ().rotation = Quaternion.LookRotation (movement);

				GetComponentInChildren<Animator> ().SetBool ("Walk", true);


				timer += Time.deltaTime;
				if (timer > timerForFootsteps) {
					if (!onWater) {
						GameObject foots =	Instantiate (Sandfootprint, gameObject.transform) as GameObject;
						foots.transform.SetParent (null);
						GetComponent<StudioEventEmitter> ().SetParameter ("StepimWasser", 0f);

					} else {
						GameObject foots =	Instantiate (Waterfootprints, gameObject.transform) as GameObject;
						foots.transform.SetParent (null);
						GetComponent<StudioEventEmitter> ().SetParameter ("StepimWasser", 1f);
					}
					if (!GetComponent<StudioEventEmitter> ().IsPlaying()){
						GetComponent<StudioEventEmitter> ().Play ();
					}
					timer = 0f;
				
				}
			} else {

				GetComponentInChildren<Animator> ().SetBool ("Walk", false);
				GetComponent<StudioEventEmitter> ().Stop ();
				waterParticles.SetActive (false);
				timer = 0f;
		
			}





			GetComponent<Rigidbody> ().velocity = movement * currentSpeed;




			SandCheck ();

			if (onWater) {

				GetComponentInChildren<Animator> ().SetBool ("OnWater", true);
				waterParticles.SetActive (true);


			} else {

				GetComponentInChildren<Animator> ().SetBool ("OnWater", false);
				waterParticles.SetActive (false);

			}


		 
		} else {
			if (waterParticles.activeSelf) {
				waterParticles.SetActive (false);
			}

			//dodo is dead
		}

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


	void SetApple(){
		Vector3 newPosition = new Vector3 (Random.Range (10f, 150f), apple.transform.position.y, Random.Range (10f, 100f));
		while (Vector3.Distance (currentApple.transform.position, newPosition) < 60f) {
			newPosition = new Vector3 (Random.Range (10f, 150f), apple.transform.position.y, Random.Range (10f, 100f));

		}
		//print (Vector3.Distance (currentApple.transform.position, newPosition));
		currentApple.transform.position = newPosition;
		_appleTimer = 0f;
	}


	void OnTriggerEnter(Collider coll){
		
		if (coll.CompareTag("Apple")){
			
			appleSound.GetComponent<StudioEventEmitter> ().Play ();

			if (appleCount < 4) {
				appleImages [appleCount].sprite = appleRed;

				appleCount++;
				SetApple ();


				//score.text = "Apples to eat \n \n"  + (4 - appleCount).ToString ();
			} else {
				appleImages [appleCount].sprite = appleRed;
				score.text = "Dodo wins!";
				GameObject.Find ("Worm").GetComponent<StateMachine> ().ChangeState ("ScorpioDead");
				Destroy (currentApple);
			}
	}
			}
			
}
