using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

	public GameObject bulletClass;
	public float moveSpeed = 10.0f;
	public float fireDelay = 0.2f;

	private float fireCooldown = 0.0f;
	private CharacterController charController;


	// Use this for initialization
	void Start () {
		charController = GetComponent<CharacterController>();
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
		if (InputV.magnitude > 0.1f) {
			InputV = InputV * moveSpeed;
			charController.SimpleMove (InputV * Time.deltaTime);
		} else {
			InputV = Vector3.zero;
		}
		//gameObject.transform.Translate (InputV * Time.deltaTime * moveSpeed);

		Vector3 AimV = Input.GetAxis ("AimHorizontal") * right + Input.GetAxis ("AimVertical") * forward;
		if (fireCooldown <= 0.0f && AimV.magnitude > 0.5f) {
			AimV.Normalize ();
			fireCooldown = fireDelay;
			GameObject newBullet = Instantiate (bulletClass);
			newBullet.transform.position = gameObject.transform.position;
			Bullet bulletComp = newBullet.GetComponent<Bullet> ();
			bulletComp.inheritedVelocity = InputV * 0.01f;
			bulletComp.bulletDirection = AimV;
		}
		if (fireCooldown > 0.0f) {
			fireCooldown -= Time.deltaTime;
		}
	}
}
