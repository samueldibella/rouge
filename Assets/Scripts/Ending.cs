using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			Generation.maxEnemies = 5;
			Application.LoadLevel("TitleScreen");
		}
	}
}
