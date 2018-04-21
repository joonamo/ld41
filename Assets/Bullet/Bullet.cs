using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 100.0f;
	private float spawnTime = 0.0f;
	public float lifeTime = 2.0f;
	public Vector3 inheritedVelocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		spawnTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad - spawnTime > lifeTime) {
			Destroy (gameObject);
		}
		gameObject.transform.Translate (new Vector3(0, 0, Time.deltaTime * speed));
	}
}
