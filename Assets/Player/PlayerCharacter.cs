using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

	public GameObject bulletClass;
	public float moveSpeed = 10.0f;
	public float fireDelay = 0.2f;
	public float inheritedSpeedScale = 0.3f;
	public float accel = 100.0f;
	public float decel = 400.0f;
	public float angularAccel = 5.0f;

	public Vector3 lastInput = Vector3.zero;

	private float fireCooldown = 0.0f;
	private CharacterController charController;
	private float currentSpeed = 0.0f;


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
			if (currentSpeed > 0.0f) {
				lastInput = Vector3.Slerp (lastInput, InputV, angularAccel);
			} else {
				lastInput = InputV;
			}
			currentSpeed = Mathf.Min (currentSpeed + accel * Time.deltaTime, moveSpeed);
		} else {
			currentSpeed = Mathf.Max (currentSpeed - decel * Time.deltaTime, 0.0f);
		}

		if (currentSpeed > 0.0f) {
			charController.SimpleMove (lastInput * currentSpeed);
		}

		Vector3 AimV = Input.GetAxis ("AimHorizontal") * right + Input.GetAxis ("AimVertical") * forward;
		if (fireCooldown <= 0.0f && AimV.magnitude > 0.5f) {
			AimV.Normalize ();
			fireCooldown = fireDelay;
			GameObject newBullet = Instantiate (bulletClass);
			newBullet.transform.position = gameObject.transform.position;
			Bullet bulletComp = newBullet.GetComponent<Bullet> ();
			bulletComp.inheritedVelocity = lastInput * currentSpeed * inheritedSpeedScale;
			bulletComp.bulletDirection = AimV;
		}
		if (fireCooldown > 0.0f) {
			fireCooldown -= Time.deltaTime;
		}
	}
}
