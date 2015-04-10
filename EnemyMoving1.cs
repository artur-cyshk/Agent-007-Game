using UnityEngine;
using System.Collections;

public class EnemyMoving1 : MonoBehaviour {
	public float speed=0.5f;
	public float orientation=-1f;
	public bool enemyFacingRight=false;
	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "EnemySwap") {
			orientation = -orientation;
			Flip ();
		}
	}
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = new Vector2 (speed * orientation,rigidbody2D.velocity.y);
	}

	public void Flip(){
		enemyFacingRight = !enemyFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}		
}
