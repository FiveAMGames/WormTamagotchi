using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMODUnity;

public class ScorpionAttack : MonoBehaviour
{private bool onDodo = false;
	private GameObject dodo;
	public GameObject camera;
	public Text score;
    // Animation event - when scorpion has stung the dodo...
    public void Stung()
    {
		if (onDodo) {
			score.text = "Scorpio rulezzz!";

			dodo.GetComponentInChildren<Animator> ().SetBool ("Dead", true);
			dodo.GetComponent<TargetPositionController> ().dodoDead = true;
			dodo.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			GetComponentInParent<Pathfinding> ().DodoIsDead ();
			GetComponent<StudioEventEmitter> ().Play ();
			camera.GetComponent<SoundController> ().dodoIsAlive = false;
		}
    }
	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Herbert")){
			onDodo = true;
			dodo = coll.gameObject;
		}
	}
	void OnTriggerExit(Collider coll){
		if (coll.CompareTag("Herbert")){
			onDodo = false;
		}
	}
}
