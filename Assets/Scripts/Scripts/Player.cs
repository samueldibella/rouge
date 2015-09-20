using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int playerSpeed;
	public bool fatigued = true;
	GameObject manager;
	char currentTurn;
	int playerNumber;
	int x;
	int y;
	// Use this for initialization
	void Start () {
		manager = transform.parent.gameObject;
	}

	// Update is called once per frame
	void Update () {
		if(!fatigued && playerNumber == 1 && manager.GetComponent<GameState>().currentTurn == GameState.Turns.playerOne) {

			if(Input.GetKeyDown(KeyCode.W)) {
				manager.GetComponent<GameState>().move(this.gameObject, 'n');
			} else if (Input.GetKeyDown(KeyCode.A)) {
				manager.GetComponent<GameState>().move(this.gameObject, 'w');
			} else if (Input.GetKeyDown(KeyCode.S)) {
				manager.GetComponent<GameState>().move(this.gameObject, 's');
			} else if (Input.GetKeyDown(KeyCode.D)) {
				manager.GetComponent<GameState>().move(this.gameObject, 'e');
			}
		} else if(!fatigued && playerNumber == 2 && manager.GetComponent<GameState>().currentTurn == GameState.Turns.playerTwo) {
			if(Input.GetKeyDown(KeyCode.Y)) {
				manager.GetComponent<GameState>().move(this.gameObject, 'n');
			} else if (Input.GetKeyDown(KeyCode.G)) {
				manager.GetComponent<GameState>().move(this.gameObject, 'w');
			} else if (Input.GetKeyDown(KeyCode.H)) {
				manager.GetComponent<GameState>().move(this.gameObject, 's');
			} else if (Input.GetKeyDown(KeyCode.J)) {
				manager.GetComponent<GameState>().move(this.gameObject, 'e');
			}
		}
	}

	public void setPlayerNum(int pNum) {
		playerNumber = pNum;
	}


}
