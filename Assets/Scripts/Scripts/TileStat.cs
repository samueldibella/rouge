using UnityEngine;
using System.Collections;

public class TileStat : MonoBehaviour {

	GameObject manager;
	Color color;
	public float absoluteLight;
	public float actualLight;
	public int x;
	public int y;
	public bool occupied = false;

	// Use this for initialization
	void Start () {
		manager = transform.parent.gameObject;
		color = this.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color;
 		color.a = 0f;
		this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
		absoluteLight = 0;
		actualLight = 0;
	}

	// Update is called once per frame
	void Update () {

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

		absoluteLight = absoluteLight - (yDisplacement * (manager.GetComponent<GameState>().lightRange * manager.GetComponent<GameState>().lightIntensity));
		absoluteLight = absoluteLight - (xDisplacement * (manager.GetComponent<GameState>().lightRange * manager.GetComponent<GameState>().lightIntensity));

		color = this.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color;
		color.a = Mathf.Lerp(actualLight, absoluteLight, 5 * Time.deltaTime);

		this.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", color);
	}
}
