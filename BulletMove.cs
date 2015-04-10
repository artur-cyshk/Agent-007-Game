using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour {
	public float speed=3f;
	public float orientation=1f;
	public int damage=1;

	void Start () {

	}

	void OnCollisionEnter2D(Collision2D col){
		GameObject.DestroyObject (gameObject);
	}
	

	void Update () {
		speed +=0.1f;
		rigidbody2D.velocity = new Vector2 (speed * orientation,rigidbody2D.velocity.y);
		}

}
	

