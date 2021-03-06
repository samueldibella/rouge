﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject segmentPrefab;
	GameObject manager;
	GameObject segmentHolder;
	public GameObject tail;
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
		tail = this.gameObject;
	}

	// Update is called once per frame
	void Update () {
		switch(playerNumber) {
			case 1:
			if(Input.GetKeyDown(KeyCode.W) && GetComponent<Animus>().lastDir != 's') {
				this.GetComponent<Animus>().dir = 'n';
			} else if (Input.GetKeyDown(KeyCode.A) && GetComponent<Animus>().lastDir != 'e') {
				this.GetComponent<Animus>().dir = 'w';
			} else if (Input.GetKeyDown(KeyCode.S) && GetComponent<Animus>().lastDir != 'n') {
				this.GetComponent<Animus>().dir = 's';
			} else if (Input.GetKeyDown(KeyCode.D) && GetComponent<Animus>().lastDir != 'w') {
				this.GetComponent<Animus>().dir = 'e';
			}

			if(this.gameObject.GetComponent<Animus>().moved == true && manager.GetComponent<GameState>().playerTwo.GetComponent<Animus>().moved == false) {
				manager.GetComponent<GameState>().playerTwo.GetComponent<Animus>().dir = this.GetComponent<Animus>().dir;
			}
				break;
			case 2:
				if(Input.GetKeyDown(KeyCode.I) && GetComponent<Animus>().lastDir != 's') {
					this.GetComponent<Animus>().dir = 'n';
				} else if (Input.GetKeyDown(KeyCode.J) && GetComponent<Animus>().lastDir != 'e') {
					this.GetComponent<Animus>().dir = 'w';
				} else if (Input.GetKeyDown(KeyCode.K) && GetComponent<Animus>().lastDir != 'n') {
					this.GetComponent<Animus>().dir = 's';
				} else if (Input.GetKeyDown(KeyCode.L) && GetComponent<Animus>().lastDir != 'w') {
					this.GetComponent<Animus>().dir = 'e';
				}

				if(this.gameObject.GetComponent<Animus>().moved == true && 			manager.GetComponent<GameState>().playerOne.GetComponent<Animus>().moved == false) {
				manager.GetComponent<GameState>().playerOne.GetComponent<Animus>().dir = this.GetComponent<Animus>().dir;
				}
				break;
		}
/*
		if(length > 1) {
			segmentTraversal = this.gameObject.transform;

			for(int i = 1; i < length; i++) {
				segmentTraversal = segmentTraversal.GetChild(0);
			}

			for(int i = length; i > 1; i--) {
				Debug.Log(this.gameObject.transform.parent);

			}

		}*/

	}

	public void setPlayerNum(int pNum) {
		playerNumber = pNum;
	}

	//tile is location new segment appears
	public void grow(GameObject tile) {
		segmentHolder = Instantiate(segmentPrefab, tile.transform.position, Quaternion.identity) as GameObject;
		segmentHolder.transform.parent = tail.transform;

		if(playerNumber == 2) {
			segmentHolder.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.black;
		}
		segmentHolder.GetComponent<Animus>().location = tile;
		segmentHolder.GetComponent<Animus>().dir = GetComponent<Animus>().dir;
		tile.GetComponent<TileStat>().occupied = true;
		tile.GetComponent<TileStat>().occupant = segmentHolder;
		segmentHolder.transform.parent = tail.transform;
		tail = segmentHolder;
		length++;
	}
}
