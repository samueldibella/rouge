using UnityEngine;
using System.Collections;

public class Opening : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			Generation.score = 0;
			Generation.maxEnemies = 5; 
			Application.LoadLevel("The Beginning");
		}
	}
}
