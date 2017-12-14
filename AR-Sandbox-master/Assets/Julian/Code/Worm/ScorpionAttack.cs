using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorpionAttack : MonoBehaviour
{
	public Text score;
    // Animation event - when scorpion has stung the dodo...
    public void Stung()
    {
		
		score.text = "Scorpio rulezzz!";
    }
}
