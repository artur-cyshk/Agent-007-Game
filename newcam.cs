using UnityEngine;
using System.Collections;

public class newcam : MonoBehaviour {
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public BondMoving bond;
	public GUISkin myskin;
	// Update is called once per frame

	void Start(){
		//target= GameObject.Find("Bond(Clone)").transform;
	}
	void Update () 
	{

		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(new Vector3(target.position.x, target.position.y,target.position.z));
			Vector3 delta = new Vector3(target.position.x, target.position.y,target.position.z) - camera.ViewportToWorldPoint(new Vector3(0.3f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			
			
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
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
		if (bond.deadwindow) {
			GUI.skin.GetStyle("FlyTime").fontSize=(int)(Screen.height*0.06);
			GUI.Label (new Rect (Screen.width / 2-Screen.width*0.1f, Screen.height / 2-Screen.width*0.1f, Screen.width/2, Screen.height*0.1f), "YOU DEAD", GUI.skin.GetStyle ("FlyTime"));
			GUI.Label (new Rect (Screen.width / 2-Screen.width*0.14f, Screen.height / 2, Screen.width/2, Screen.height*0.1f), "press F1 to restart" , GUI.skin.GetStyle ("FlyTime"));
			GUI.Label (new Rect (Screen.width / 2-Screen.width*0.18f, Screen.height / 2+Screen.width*0.1f, Screen.width/2, Screen.height*0.1f), "press ECS to main menu", GUI.skin.GetStyle ("FlyTime"));
		}
	}
}

