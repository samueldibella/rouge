using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	GameObject p1;
	GameObject p2;
	Vector2 lerp;
	Vector3 destination;
	public float cameraSpeed;

	// Use this for initialization
	void Start () {
		p1 = transform.parent.gameObject.GetComponent<GameState>().playerOne;
		p2 = transform.parent.gameObject.GetComponent<GameState>().playerTwo;
		lerp = Vector2.Lerp(p1.transform.position, p2.transform.position, .5f);
		cameraSpeed = .3f;
		transform.position = new Vector3(lerp.x, lerp.y, transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		switch(transform.parent.gameObject.GetComponent<GameState>().currentTurn) {
			case GameState.Turns.playerOne:
				lerp = Vector2.Lerp(p1.transform.position, p2.transform.position, 0);
				break;
			case GameState.Turns.playerTwo:
				lerp = Vector2.Lerp(p1.transform.position, p2.transform.position, 1);
				break;
			default:
				lerp = Vector2.Lerp(p1.transform.position, p2.transform.position, .5f);
				break;
		}

		destination = new Vector3(lerp.x, lerp.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, destination, cameraSpeed * Time.deltaTime);
	}
}
