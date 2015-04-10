using UnityEngine;
using System.Collections;

public class platformMove : MonoBehaviour {
	public float speed =7f;
	public float direction=1f;
	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "PlatformSwap")
			direction = -direction;
		if (col.gameObject.name == "flore")
			direction = -direction;
	}
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity=new Vector2(rigidbody2D.velocity.x,speed*direction);
	}
}
