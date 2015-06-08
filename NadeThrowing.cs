using UnityEngine;
using System.Collections;

public class NadeThrowing : MonoBehaviour {
	public Transform nadePrefab;
	public float nadeRate=1f;
	public float nadeCooldown;
	public int nadeCount;
	public int maxNadeCount=5;
	public bool NowIsThrowing=false;
	public BondMoving bond;
	private Animator anim;
	public BulletShotFromBond bullet;
	public BondFlying bondFly;
	AnimatorStateInfo currentState;
	public bool CanThrowFromTime{
		get{
			return nadeCooldown<=0f;
		}
	}
	// Use this for initialization
	public void Start () {
		nadeCooldown = 0f;
		nadeCount = maxNadeCount;
		anim = GetComponent<Animator>();
		bullet = GetComponent<BulletShotFromBond> ();
		bondFly = GetComponent<BondFlying> ();

	}
	
	// Update is called once per frame
	public void Update () {

		currentState = anim.GetCurrentAnimatorStateInfo (0);
		if (nadeCooldown > 0) {
			nadeCooldown -= Time.deltaTime;
		}
		
		if (!bondFly.isFlying && CanThrowFromTime && Input.GetKeyDown (KeyCode.Q) && !bullet.nowIsReload && bond.bondHp>0 && !bullet.nowIsShoot && nadeCount>0 && bond.grounded && bond.move == 0) {
			changeNadeCount();
			nadeCooldown=nadeRate;
			anim.SetTrigger ("ThrowNade");	
			//Throw();
		}

		if(currentState.IsName("ThrowNade")){
			NowIsThrowing=true;
		}
		else
			NowIsThrowing = false;
	}
	public void changeNadeCount(){
		nadeCount--;
	}
	public void Throw(){

			var nadeTransform=Instantiate(nadePrefab) as Transform;
			if(bond.facingRight){
				nadeTransform.position=new Vector2(transform.position.x-0.1f,transform.position.y+0.6f);
			}
			else{
				nadeTransform.position=new Vector2(transform.position.x+0.1f,transform.position.y+0.6f);
			}
	}
}
