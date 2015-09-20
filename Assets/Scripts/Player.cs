using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject segmentPrefab;
	GameObject manager;
	GameObject segmentHolder;
	Transform segmentTraversal;
	public int length;
	int playerNumber;
	int x;
	int y;
	char currentTurn;
	public int playerSpeed;

	// Use this for initialization
	void Start () {
		manager = transform.parent.gameObject;
		length = 1;
	}

	// Update is called once per frame
	void Update () {
		switch(playerNumber) {
			case 1:
			if(Input.GetKeyDown(KeyCode.W)) {
				this.GetComponent<Animus>().dir = 'n';
			} else if (Input.GetKeyDown(KeyCode.A)) {
				this.GetComponent<Animus>().dir = 'w';
			} else if (Input.GetKeyDown(KeyCode.S)) {
				this.GetComponent<Animus>().dir = 's';
			} else if (Input.GetKeyDown(KeyCode.D)) {
				this.GetComponent<Animus>().dir = 'e';
			}
				break;
			case 2:
			if(Input.GetKeyDown(KeyCode.I)) {
				this.GetComponent<Animus>().dir = 'n';
			} else if (Input.GetKeyDown(KeyCode.J)) {
				this.GetComponent<Animus>().dir = 'w';
			} else if (Input.GetKeyDown(KeyCode.K)) {
				this.GetComponent<Animus>().dir = 's';
			} else if (Input.GetKeyDown(KeyCode.L)) {
				this.GetComponent<Animus>().dir = 'e';
			}
				break;
		}

	}

	public void setPlayerNum(int pNum) {
		playerNumber = pNum;
	}

	//tile is location new segment appears
	public void grow(GameObject tile) {
			segmentHolder = Instantiate(segmentPrefab, tile.transform.position, Quaternion.identity) as GameObject;
			segmentTraversal = this.gameObject.transform;

		if(length == 1) {
		} else {
			for(int i = 1; i < length; i++) {
				segmentTraversal = segmentTraversal.GetChild(0);
			}
		}

		segmentHolder.transform.parent = segmentTraversal;
		length++;
	}
}
