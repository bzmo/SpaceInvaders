using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject projectile;
	public float speed = 15.0f;
	public float padding = 1f;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 5f;

	public Text livesText;

	float xmin, xmax, ymin, ymax;
	GameOver gameOver;

	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;

		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;

		Vector3 downmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 upmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, distance));
		ymin = downmost.y + padding;
		ymax = upmost.y - padding;

		gameOver = GameObject.Find ("Game Over").GetComponent<GameOver> ();
		livesText.text = "Lives " + health;
	}

	public AudioClip fireSound;
	public AudioClip deathSound;

	void Fire(){
		Vector3 newPosn = new Vector3 (transform.position.x, transform.position.y + 0.5f, 0);
		GameObject beam = Instantiate (projectile, newPosn, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed,0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}
	
	// Update is called once per frame
	void Update () {

	if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, firingRate);
	}

	if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("Fire");
		}
	
	if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
			//transform.position += new Vector3(0, +speed * Time.deltaTime, 0);
			transform.position += Vector3.up * speed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
			//transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
			transform.position += Vector3.down * speed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			//transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.left * speed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			//transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.right * speed * Time.deltaTime;
		}


		// restrict the player to the gamespace
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);

		float newY = Mathf.Clamp (transform.position.y, ymin, ymax / 10);
		transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		
		//if Projectile not found, missile would be null
		if (missile) {
			health -= missile.GetDamage();
			livesText.text = "Lives " + health;
			missile.Hit ();
			if (health <= 0){
				Destroy(gameObject);
				AudioSource.PlayClipAtPoint(deathSound, transform.position);
				gameOver.ShowGameOver();
			}
		}
	}
}
