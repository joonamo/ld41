using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	GameObject player;
	public float rotSpeed = 4.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!player) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		if (player) {
			Quaternion targetRot = Quaternion.LookRotation (player.transform.position - transform.position);

			transform.rotation = Quaternion.Slerp (transform.rotation, targetRot, Time.deltaTime * rotSpeed);
		}
	}
}
