using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorpionAttack : MonoBehaviour
{private bool onDodo = false;
	private GameObject dodo;
	public Text score;
    // Animation event - when scorpion has stung the dodo...
    public void Stung()
    {
		if (onDodo) {
			score.text = "Scorpio rulezzz!";
			print ("scorpio!!");
			dodo.GetComponentInChildren<Animator> ().SetBool ("Dead", true);
			dodo.GetComponent<TargetPositionController> ().dodoDead = true;
			dodo.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			GetComponentInParent<Pathfinding> ().enabled = false;
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
