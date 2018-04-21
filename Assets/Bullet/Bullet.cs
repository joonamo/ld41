﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public LayerMask rayMask = -1;
	public float speed = 100.0f;
	private float spawnTime = 0.0f;
	public float lifeTime = 2.0f;
	public float ballImpulse = 100.0f;
	public Vector3 inheritedVelocity = Vector3.zero;
	public Vector3 bulletDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
		spawnTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		float deltaT = Time.deltaTime;
		if (Time.timeSinceLevelLoad - spawnTime > lifeTime) {
			Destroy (gameObject);
		}

		Vector3 translateAmount = speed * bulletDirection * deltaT + inheritedVelocity * deltaT;

		RaycastHit rayHit;
		if (Physics.Raycast (gameObject.transform.position, translateAmount.normalized, out rayHit, translateAmount.magnitude, rayMask)) {
			if (rayHit.collider.gameObject.tag == "Football") {
				GameObject targetObject = rayHit.collider.gameObject;
				while (targetObject.transform.parent && targetObject.transform.parent.gameObject.tag == "Football") {
					targetObject = targetObject.transform.parent.gameObject;
				}
				Vector3 forceDir = -rayHit.normal;

				float bestDot = 0.0f;
				Vector3 bestDir = forceDir;
				foreach (GameObject goal in GameObject.FindGameObjectsWithTag("Goal")) {
					Vector3 dir = (goal.transform.position - gameObject.transform.position).normalized;
					float dot = Vector3.Dot (forceDir, dir);
					if (dot > bestDot) {
						bestDot = dot;
						bestDir = dir;
					}
				}

				forceDir = Vector3.Slerp (forceDir, bestDir, (1.0f - bestDot * 0.7f) * 0.7f);
				forceDir.y = 0.2f;
				forceDir.Normalize ();
				targetObject.GetComponent<Rigidbody> ().AddForce (
					forceDir * ballImpulse, ForceMode.Impulse);
			}
			Destroy (gameObject);
		}

		gameObject.transform.position += translateAmount;
		gameObject.transform.rotation = Quaternion.LookRotation (translateAmount);
	}
}
