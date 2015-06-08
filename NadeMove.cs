using UnityEngine;
using System.Collections;

public class NadeMove : MonoBehaviour {
	public float orientation=1f;
	public int damage=6;
	private Animator anim;
	public AudioClip myAudioExpl;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigidbody2D.AddForce (new Vector2 (200f*orientation, 300f));
	}

	public void Destroying(){
		GameObject.DestroyObject (gameObject);
	}

	void OnCollisionEnter2D(Collision2D col){
		audio.PlayOneShot (myAudioExpl);
		rigidbody2D.isKinematic = true;
		rigidbody2D.gravityScale = 0f;
		anim.SetTrigger ("IsBoom");
	}



	// Update is called once per frame
	void Update () {

	}
}
