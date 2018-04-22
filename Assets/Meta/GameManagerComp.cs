using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerComp : MonoBehaviour {

	public int score = 0;
	public int enemyScore = 0;
	public UnityEngine.UI.Text scoreText;
	public UnityEngine.UI.Text timerText;

	public List<GameObject> enemies = new List<GameObject>();
	public List<GameObject> balls = new List<GameObject>();
	public List<GameObject> spawnPoints = new List<GameObject>();
	public GameObject ballSpawnPoint;

	public GameObject ballClass;
	public float spawnKickForce = 20.0f;

	public GameObject enemyClass;

	protected int ballTarget = 1;
	protected int enemyTarget = 0;
	public float timeLeft = 3.0f * 60.0f;
	protected bool timerRunning = false;
	protected bool gameRunning = true; 
	protected GameObject player;
	public UnityEngine.UI.Text timeUpText;
	public UnityEngine.UI.Text restartText;

	public AudioClip winSound;
	public AudioClip loseSound;
	public AudioClip drawSound;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		GameObject scoreGO = GameObject.Find ("ScoreText");
		if (scoreGO) {
			scoreText = scoreGO.GetComponent<UnityEngine.UI.Text> ();
		}
		GameObject timerGO = GameObject.Find ("TimerText");
		if (timerGO) {
			timerText = timerGO.GetComponent<UnityEngine.UI.Text> ();
			timerText.text = string.Format ("{0:0} : {1:00} : {2:00}", (int)Mathf.Floor (timeLeft / 60.0f), (int)(timeLeft % 60.0f), (int)((timeLeft % 1.0f) * 100.0f));
		}

		GameObject timeGO = GameObject.Find ("TimeText");
		if (timeGO) {
			timeUpText = timeGO.GetComponent<UnityEngine.UI.Text> ();
			timeUpText.CrossFadeAlpha (0.0f, 0.0f, true);
		}
		GameObject restartGO = GameObject.Find ("RestartText");
		if (restartGO) {
			restartText = restartGO.GetComponent<UnityEngine.UI.Text> ();
			restartText.CrossFadeAlpha (0.0f, 0.0f, true);
		}

		foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("EnemySpawnPoint")) {
			spawnPoints.Add (spawnPoint);
		}
		ballSpawnPoint = GameObject.FindGameObjectWithTag ("BallSpawnPoint");
	}

	public void AddScore(int amountPlayer, int amountEnemy)
	{
		score += amountPlayer;
		enemyScore += amountEnemy;
		scoreText.text = string.Format ("{0} - {1}", score, enemyScore);

		int totalScore = score + enemyScore;
		ballTarget = (int)Mathf.Sqrt (totalScore * 2);
		enemyTarget = (int)Mathf.Sqrt (totalScore * 10);

		if (!timerRunning) {
			timerRunning = true;
			Destroy (GameObject.Find ("ControllerImage"));
			Destroy (GameObject.Find ("KBImage"));
		}
	}

	// Update is called once per frame
	void Update () {
		if (gameRunning) {
			if (timerRunning) {
				timeLeft -= Time.deltaTime;
				if (timeLeft < 0.0f) {
					timeLeft = 0.0f;
					gameRunning = false;
					foreach (GameObject GO in balls) {
						Destroy (GO);
					}
					ballTarget = 0;
					enemyTarget = 0;

					AudioSource audio = GetComponent<AudioSource> ();

					timeUpText.CrossFadeAlpha(1.0f, 0.2f, false);
					if (score == enemyScore) {
						timeUpText.text = "Time Up! Draw!";
						audio.clip = drawSound;
					} else if (score > enemyScore) {
						timeUpText.text = "Time Up! You Win!";
						audio.clip = winSound;
					} else {
						timeUpText.text = "Time Up! You Lose!";
						audio.clip = loseSound;
					}
					audio.Play ();

					restartText.CrossFadeAlpha(1.0f, 0.2f, false);
				}
				timerText.text = string.Format ("{0:0} : {1:00} : {2:00}", (int)Mathf.Floor (timeLeft / 60.0f), (int)(timeLeft % 60.0f), (int)((timeLeft % 1.0f) * 100.0f));
			}

			if (balls.Count < ballTarget) {
				Vector3 SpawnPos = ballSpawnPoint.transform.position;
				SpawnPos.y += Random.Range (2.0f, 10.0f);
				Vector3 kickTarget = new Vector3 (Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f));
				GameObject newBall = Instantiate (ballClass, SpawnPos, Quaternion.identity);
				newBall.GetComponent<Rigidbody> ().AddForce (kickTarget * spawnKickForce, ForceMode.Impulse);
			}

			if (enemies.Count < enemyTarget) {
				GameObject spawnPoint = GetSpawnPoint ();
				Vector3 SpawnPos = spawnPoint.transform.position;
				SpawnPos.y += 1.0f;

				Instantiate (enemyClass, SpawnPos, Quaternion.identity);
			}
		} else {
			if (Input.GetButton("Submit")) {
				UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
			}
		}
	}

	GameObject GetSpawnPoint()
	{
		GameObject spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Count - 1)];
		while ((spawnPoint.transform.position - player.transform.position).magnitude < 15.0f) {
			spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Count - 1)];
		}
		return spawnPoint;
	}
}
