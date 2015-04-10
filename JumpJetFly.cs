using UnityEngine;
using System.Collections;

public class JumpJetFly : MonoBehaviour {
	public float speed=3f;
	public float orientation=1f;
	public bool flyVert=false;
	public bool flyRight=false;
	bool final=false;
	private Animator anim;
	public newcam mycam;
	public GUISkin myskin;
	public AudioClip myAudioFly;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player") {
			flyVert=true;
			audio.PlayOneShot(myAudioFly);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "jumpjetRight") {
			flyRight=true;
		}
		if (col.gameObject.tag == "nextLev") {
			Application.LoadLevel(2);
		}
		if (col.gameObject.tag == "final") {
			final=true;
		}
	}
	// Update is called once per frame
	void Update () {
		if (flyVert) {
			rigidbody2D.velocity=new Vector2(0,speed);
			mycam.target=gameObject.transform;
		}
		if (flyRight) {
			anim.SetTrigger("Right");
			flyVert=false;
			speed=8f;
			rigidbody2D.velocity=new Vector2(speed,0);
		}
		if (Input.GetKey (KeyCode.Escape)) {
			Application.LoadLevel(0);
		}
		
		if (Input.GetKey (KeyCode.F1)) {
			Application.LoadLevel (1);
		}
	}
	void OnGUI(){
		GUI.skin = myskin;
		if (final == true) {
			GUI.Label (new Rect (Screen.width / 2-Screen.width*0.1f, Screen.height / 2-Screen.width*0.1f, Screen.width/2, Screen.height*0.1f), "YOU WIN", GUI.skin.GetStyle ("FlyTime"));
			GUI.Label (new Rect (Screen.width / 2-Screen.width*0.14f, Screen.height / 2, Screen.width/2, Screen.height*0.1f), "press F1 to restart", GUI.skin.GetStyle ("FlyTime"));
			GUI.Label (new Rect (Screen.width / 2-Screen.width*0.18f, Screen.height / 2+Screen.width*0.1f, Screen.width/2, Screen.height*0.1f), "press ECS to main menu", GUI.skin.GetStyle ("FlyTime"));
		}
	}
}
