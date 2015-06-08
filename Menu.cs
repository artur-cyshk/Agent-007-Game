
using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public void loadHistory(){
		Application.LoadLevel ("scene1");
	}

	public void loadOnline(){
		Application.LoadLevel ("multik");
	}

	public void exitGame(){
		Application.Quit ();
	}
}
