using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int playerSpeed;
	GameObject manager;
	char currentTurn;
	int playerNumber;

	// Use this for initialization
	void Start (int num) {
		playerNumber = num;
		manager = transform.parent.gameObject;
		currentGameMove = manager.GetComponent<GameState>().currentMove;
	}

	// Update is called once per frame
	void Update () {
		if(playerNumber == 1 && manager.GetComponent<GameState>().currentTurn == Turns.playerOne) {
			if(Input.GetKeyDown(KeyCode.W)) {
				this.SendMessageUpwards("move", this, 'n');
			} else if (Input.GetKeyDown(KeyCode.A)) {
				this.SendMessageUpwards("move", this, 'w');
			} else if (Input.GetKeyDown(KeyCode.S)) {
				this.SendMessageUpwards("move", this, 's');
			} else if (Input.GetKeyDown(KeyCode.D)) {
				this.SendMessageUpwards("move", this, 'e');
			}
		} else if(playerNumber == 2 && manager.GetComponent<GameState>().currentTurn == Turns.playerTwo) {
			if(Input.GetKeyDown(KeyCode.Y)) {
				this.SendMessageUpwards("move", this, 'n');
			} else if (Input.GetKeyDown(KeyCode.G)) {
				this.SendMessageUpwards("move", this, 'w');
			} else if (Input.GetKeyDown(KeyCode.H)) {
				this.SendMessageUpwards("move", this, 's');
			} else if (Input.GetKeyDown(KeyCode.J)) {
				this.SendMessageUpwards("move", this, 'e');
			}
		}
	}
}
