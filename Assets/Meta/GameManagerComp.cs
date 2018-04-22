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
	public float enemyRate = 0.5f;

	public GameObject enemyClass;

	protected int ballTarget = 1;
	protected int enemyTarget = 0;
	public float timeLeft = 3.0f * 60.0f;
	protected bool timerRunning = false;
	protected bool gameRunning = true; 
	protected GameObject player;
	protected GameObject musicPlayer;
	protected float enemySpawnCooldown = 0.0f;
	public UnityEngine.UI.Text timeUpText;
	public UnityEngine.UI.Text restartText;

	public AudioClip winSound;
	public AudioClip loseSound;
	public AudioClip drawSound;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		musicPlayer = GameObject.Find ("MusicPlayer");
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

		ballTarget = (int)Mathf.Sqrt (score * 2);
		enemyTarget = (int)Mathf.Sqrt (score * 10);

		if (!timerRunning) {
			timerRunning = true;
			Destroy (GameObject.Find ("ControllerImage"));
			Destroy (GameObject.Find ("KBImage"));
			Destroy (GameObject.Find ("LogoImage"));

			musicPlayer.GetComponent<AudioSource> ().Play ();
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

					musicPlayer.GetComponent<AudioSource> ().Stop ();

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

			if (enemySpawnCooldown > 0.0f) {
				enemySpawnCooldown -= Time.deltaTime;
			}

			if (enemies.Count < enemyTarget && enemySpawnCooldown <= 0.0f) {
				GameObject spawnPoint = GetSpawnPoint ();
				Vector3 SpawnPos = spawnPoint.transform.position;
				SpawnPos.y += 1.0f;

				Instantiate (enemyClass, SpawnPos, Quaternion.identity);
				enemySpawnCooldown = enemyRate;
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
