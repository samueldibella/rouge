using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject playerOne;
	public GameObject playerTwo;
	public GameObject[,] tiles;
	public Material exitLook;
	public int mapHeight;
	public int mapWidth;

	//game state vars
	int globalTurn;
	public enum Turns {playerOne, gameOne, playerTwo, gameTwo};
	public Turns currentTurn = Turns.playerOne;
	public	char currentMove = '';
	int turnSpeed;

	Vector3 firstTile;
	public int tileScale;
	int firstX;
	int firstY;
	int playerStartY;

	// Use this for initialization
	void Awake () {
		globalTurn = 0;
		mapHeight = 40;
		mapWidth = 40;
		turnSpeed = 5;
		tileScale = 1;

		//quadrant p1 pick
		switch(Math.random(0,4) {
			case 0:
				firstX = (3 * mapWidth)/4;
				firstY = mapWidth/4;
				break;
			case 1:
				firstX = mapWidth/4;
				firstY = mapWidth/4;
				break;
			case 2:
				firstX = mapWidth/4;
				firstY = (3 * mapWidth)/4;
				break;
			case 3:
				firstX = (3 * mapWidth)/4;
				firstY = (3 * mapWidt)/4;
				break;
		}
		firstX += Math.Random(-2, 2);
		firstY += Math.Random(-2, 2);
		firstTile = new Vector3(firstX, firstY, 0);

		secondTile = firstTile;
		while(secondTile == firstTile) {
			secondTile = tiles[firstTile.GetComponent<TileStat>().y + Math.Random(-2, 2), firstTile.GetComponent<TileStat>().x + Math.Random(-2,2)];
		}

		//generate map
		tiles = new GameObject[mapHeight, mapWidth];
		for(int j = 0; j < mapHeight; j++) {
			for(int i = 0; i < mapWidth; i++) {
				tiles[j, i] = Instantiate(tilePrefab, new Vector3(firstTile.x + (i * tileScale), firstTile.y + (j * tileScale), firstTile.z), Quaternion.identity) as GameObject;
				tiles[j, i].transform.parent = this.transform;
				tiles[j, i].GetComponent<TileStat>().x = i;
				tiles[j, i].GetComponent<TileStat>().y = j;
			}
		}

		playerOneLocation = tiles[tiles[0,0].GetComponent<TileStat>().y, tiles[0,0].GetComponent<TileStat>().x];
		playerOne = Instantiate(player(1), new Vector3(firstTile.x, firstTile.y, 0), Quaternion.identity) as GameObject;
		playerOne.GetComponent<Animus>().location = firstTile;
		firstTile.GetComponent<TileStat>().occupied = true;
		playerOne.transform.parent = this.transform;

		playerTwoLocation = tiles[tiles[0,0].GetComponent<TileStat>().y, tiles[0,0].GetComponent<TileStat>().x];
		playerTwo = Instantiate(player(2), new Vector3(secondTile.x, secondTile.y, 0), Quaternion.identity) as GameObject;
		playerTwo.GetComponent<Animus>().location = secondTile;
		secondTile.GetComponent<TileStat>().occupied = true;
		playerTwo.transform.parent = this.transform;
	}

	void Start () {
		StartCoroutine ( turns() );
	}

	void Update () {
	}

	IEnumerator turns() {
		while(true) {
			switch(currentTurn) {
				case playerOne:
					currentTurn = Turns.gameOne;
					break;
				case playerTwo:
					currentTurn = Turns.gameTwo;
					break;
				case gameOne:
					//monster 1's move calc
					currentTurn = Turns.playerTwo;
					break;
				case gameTwo:
					//monster 2's move calc
					currentTurn = Turns.playerOne;
					break;
			}

			globalTurn++;
			yield return WaitForSeconds(turnSpeed);
		}
	}

	void move(GameObject piece, char dir) {
		GameObject goalTile = piece.GetComponent<Animus>().location;
		int goalX = location.GetComponent<TileStat>().x;
		int goalY = location.GetComponent<TileStat>().y;

		switch(dir) {
		case 'n':
			if(goalY < mapHeight - 1 && tiles[goalY + 1, goalX].GetComponent<TileStat>().occupied == false) {
					goalTile = tiles[goalY + 1, goalX];
			}
			break;
		case 'w':
			if(goalX > 0 && tiles[goalY, goalX - 1].GetComponent<TileStat>().occupied == false) {
				goalTile = tiles[goalY, goalX - 1];
			}
			break;
		case 'e':
			if(goalX < mapWidth - 1 && tiles[goalY, goalX + 1].GetComponent<TileStat>().occupied == false) {
				goalTile = tiles[goalY, goalX + 1];
			}
			break;
		case 's':
			if(goalY > 0 && tiles[goalY - 1, goalX].GetComponent<TileStat>().occupied == false) {
				goalTile = tiles[goalY - 1, goalX];
			}
			break;
		default:
			break;
		}

		if(goalTile != piece.GetComponent<Animus>().location) {
			piece.GetComponent<Animus>().location.GetComponent<TileStat>().occupied = false;
		}

		piece.transform.position = goalTile.transform.position;
		piece.GetComponent<Animus>().location = goalTile;
		goalTile.GetComponent<TileStat>().occupied = true;
	}
}
