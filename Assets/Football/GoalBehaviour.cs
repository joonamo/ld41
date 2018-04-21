using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Football") {
			Destroy (other.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
