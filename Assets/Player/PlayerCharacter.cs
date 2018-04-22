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
	private bool swapRightAxis = false;

	private Plane groundPlane;
	private AudioSource[] audioSources;

	// Use this for initialization
	void Start () {
		charController = GetComponent<CharacterController>();
		swapRightAxis = Application.platform == RuntimePlatform.WebGLPlayer;

		groundPlane = new Plane(Vector3.up, Vector3.zero);

		audioSources = GetComponents<AudioSource> ();
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

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float enter = 0.0f;

		Vector3 AimV = Vector3.zero;
		if (swapRightAxis) {
			AimV = Input.GetAxis ("AimHorizontal") * forward + Input.GetAxis ("AimVertical") * -right;
		} else {
			AimV = Input.GetAxis ("AimHorizontal") * right + Input.GetAxis ("AimVertical") * forward;
		}
		AimV += Input.GetAxis ("AimHorizontalKB") * right + Input.GetAxis ("AimVerticalKB") * forward;

		if (Input.GetButton ("Fire1")) {
			if (groundPlane.Raycast (ray, out enter)) {
				Vector3 hitPoint = ray.GetPoint (enter);
				AimV = (hitPoint - transform.position);
				AimV.y = 0.0f;
			}
		}
		
		if (fireCooldown <= 0.0f && AimV.magnitude > 0.5f) {
			AimV.Normalize ();
			fireCooldown = fireDelay;
			GameObject newBullet = Instantiate (bulletClass);
			newBullet.transform.position = gameObject.transform.position;
			Bullet bulletComp = newBullet.GetComponent<Bullet> ();
			bulletComp.inheritedVelocity = lastInput * currentSpeed * inheritedSpeedScale;
			bulletComp.bulletDirection = AimV;

			audioSources [Random.Range (0, audioSources.Length)].Play ();
		}
		if (fireCooldown > 0.0f) {
			fireCooldown -= Time.deltaTime;
		}
	}
}
