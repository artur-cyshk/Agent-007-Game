using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour 
{
	public delegate void Respawn(float time);
	public event Respawn RespawnMe;
	public delegate void SendMessage(string MessageOverlay);
	public event SendMessage SendNetworkMessage;
	[SerializeField] Animator anim;
	Vector3 position;
	Vector3 shootPos;
	float smoothing = 10f;
	bool ground=true;
	float Speed = 0f;
	float vSpeed = 0f;
	bool initialLoad = true;
	bool IsReload=false;
	bool IsShoot=false;

	public Vector3 bulletPosition;
	
	public float shootRate=0.4f;
	public float shootCooldown=0f;
	public int bulletsCount;
	public int maxBulletsCount = 10;
	public int AllBulletsCount=150;
	public bool nowIsReload=false;
	public bool nowIsShoot=false;
	public AudioClip myaudio;

	public bool facingRight = true;
	public bool grounded = false;
	public bool onLift = false;
	public bool loadingLevel=false;
	public bool exitWindow=false;
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
	public BulletNetworkMove bullet;
	public LayerMask whatIsGround;
	public GUISkin mySkin;
	public Texture2D[] toolBar;
	public bool face;
	public bool now = false;
	public bool isVisibleMenu=false;
	void Start () {
		anim = GetComponent<Animator> ();
		if(photonView.isMine)
		{

			shootCooldown=0f;
			bulletsCount=maxBulletsCount;
			bondHpMax = bondHp;
			foreach(SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
			{
				spr.enabled=true;
			}
			foreach(Camera cam in GetComponentsInChildren<Camera>())
			{
				cam.enabled = true;
			}
			now=true;
		}
		else{
			StartCoroutine("UpdateData");
		}
	}
	IEnumerator UpdateData () 
	{
		if(initialLoad)
		{
			initialLoad = false;
			transform.position = position;
		}
		
		while(true)
		{
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
			anim.SetBool("Ground", ground);
			anim.SetBool("IsShoot", IsShoot);
			anim.SetFloat ("Speed",Speed );
			anim.SetFloat ("vSpeed", vSpeed);
			if(facingRight!=face){
				Flip();
			}
			if(IsReload){
				anim.SetTrigger("IsReload");
			}
			yield return null;
		}

	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(anim.GetBool ("Ground"));
			stream.SendNext(anim.GetBool ("IsShoot"));
			stream.SendNext(anim.GetFloat("Speed"));
			stream.SendNext(anim.GetFloat("vSpeed"));
			stream.SendNext(facingRight);
			stream.SendNext(nowIsReload);

		}
		else
		{
			position = (Vector3)stream.ReceiveNext();
			ground = (bool)stream.ReceiveNext();
			IsShoot = (bool)stream.ReceiveNext();
			Speed = (float)stream.ReceiveNext();
			vSpeed = (float)stream.ReceiveNext();
			face=(bool)stream.ReceiveNext();
			IsReload = (bool)stream.ReceiveNext();

		}
	}

	public void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}	

	void Update()
	{
		if (photonView.isMine && now) {
			if (isVisibleMenu) {
				exitWindow = true;
			} else {
				exitWindow = false;
			}

			var currentState = anim.GetCurrentAnimatorStateInfo (0);
			
			if (shootCooldown > 0) {
				shootCooldown -= Time.deltaTime;
			}
			//стрельба
			if (Input.GetKeyDown (KeyCode.Mouse0) && bondHp>0) {
				nowIsShoot=true;
				anim.SetBool("IsShoot",true);
			}
			if(Input.GetKeyUp(KeyCode.Mouse0) ||  bulletsCount<=0){
				anim.SetBool("IsShoot",false);
				nowIsShoot = false;
			}
			
			if (nowIsShoot && bulletsCount>0) {
				Attack();
			}
			//перезарядка
			if (bondHp>0 && !nowIsShoot && Input.GetKeyDown (KeyCode.R) && bulletsCount<maxBulletsCount && AllBulletsCount>0 && grounded && isShooting==false) {
				anim.SetTrigger ("IsReload");
				if(AllBulletsCount<10)
					bulletsCount=AllBulletsCount;
				else
					bulletsCount=maxBulletsCount;
			}
			if(currentState.IsName("reload")){
				nowIsReload=true;
			}
			else
				nowIsReload=false;


			if(Input.GetKeyDown (KeyCode.F2)){
				PhotonNetwork.Disconnect();
				Application.LoadLevel("scene0");
			}
			if (Input.GetKeyDown (KeyCode.F1)) {
				PhotonNetwork.Disconnect();	
				Application.LoadLevel(Application.loadedLevel);
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				isVisibleMenu=!isVisibleMenu;
			}
			grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
			move = Input.GetAxis ("Horizontal");
			anim.SetFloat ("Speed", Mathf.Abs(move));
			anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
			bondHealthBar = (float)bondHp / bondHpMax;
			
			if (facingRight) {
				bullet.orientation = 1f;
			} else {
				bullet.orientation = -1f;
			}
			
			
			if (bondHp > 0) {
				anim.SetBool ("Ground", grounded);
				groundRadius = 0.2f;
				rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
				if (!nowIsReload && grounded && Input.GetKeyDown (KeyCode.Space)) {
					anim.SetBool ("Ground", false);
					rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
				}
				if (move > 0 && !facingRight)
					Flip ();
				else if (move < 0 && facingRight)
					Flip ();
			} else {
				RespawnMe(1f);
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}

	

	public void Damage(int bulletCount){
		bondHp -= bulletCount;
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if (photonView.isMine) {
			if (col.gameObject.tag == "platformVert") {
				rigidbody2D.gravityScale = 3f;
				jumpForce = 420f;
				onLift = true;
			}
			if (col.gameObject.tag == "bullet") {
				BulletNetworkMove bullet = col.gameObject.GetComponent<BulletNetworkMove> ();	
				if (bullet != null) {
					Damage (bullet.damage);
				}
			}
		}
	}
	
	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "platformVert") {
			if(photonView.isMine){
				rigidbody2D.gravityScale=0.7f;
				jumpForce=350f;
				onLift=false;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (photonView.isMine) {
			if (col.gameObject.tag == "enemyfull" || col.gameObject.tag == "deadzone") {
				RespawnMe(1f);
				PhotonNetwork.Destroy(gameObject);
			}
		
			if (col.gameObject.tag == "medicine") {
				if (bondHp < bondHpMax) {
					bondHp++;
					PhotonNetwork.Destroy(col.gameObject);
				}
			}
			if (col.gameObject.tag == "case") {
				AllBulletsCount += 10;
				PhotonNetwork.Destroy(col.gameObject);
			}
		}
	}
	
	void OnGUI(){
		GUI.skin = mySkin;
		if (photonView.isMine && now) {
			if (loadingTime < 100) {
				loadingLevel = true;
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), toolBar [3]);
				GUI.DrawTexture (new Rect (0, Screen.height - 30, loadingTime / 100 * Screen.width, 30), toolBar [4]);
				loadingTime += Time.deltaTime * 100f;
			} else
				loadingLevel = false;
			if (bondHp > 0 && !loadingLevel) {

				GUI.Box (new Rect (9, 5, 90 * bondHealthBar, 10), " ", GUI.skin.GetStyle ("bondBar"));
				GUI.Box (new Rect (0, 0, 100, 20), " ", GUI.skin.GetStyle ("BondFon"));
				GUI.DrawTexture (new Rect (0, 15, 40, 40), toolBar [0]);
				GUI.Box (new Rect (50, 20, 40, 40), bulletsCount.ToString () + "/" + maxBulletsCount.ToString (), GUI.skin.GetStyle ("barNum"));
				GUI.DrawTexture (new Rect (130, 20, 20, 20), toolBar [2]);
				GUI.Label (new Rect (150, 20, 40, 40), AllBulletsCount.ToString (), GUI.skin.GetStyle ("barNum"));
			}
			if(exitWindow){
				GUI.skin.GetStyle("FlyTime").fontSize=(int)(Screen.height*0.06);
				GUI.Label (new Rect (Screen.width / 2-Screen.width*0.14f, Screen.height / 2, Screen.width/2, Screen.height*0.1f), "F1 - LOBBY" , GUI.skin.GetStyle ("FlyTime"));
				GUI.Label (new Rect (Screen.width / 2-Screen.width*0.16f, Screen.height / 2+Screen.width*0.1f, Screen.width/2, Screen.height*0.1f), "F2 - MAIN MENU", GUI.skin.GetStyle ("FlyTime"));
			}
			if (bulletsCount <= 0 && AllBulletsCount > 0) {
				GUI.Label (new Rect (100, 0, 100, 20), "Press R to reload", GUI.skin.GetStyle ("barNum"));
			}
		}
	}

	public void Attack(){
		
		if (bondHp>0 && CanShoot && !nowIsReload && photonView.isMine && now) {
			audio.PlayOneShot (myaudio);
			bulletsCount--;
			AllBulletsCount--;
			shootCooldown=shootRate;
			if(facingRight){
				bulletPosition=new Vector3(transform.position.x+0.5f,transform.position.y+0.1f,0);
			}
			else{
				bulletPosition=new Vector3(transform.position.x-0.5f,transform.position.y+0.1f,0);
			}
			PhotonNetwork.Instantiate("bullet",bulletPosition,Quaternion.identity,0);

		}
	}

	public bool CanShoot{
		get{
			return shootCooldown<=0f;
		}
	}
}
