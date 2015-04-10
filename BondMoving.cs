using UnityEngine;
using System.Collections;

public class BondMoving : MonoBehaviour {

	public bool facingRight = true;
	public bool grounded = false;
	public bool onEnemyHead=false;
	public bool onLift = false;
	public bool loadingLevel=false;
	public bool deadwindow=false;
	public bool isShooting;
	public int bondHp;
	public int bondHpMax;
	public float loadingTime = 0f;
	public float maxSpeed = 2.5f;
	public float jumpForce = 350f;
	public float groundRadius;
	public float move;
	public float bondHealthBar;
	public Transform groundCheck;
	public BulletMove bullet;
	public NadeMove nade;
	public LayerMask whatIsGround;
	public LayerMask whatIsEnemyHead;
	private Animator anim;
	public GUISkin mySkin;
	public Texture2D[] toolBar;



	// Use this for initialization
	void Start () {
		anim=GetComponent<Animator>();
		bondHpMax = bondHp;
	}

	public void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}	

	public void Damage(int bulletCount){
		bondHp -= bulletCount;
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "platformVert") {
			rigidbody2D.gravityScale=3f;
			jumpForce=420f;
			onLift=true;
		}
		if (col.gameObject.tag == "enemyBullet") {
			BulletMove bullet=col.gameObject.GetComponent<BulletMove>();	
			if(bullet!=null){
				Damage (bullet.damage);
			}
		}
		if (col.gameObject.tag == "jumpjet") {
			GameObject.DestroyObject(gameObject);
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "platformVert") {
			rigidbody2D.gravityScale=0.7f;
			jumpForce=350f;
			onLift=false;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "enemyfull") {
			Damage(bondHpMax);
			deadwindow=true;
		}
		if (col.gameObject.tag == "deadzone") {
			deadwindow = true;
			GameObject.DestroyObject(gameObject);
		}

		if (col.gameObject.tag == "medicine") {
			if(bondHp<bondHpMax){
				bondHp++;
				GameObject.DestroyObject(col.gameObject);
			}
		}
		if (col.gameObject.tag == "case") {
			BulletShotFromBond shoot=GetComponent<BulletShotFromBond>();
			NadeThrowing nadeThrow = GetComponent<NadeThrowing> ();
			int bonusNum=Random.Range(0,2);
			switch(bonusNum){
			case 0:
				shoot.AllBulletsCount+=10;
				break;
			case 1:{
				if(nadeThrow.nadeCount<nadeThrow.maxNadeCount)
					nadeThrow.nadeCount++;
				else{
					nadeThrow.maxNadeCount++;
					nadeThrow.nadeCount++;
				}
				break;
			}
			default: break;
			}
			GameObject.DestroyObject(col.gameObject);
		}
	}
	
	void Update(){
		if(!loadingLevel){
			BondFlying bondFly = GetComponent<BondFlying> ();
			grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
			onEnemyHead = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsEnemyHead);
			move = Input.GetAxis ("Horizontal");
			anim.SetFloat ("Speed", Mathf.Abs (move));
			anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
			BulletShotFromBond shoot = GetComponent<BulletShotFromBond> ();
			NadeThrowing nadeThrow = GetComponent<NadeThrowing> ();
			bondHealthBar = (float)bondHp / bondHpMax;
		
			if (facingRight) {
				bullet.orientation = 1f;
				nade.orientation = 1f;
			} else {
				bullet.orientation = -1f;
				nade.orientation = -1f;
			}
		

			if (bondHp > 0) {
				anim.SetBool ("Ground", grounded);
				groundRadius = 0.2f;
				if (!nadeThrow.NowIsThrowing) {
					rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
					if (!bondFly.isFlying && !shoot.nowIsReload && grounded && Input.GetKeyDown (KeyCode.Space)) {
						anim.SetBool ("Ground", false);
						rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
					}
					if (move > 0 && !facingRight)
						Flip ();
					else if (move < 0 && facingRight)
						Flip ();
				}
			} else {
				anim.SetBool ("Dead", true);
				groundRadius = 0.1f;
				rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
				deadwindow=true;
				if(onLift){
					rigidbody2D.isKinematic=false;
				}
				else
				if(grounded){
					rigidbody2D.isKinematic=true;
				}
			}
		
			if (onEnemyHead)
				anim.SetBool ("Ground", onEnemyHead);

			if(Input.GetKey(KeyCode.F3) && bondHp>0){
				bondHpMax=int.MaxValue; 
				bondHp=int.MaxValue;
			}
		}
	}

    void OnGUI(){
		GUI.skin = mySkin;
		if (loadingTime < 100 ) {
			loadingLevel = true;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), toolBar [3]);
			GUI.DrawTexture (new Rect (0, Screen.height - 30, loadingTime / 100 * Screen.width, 30), toolBar [4]);
			loadingTime += Time.deltaTime * 100f;
		} else
			loadingLevel = false;
		if (bondHp > 0 && !loadingLevel) {
			BulletShotFromBond shoot = GetComponent<BulletShotFromBond> ();
			NadeThrowing nade=GetComponent<NadeThrowing>();
			GUI.Box (new Rect (9, 5, 90 * bondHealthBar, 10), " ", GUI.skin.GetStyle ("bondBar"));
			GUI.Box (new Rect (0, 0, 100, 20), " ", GUI.skin.GetStyle ("BondFon"));
			GUI.DrawTexture(new Rect(0,15,40,40),toolBar[0]);
			GUI.DrawTexture(new Rect(0,40,40,40),toolBar[1]);
			GUI.Box (new Rect (50, 20, 40, 40),shoot.bulletsCount.ToString() + "/" + shoot.maxBulletsCount.ToString(),GUI.skin.GetStyle ("barNum"));
			GUI.DrawTexture(new Rect(130,20,20,20),toolBar[2]);
			GUI.Label(new Rect(150,20,40,40),shoot.AllBulletsCount.ToString(),GUI.skin.GetStyle ("barNum"));
			GUI.Box (new Rect(50,45,40,40),nade.nadeCount.ToString()+"/" + nade.maxNadeCount.ToString(),GUI.skin.GetStyle ("barNum"));
		}
	}
}
