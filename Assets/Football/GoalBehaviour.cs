using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour {

	public GameManagerComp GameMan;

	private List<GameObject> EnteredThisFrame = new List<GameObject>();

	// Use this for initialization
	void Start () {
		GameMan = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerComp> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (!EnteredThisFrame.Contains(other.gameObject) && other.gameObject.tag == "Football") {
			Destroy (other.gameObject);
			GameMan.AddScore (1);
			EnteredThisFrame.Add (other.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		EnteredThisFrame.Clear ();
	}
}
