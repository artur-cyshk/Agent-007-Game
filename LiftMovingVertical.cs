using UnityEngine;
using System.Collections;

public class LiftMovingVertical : MonoBehaviour {
	public float speed=2f;
	public float orientation=1f;
	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "PlatformSwap")
			orientation = -orientation;
	}
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x,speed * orientation );
	}
}
