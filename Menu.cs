
using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public GUISkin myskin;
	public Texture2D[] texture;
	public float loadingTime=0;
	// Use this for initialization
	void Start () {

	}

	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.skin = myskin;
	


		if (GUI.Button (new Rect (0, 0, 100, 100), texture [0])) {

		}
		if (GUI.Button (new Rect (100, 0, 100, 100),texture[1]))
			Application.LoadLevel ("scene2");
		if (GUI.Button (new Rect (200, 0, 100, 100),texture[2]))
			Application.LoadLevel ("scene2");
		if (GUI.Button (new Rect (300, 0, 100, 100),texture[3]))
			Application.Quit();
	}
}
