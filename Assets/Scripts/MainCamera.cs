using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	GameObject p1;
	GameObject p2;
	Vector2 lerp;
	Vector3 destination;
	public float cameraSpeed;
	float minOrtho;
	float maxOrtho;
	float lerpOrtho;

	// Use this for initialization
	void Start () {
		p1 = transform.parent.gameObject.GetComponent<GameState>().playerOne;
		p2 = transform.parent.gameObject.GetComponent<GameState>().playerTwo;
		lerp = Vector2.Lerp(p1.transform.position, p2.transform.position, .5f);
		cameraSpeed = 1f;
		transform.position = new Vector3(lerp.x, lerp.y, transform.position.z);
		minOrtho = 5;
		maxOrtho = 10;
	}

	// Update is called once per frame
	void Update () {
		lerp = Vector2.Lerp(p1.transform.position, p2.transform.position, .5f);

		transform.position = Vector3.Lerp (transform.position, new Vector3(lerp.x, lerp.y, -1f), 2f * Time.deltaTime);
		lerpOrtho =  ((Mathf.Abs(p1.transform.position.sqrMagnitude - p2.transform.position.sqrMagnitude)) * .05f) - 5;

		if(lerpOrtho < minOrtho) {
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, minOrtho, cameraSpeed * Time.deltaTime);
		} else if(lerpOrtho > maxOrtho) {
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,  maxOrtho, cameraSpeed * Time.deltaTime);
		} else {
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,  lerpOrtho, cameraSpeed * Time.deltaTime);
		}

	}
}
