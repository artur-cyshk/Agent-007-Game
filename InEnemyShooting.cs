using UnityEngine;
using System.Collections;

public class InEnemyShooting : MonoBehaviour {
	public int hp=5;
	public int maxhp;
	public float healthBar;
	public GUISkin mySkin;
	public bool barIsVisible;
	public EnemyMoving1 enemyMoving;
	private Animator anim;
	public float width;
	public float height;
	public GameObject head;

	public void Damage(int bulletDam){
		hp -= bulletDam;

		if (hp <= 0) {
			anim.SetTrigger("deadEnemy");
			enemyMoving.speed=0f;
			collider2D.isTrigger=true;
			GameObject.Destroy(head);
		}
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		maxhp = hp;
		barIsVisible = false;
		width = -gameObject.transform.localScale.x/2*100f;
		height = gameObject.transform.localScale.y*100f;
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "bullet") {
			BulletMove bullet=col.gameObject.GetComponent<BulletMove>();	
			if(bullet != null){
				Damage (bullet.damage);

			}
		}
		if (col.gameObject.tag == "nade") {
			NadeMove nade=col.gameObject.GetComponent<NadeMove>();
			if(nade != null){
				Damage (nade.damage);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		healthBar = (float)hp / maxhp;
		if (hp < maxhp)
			barIsVisible = true;
	}

	void OnGUI(){
		if (barIsVisible && hp>0) {
			GUI.skin = mySkin;
			Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			Rect position = new Rect(screenPosition.x-width, Screen.height - screenPosition.y-height, 40f, 10f);
			GUI.Box(position,"",GUI.skin.GetStyle("fon"));
			GUI.Box(new Rect(screenPosition.x-width, Screen.height - screenPosition.y-height,40f*healthBar,10f),"",GUI.skin.GetStyle("bar"));
		}
	}

}
