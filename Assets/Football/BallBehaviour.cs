using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour {
	protected GameManagerComp gameMan;

	// Use this for initialization
	void Start () {
		gameMan = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerComp>();
		if (gameMan) {
			gameMan.balls.Add (this.gameObject);
		}
	}

	void OnDestroy() {
		if (gameMan) {
			gameMan.balls.Remove (this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < -10.0f) {
			Destroy (this.gameObject);
		}
	}
}
