using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

	public GameObject bulletClass;
	public float moveSpeed = 10.0f;
	public float fireDelay = 0.2f;

	private float fireCooldown = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Transform CamTran = Camera.main.transform;
		Vector3 forward = CamTran.forward;
		forward.y = 0;
		forward.Normalize ();
		Vector3 right = CamTran.right;
		right.y = 0;
		right.Normalize();
		Vector3 InputV = Input.GetAxis ("Horizontal") * right + Input.GetAxis ("Vertical") * forward;
		gameObject.transform.Translate (InputV * Time.deltaTime * moveSpeed);

		Vector3 AimV = Input.GetAxis ("AimHorizontal") * right + Input.GetAxis ("AimVertical") * forward;
		if (fireCooldown <= 0.0f && AimV.magnitude > 0.0f) {
			fireCooldown = fireDelay;
			GameObject newBullet = Instantiate (bulletClass);
			newBullet.transform.rotation = Quaternion.LookRotation (AimV);
			newBullet.transform.position = gameObject.transform.position;
		}
		if (fireCooldown > 0.0f) {
			fireCooldown -= Time.deltaTime;
		}
	}
}
