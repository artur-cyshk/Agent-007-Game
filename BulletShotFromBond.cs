using UnityEngine;
using System.Collections;

public class BulletShotFromBond : MonoBehaviour {
	public Transform shootPrefab;
	public float shootRate=0.4f;
	public float shootCooldown=0f;
	private Animator anim;
	public BondMoving bond;
	public int bulletsCount=10;
	public int maxBulletsCount = 10;
	public int AllBulletsCount=150;
	public bool nowIsReload=false;
	public bool nowIsShoot=false;
	NadeThrowing nadeThrow;
	BondFlying bondFly;
	public GUISkin myskin;
	public AudioClip myaudio;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		shootCooldown=0f;
		bulletsCount=maxBulletsCount;
		
	}
	
	void Update () {
		nadeThrow = GetComponent<NadeThrowing> ();
		bondFly = GetComponent<BondFlying> ();
		var currentState = anim.GetCurrentAnimatorStateInfo (0);
		
		if (shootCooldown > 0) {
			shootCooldown -= Time.deltaTime;
		}
		//стрельба
		if (Input.GetKeyDown (KeyCode.Mouse0) && bond.bondHp>0) {
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
		if (bond.bondHp>0 && !nowIsShoot && !nadeThrow.NowIsThrowing && !bondFly.isFlying && Input.GetKeyDown (KeyCode.R) && bulletsCount<maxBulletsCount && AllBulletsCount>0 && bond.grounded && bond.isShooting==false) {
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
	}
	public void bulletsChange(){
		bulletsCount--;
		AllBulletsCount--;
	}
	public void Attack(){
		
		nadeThrow = GetComponent<NadeThrowing> ();
		bondFly = GetComponent<BondFlying> ();
		if (bond.bondHp>0 && CanShoot && !nowIsReload && !nadeThrow.NowIsThrowing && !bondFly.isFlying) {
			audio.PlayOneShot (myaudio);
			bulletsChange();
			shootCooldown=shootRate;
			var shootTransform=Instantiate(shootPrefab) as Transform;
			if(bond.facingRight){
				shootTransform.position=new Vector2(transform.position.x+0.3f,transform.position.y+0.1f);
			}
			else{
				shootTransform.position=new Vector2(transform.position.x-0.3f,transform.position.y+0.1f);
			}
		}
	}
	
	public bool CanShoot{
		get{
			return shootCooldown<=0f;
		}
	}
	
	void OnGUI(){
		GUI.color = Color.red;
		GUI.skin = myskin;
		if (bulletsCount <= 0 && AllBulletsCount > 0) {
			GUI.Label (new Rect (100, 0, 100, 20), "Press R to reload", GUI.skin.GetStyle ("barNum"));
		}
	}
}


