using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
	public float speed = 10.0f;
	public float avoidRange = 15.0f;
	public float avoidMult = 1.0f;
	public float ballMult = 1.0f;
	public float ballRange = 50.0f;
	public float ballMin = 0.1f;
	public float playerRange = 15.0f;
	public float playerMult = 2.0f;

	public float ballImpulse = 50.0f;
	public float kickDist = 2.0f;

	//protected GameObject targetBall;
	protected GameObject player;
	protected GameObject targetGoal;
	protected GameManagerComp gameMan;
	protected float currentSpeed = 0.0f;
	protected Vector3 currentDir = Vector3.zero;
	protected CharacterController charController;
	protected float kickCooldown = 0.0f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		gameMan = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerComp>();
		if (gameMan) {
			gameMan.enemies.Add (this.gameObject);
		}
		charController = GetComponent<CharacterController>();

		foreach (GoalBehaviour goal in GameObject.FindObjectsOfType<GoalBehaviour>()) {
			if (goal.isEnemyGoal) {
				targetGoal = goal.gameObject;
			}
		}
	}

	void OnDestroy() {
		if (gameMan) {
			gameMan.enemies.Remove (this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (kickCooldown > 0.0f) {
			kickCooldown -= Time.deltaTime;
		}

//		if (!targetBall || targetBall.transform.position.Equals(Vector3.zero)) {
//			if (gameMan.balls.Count > 0) {
//				targetBall = gameMan.balls [Random.Range (0, gameMan.balls.Count - 1)];
//				while (targetBall.transform.parent) {
//					targetBall = targetBall.transform.parent.gameObject;
//				}
//			}
//		}

		Vector3 toPlayer = (player.transform.position - transform.position);
		Vector3 ballVec = Vector3.zero;
		foreach (GameObject targetBall in gameMan.balls) {
			Vector3 toBall = targetBall.transform.position - transform.position;
			float toBallMag = toBall.magnitude;
			if (toBallMag < ballRange) {
				ballVec += toBall.normalized * ballMult * (1.0f - (toBallMag / ballRange));
			}
			if (toBall.magnitude < kickDist && kickCooldown <= 0.0f) {
				Vector3 kickVec = (targetGoal.transform.position - transform.position).normalized;
				kickVec.y = 0.3f;
				kickVec.Normalize ();
				targetBall.GetComponent<Rigidbody> ().AddForce (kickVec * ballImpulse, ForceMode.Impulse);

				kickCooldown = 1.5f;
			}
		}

		float playerMag = toPlayer.magnitude;
		if (playerMag < playerRange) {
			ballVec += toPlayer.normalized * (1.0f - (playerMag / playerRange)) * playerMult;
		}

		Vector3 avoidVec = Vector3.zero;
		foreach (GameObject Enemy in gameMan.enemies) {
			if (Enemy == gameObject) {
				continue;
			}
			Vector3 toOther = (transform.position - Enemy.gameObject.transform.position);
			float d = toOther.magnitude;
			if (d > avoidRange) {
				continue;
			}
			Vector3 v = toOther.normalized * (1.0f - (d / avoidRange)) * avoidMult;
//			Debug.DrawLine(transform.position, transform.position + v * 10.0f);
			avoidVec += v;
		}
		avoidVec += ballVec;
		avoidVec.Normalize ();
		currentDir = Vector3.Slerp (currentDir, avoidVec, Time.deltaTime * 10.0f);
		charController.SimpleMove (currentDir * speed);
	}
}
