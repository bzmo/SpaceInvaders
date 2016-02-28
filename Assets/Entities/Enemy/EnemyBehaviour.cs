using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject projectile;
	public float projectileSpeed = 10;
	public float health;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 150;
	public AudioClip deathSound;
	public AudioClip fireSound;

	private ScoreKeeper scoreKeeper;

	void Start(){
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();

		//if Projectile not found, missile would be null
		if (missile) {
			health -= missile.GetDamage();
			missile.Hit ();
			if (health <= 0){
				Destroy(gameObject);
				AudioSource.PlayClipAtPoint(deathSound, transform.position);
				scoreKeeper.Score(scoreValue);
			}
		}
	}

	void Fire(){
		Vector3 startPosition = transform.position + new Vector3 (0, -0.8f, 0);
		GameObject missile = Instantiate (projectile, startPosition, Quaternion.identity) as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, -projectileSpeed);

		EnemySpawner enemyObject = GameObject.Find ("EnemyFormation").GetComponent<EnemySpawner> ();
		Vector3 curPosn = transform.position;

		if (enemyObject.getMax () >= curPosn.x) {
			AudioSource.PlayClipAtPoint (fireSound, transform.position);
		}
	}

	void Update(){
		float probability = shotsPerSecond * Time.deltaTime;

		if (Random.value < probability) {
			Fire ();
		}
	}

}
