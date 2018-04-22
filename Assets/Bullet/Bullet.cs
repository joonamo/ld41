using System.Collections;
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
			GameObject targetObject = rayHit.collider.gameObject;
			switch (rayHit.collider.gameObject.tag) {
			case "Football":
				{
					while (targetObject.transform.parent && targetObject.transform.parent.gameObject.tag == "Football") {
						targetObject = targetObject.transform.parent.gameObject;
					}
					Vector3 forceDir = -rayHit.normal;

					foreach (GoalBehaviour goal in GameObject.FindObjectsOfType<GoalBehaviour>()) {
						if (goal.isEnemyGoal)
							continue;
						
						Vector3 dir = (goal.transform.position - targetObject.gameObject.transform.position).normalized;
						forceDir = Vector3.Slerp (forceDir, dir, 0.7f);

						break;
					}

					AudioSource audio = targetObject.GetComponent<AudioSource> ();
					if (audio) {
						audio.pitch = Random.Range (0.9f, 1.1f);
						audio.Play ();
					}

					forceDir.y = 0.2f;
					forceDir.Normalize ();
					targetObject.GetComponent<Rigidbody> ().AddForce (
						forceDir * ballImpulse, ForceMode.Impulse);
				
					break;
				}
			case "Enemy":
				{
					GameObject ower = GameObject.Find ("Ower");
					ower.transform.position = targetObject.transform.position;
					AudioSource audio = ower.GetComponent<AudioSource> ();
					if (audio) {
						audio.pitch = Random.Range (0.9f, 1.1f);
						audio.Play ();
					}
					Destroy (targetObject);
					break;
				}
			default:
				break;
			}
			Destroy (gameObject);
		}

		gameObject.transform.position += translateAmount;
		gameObject.transform.rotation = Quaternion.LookRotation (translateAmount);
	}
}
