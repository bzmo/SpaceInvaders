using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = 5f;
	public float spawnDelay;
	public float timer;
	public bool isSpaceShip = false;

	private float stallTimer = 10f;
	private float tempStallTimer;
	private float spawnTimer;
	private bool movingRight = true;
	private bool stayStill = false;
	private float xmax, xmin;

	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
		xmax = rightBoundary.x;
		xmin = leftBoundary.x;

		spawnTimer = timer;
		tempStallTimer = stallTimer;

		//SpawnEnemies ();
	}

	public void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height));
	}

	void SpawnEnemies(){
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount <= 0) {
				return childPositionGameObject;
			}
		}
		return null;
	}

	void SpawnUntilFull(){


		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}

		if (NextFreePosition ()){
			Invoke ("SpawnUntilFull", spawnDelay); 
		}
	}
	

	bool AllMembersDead(){
		foreach(Transform childPositionGameObject in transform){
			if (childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}

	public float getMin(){
		return xmin;
	}

	public float getMax(){
		return xmax;
	}
	
	// Update is called once per frame
	void Update () {
		if (stayStill) {

		} else if (movingRight) {
			transform.position += Vector3.right * Time.deltaTime * speed;
		} else {
			transform.position += Vector3.left * Time.deltaTime * speed;
		}

		if (AllMembersDead()) {
			timer -= Time.deltaTime;
			if (timer <= 0){
				SpawnUntilFull ();
				timer = spawnTimer;
			}
		}

		float rightEdgeOfFormation = transform.position.x + 0.5f * width;
		float leftEdgeOfFormation = transform.position.x - 0.5f * width;

		if (isSpaceShip == true) {
				rightEdgeOfFormation = transform.position.x - 1;
		}


		if (leftEdgeOfFormation < xmin) {
			movingRight = true;
		} else if (rightEdgeOfFormation > xmax) {

			if (!isSpaceShip) {
				movingRight = false;
			} else {
				stayStill = true;
				tempStallTimer -= Time.deltaTime;
				if (tempStallTimer <= 0) {
					transform.position = new Vector3 (xmin - 1, transform.position.y, 0);
					movingRight = true;
					tempStallTimer = stallTimer;
					stayStill = false;
				}
			}
		}
	}
}
