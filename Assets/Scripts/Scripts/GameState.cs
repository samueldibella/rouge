using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject playerPrefab;
	public GameObject playerOne;
	public GameObject playerTwo;
	public GameObject[,] tiles;
	public GameObject turnGUI;
	public int mapHeight;
	public int mapWidth;

	//game state vars
	float timeTurner;
	float turnLength;
	public enum Turns {playerOne, gameOne, playerTwo, gameTwo};
	public Turns currentTurn = Turns.playerOne;
	int globalTurn;
	public	char currentMove = ' ';


	Vector3 firstTile;
	Vector3 secondTile;
	GameObject playerOneStart;
	GameObject playerTwoStart;
	public int tileScale;
	int firstX;
	int firstY;
	int secondX;
	int secondY;

	Vector3 originTile;

	// Use this for initialization
	void Awake () {
		globalTurn = 0;
		mapHeight = 40;
		mapWidth = 40;
		tileScale = 1;

		turnLength = 1.0f;
		timeTurner = 0f;

		originTile = Vector3.zero;

		//quadrant p1 pick
		switch(Random.Range(0,4)) {
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
				firstY = (3 * mapWidth)/4;
				break;
		}

		firstX += Random.Range(-2, 2);
		firstY += Random.Range(-2, 2);
		firstTile = new Vector3(firstX, firstY, 0);
		secondTile = firstTile;
		while(secondTile == firstTile) {
			secondX = Random.Range(-2, 2);
			secondY = Random.Range(-2, 2);
			secondTile = new Vector3(firstTile.x + secondX, firstTile.y + secondY, 0);
		}

		//generate map
		tiles = new GameObject[mapHeight, mapWidth];
		for(int j = 0; j < mapHeight; j++) {
			for(int i = 0; i < mapWidth; i++) {
				tiles[j, i] = Instantiate(tilePrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), firstTile.z), Quaternion.identity) as GameObject;
				tiles[j, i].transform.parent = this.transform;
				tiles[j, i].GetComponent<TileStat>().x = i;
				tiles[j, i].GetComponent<TileStat>().y = j;
			}
		}

		playerOneStart = tiles[tiles[0,0].GetComponent<TileStat>().y + firstY, tiles[0,0].GetComponent<TileStat>().x + firstX];
		playerOne = Instantiate(playerPrefab, new Vector3(firstTile.x, firstTile.y, 0), Quaternion.identity) as GameObject;
		playerOne.GetComponent<Player>().setPlayerNum(1);
		playerOne.GetComponent<Animus>().location = playerOneStart;
		playerOneStart.GetComponent<TileStat>().occupied = true;
		playerOne.transform.parent = this.transform;

		playerTwoStart = tiles[tiles[0,0].GetComponent<TileStat>().y + (firstY + secondY), tiles[0,0].GetComponent<TileStat>().x + (firstX + secondX)];
		playerTwo = Instantiate(playerPrefab, new Vector3(secondTile.x, secondTile.y, 0), Quaternion.identity) as GameObject;
		playerTwo.GetComponent<Player>().setPlayerNum(2);
		playerTwo.GetComponent<Animus>().location = playerTwoStart;
		playerTwoStart.GetComponent<TileStat>().occupied = true;
		playerTwo.transform.parent = this.transform;
	}

	void Start () {
		currentTurn = Turns.playerOne;
		turnGUI.GetComponent<Text>().text = "p1";
		playerOne.GetComponent<Player>().fatigued = false;
		timeTurner = turnLength;

		Debug.Log(playerOne.GetComponent<Animus>().location);
	}

	void Update () {
		timeTurner = timeTurner - Time.deltaTime;
		if (timeTurner <= 0) {
			iterateTurns();
			timeTurner = turnLength;
		}
	}

	void iterateTurns() {
		switch(currentTurn) {
		case Turns.playerOne:
			turnGUI.GetComponent<Text>().text = "g1";
			currentTurn = Turns.gameOne;
			break;
		case Turns.playerTwo:
			turnGUI.GetComponent<Text>().text = "g2";
			currentTurn = Turns.gameTwo;
			break;
		case Turns.gameOne:
			//monster 1's move calc
			turnGUI.GetComponent<Text>().text = "p2";
			playerTwo.GetComponent<Player>().fatigued = false;
			currentTurn = Turns.playerTwo;
			break;
		case Turns.gameTwo:
			//monster 2's move calc
			turnGUI.GetComponent<Text>().text = "p1";
			playerOne.GetComponent<Player>().fatigued = false;
			currentTurn = Turns.playerOne;
			break;
		}
	}

	public void move(GameObject piece, char dir) {

		GameObject goalTile = piece.GetComponent<Animus>().location;
		int goalX = piece.GetComponent<Animus>().location.GetComponent<TileStat>().x;
		int goalY = piece.GetComponent<Animus>().location.GetComponent<TileStat>().y;

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

		stopTurn();
	}

	public void stopTurn() {
		timeTurner = turnLength;
		iterateTurns();
	}
}
