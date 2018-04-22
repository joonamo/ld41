using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour {

	public GameManagerComp GameMan;
	public bool isEnemyGoal = false;
	public ParticleSystem particles;
	public List<AudioClip> sounds;

	private AudioSource myAudio;
	private List<GameObject> EnteredThisFrame = new List<GameObject>();

	// Use this for initialization
	void Start () {
		GameMan = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerComp> ();
		particles = GetComponent<ParticleSystem> ();
		particles.Stop ();
		myAudio = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (!EnteredThisFrame.Contains(other.gameObject) && other.gameObject.tag == "Football") {

			if (isEnemyGoal)
				GameMan.AddScore (0, 1);
			else
				GameMan.AddScore (1, 0);
			EnteredThisFrame.Add (other.gameObject);

			if (sounds.Count > 0) {
				myAudio.clip = sounds [Random.Range (0, sounds.Count - 1)];
			}
			myAudio.Play ();

//			particles.shape.position = other.transform.position -
			particles.Play();

			Destroy (other.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		EnteredThisFrame.Clear ();
	}
}
