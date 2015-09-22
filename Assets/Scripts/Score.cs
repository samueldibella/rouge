using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GetComponent<TextMesh>().text = "The serpents swallowed " + GameState.highScore + " stones, but now";
	}
}
