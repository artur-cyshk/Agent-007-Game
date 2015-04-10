using UnityEngine;
using System.Collections;

public class SwapMovingSaw : MonoBehaviour {
	public float speed=10f;
	public float orientation=1f;
	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "PlatformSwap")
			orientation = -orientation;
	}
	// Update is called once per frame
	void FixedUpdate(){
		rigidbody2D.MoveRotation (rigidbody2D.rotation+100f*Time.deltaTime);
	}
	void Update () {

		rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x,speed*orientation);
	}
}
