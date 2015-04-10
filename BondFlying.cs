using UnityEngine;
using System.Collections;

public class BondFlying : MonoBehaviour {
	private Animator anim;
	public float moveVert;
	public bool isFlying;
	public Vector2 flySpeed=new Vector2(5f,5f);
	public float timeForFly=6f;
	public GUISkin mySkin;
	public int lateFly;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "JetPack") {
			anim.SetBool ("OnJetPack",true);
			GameObject.DestroyObject(col.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		lateFly = (int)timeForFly;
		var currentState = anim.GetCurrentAnimatorStateInfo (0);
		BondMoving bond = GetComponent<BondMoving> ();
		moveVert = Input.GetAxis ("Vertical");
		if (currentState.IsName ("JetPack")) {
			isFlying = true;
			timeForFly -= Time.deltaTime;
			if (moveVert > 0)
				rigidbody2D.velocity = new Vector2 (flySpeed.x * bond.move, flySpeed.y);
			else if (moveVert < 0)
				rigidbody2D.velocity = new Vector2 (flySpeed.x * bond.move, -flySpeed.y);
		}
		if(timeForFly<=0 || bond.bondHp<=0 ){
			isFlying=false;
			anim.SetBool("OnJetPack",false);
		}

	}

	void OnGUI(){
		if(isFlying){
			GUI.skin = mySkin;
			GUI.Box (new Rect (Screen.width/2,Screen.height/2, 200, 200),lateFly.ToString(),GUI.skin.GetStyle ("FlyTime"));
		}
			
	}
}
