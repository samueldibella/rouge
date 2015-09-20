using UnityEngine;
using System.Collections;

public class TileStat : MonoBehaviour {

	GameObject manager;
	Color color;
	public int x;
	public int y;
	public bool occupied = false;
	public bool lightUpdated;

	// Use this for initialization
	void Start () {
		manager = transform.parent.gameObject;
		color = this.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color;
 		color.a = 255f;
		this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
	}

	// Update is called once per frame
	void Update () {
		this.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", color);
	}

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
	}

	public void updateLight(int yDisplacement, int xDisplacement) {

	color.a = Mathf.Lerp(color.a, 0f, 200f);

	this.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", color);
	}

	public void updateLight() {

	color.a = Mathf.Lerp(color.a, 0f, 4f * Time.deltaTime);

	this.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", color);
	}

	public void deprecateLight() {

	color.a = Mathf.Lerp(color.a, 255f, .001f * Time.deltaTime);

	this.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", color);
	}
}
