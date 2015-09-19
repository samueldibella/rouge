using UnityEngine;
using System.Collections;

public class TileStat : MonoBehaviour {

	GameObject manager;
	public int x;
	public int y;
	public bool occupied = false;

	// Use this for initialization
	void Start () {
	//	manager = transform.parent.GameObject;
	}

	// Update is called once per frame
	void Update () {

	}
/*
	public GameObject getNeighbor(char dir) {
		switch(dir) {
			case 'n':
				return manager.GetComponent<GameState>().tiles[y - 1, x];
			case 'w':
				return manager.GetComponent<GameState>().tiles[y, x - 1];
			case 'e':
				return manager.GetComponent<GameState>().tiles[y, x + 1];
			case 's':
				return manager.GetComponent<GameState>().tiles[y + 1, x];
			default:
				return null;
		}
	}*/
}
