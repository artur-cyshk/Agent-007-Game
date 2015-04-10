using UnityEngine;
using System.Collections;

public class BulletShootFromEnemy : MonoBehaviour {
	public Transform shootPrefab;
	public float shootRate=0.6f;
	private float shootCooldown=0f;
	public EnemyMoving1 enemyMoving;
	public BulletMove bullet;
	public BondMoving bond;
	public LayerMask whatToHit;
	private Animator anim;
	public InEnemyShooting enemy;
	public AudioClip myaudioshoot;
	public bool inSwap=false;
	public bool CanShoot{
		get{
			return shootCooldown<=0f;
		}
	}
	// Use this for initialization
	void Start () {
		shootCooldown=0f;
		anim = GetComponent<Animator>();
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "enemySwap") {
			inSwap = true;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "enemySwap") {
			inSwap=false;
		}
	}

	void Update () {
		RaycastHit2D hit=Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y+0.2f),new Vector2(enemyMoving.orientation,0),3.5f,whatToHit);
		RaycastHit2D hitback=Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y+0.2f),new Vector2(-enemyMoving.orientation,0),1.5f,whatToHit);

		if (shootCooldown > 0) {
			shootCooldown -= Time.deltaTime;
		}
		if (enemyMoving.enemyFacingRight)
			bullet.orientation = 1f;
		else
			bullet.orientation = -1f;

		if (hitback.collider != null && bond != null && bond.bondHp>0 && enemy.hp>0 && inSwap==false) {
			enemyMoving.Flip();
			enemyMoving.orientation=-enemyMoving.orientation;
		}

		if (hit.collider != null && bond != null && bond.bondHp > 0 && enemy.hp>0 && inSwap==false) {
			enemyMoving.speed = 0f;
			anim.SetBool ("IsStay", true);
			Attack ();
		} else if(enemy.hp>0){
			anim.SetBool ("IsStay", false);
			enemyMoving.speed = 0.5f;
		}


	}
	
	public void Attack(){
		if (CanShoot && enemy.hp>0) {
			audio.PlayOneShot(myaudioshoot);
			shootCooldown=shootRate;
			var shootTransform=Instantiate(shootPrefab) as Transform;
			if(enemyMoving.enemyFacingRight)
				shootTransform.position=new Vector2(transform.position.x+0.3f,transform.position.y+0.2f);
			else
				shootTransform.position=new Vector2(transform.position.x-0.3f,transform.position.y+0.2f);

		}
	}
	

}
