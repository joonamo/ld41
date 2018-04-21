using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerComp : MonoBehaviour {

	public int score = 0;
	public UnityEngine.UI.Text scoreText;

	// Use this for initialization
	void Start () {
		GameObject scoreGO = GameObject.Find ("ScoreText");
		if (scoreGO) {
			scoreText = scoreGO.GetComponent<UnityEngine.UI.Text> ();
		}
	}

	public void AddScore(int amount = 1)
	{
		score += amount;
		scoreText.text = string.Format ("0 - {0}", score);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
