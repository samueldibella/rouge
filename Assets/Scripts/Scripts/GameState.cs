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
	int globalTurn;
	public enum Turns {playerOne, gameOne, playerTwo, gameTwo};
	public Turns currentTurn = Turns.playerOne;
	public	char currentMove = ' ';
	int turnSpeed;

	Vector3 firstTile;
	Vector3 secondTile;
	GameObject playerOneStart;
	GameObject playerTwoStart;
	public int tileScale;
	int firstX;
	int firstY;
	int originX;
	int originY;
	Vector3 originTile;

	// Use this for initialization
	void Awake () {
		globalTurn = 0;
		mapHeight = 40;
		mapWidth = 40;
		turnSpeed = 3;
		tileScale = 1;

		originX = 0;
		originY = 0;
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
			secondTile = new Vector3(firstTile.y + Random.Range(-2, 2), firstTile.x + Random.Range(-2,2), 0);
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

		playerOneStart = tiles[tiles[0,0].GetComponent<TileStat>().y, tiles[0,0].GetComponent<TileStat>().x];
		playerOne = Instantiate(playerPrefab, new Vector3(firstTile.x, firstTile.y, 0), Quaternion.identity) as GameObject;
		playerOne.GetComponent<Player>().setPlayerNum(1);
		playerOne.GetComponent<Animus>().location = playerOneStart;
		playerOneStart.GetComponent<TileStat>().occupied = true;
		playerOne.transform.parent = this.transform;

		playerTwoStart = tiles[tiles[0,0].GetComponent<TileStat>().y, tiles[0,0].GetComponent<TileStat>().x];
		playerTwo = Instantiate(playerPrefab, new Vector3(secondTile.x, secondTile.y, 0), Quaternion.identity) as GameObject;
		playerTwo.GetComponent<Player>().setPlayerNum(2);
		playerTwo.GetComponent<Animus>().location = playerTwoStart;
		playerTwoStart.GetComponent<TileStat>().occupied = true;
		playerTwo.transform.parent = this.transform;
	}

	void Start () {
		currentTurn = Turns.playerOne;
		StartCoroutine ( iterateTurns() );
	}

	void Update () {
	}
	
	IEnumerator iterateTurns() {
		switch(currentTurn) {
		case Turns.playerOne:
			turnGUI.GetComponent<Text>().text = "Turn: Game 1";
			break;
		case Turns.playerTwo:
			turnGUI.GetComponent<Text>().text = "Turn: Game 2";
			break;
		case Turns.gameOne:
			//monster 1's move calc
			turnGUI.GetComponent<Text>().text = "Turn: Player 1";
			break;
		case Turns.gameTwo:
			//monster 2's move calc
			turnGUI.GetComponent<Text>().text = "Turn: Player 2";
			break;
		}
		
		yield return new WaitForSeconds(turnSpeed);
		
		switch(currentTurn) {
		case Turns.playerOne:
			turnGUI.GetComponent<Text>().text = "Turn: Game 1";
			currentTurn = Turns.gameOne;
			break;
		case Turns.playerTwo:
			turnGUI.GetComponent<Text>().text = "Turn: Game 2";
			currentTurn = Turns.gameTwo;
			break;
		case Turns.gameOne:
			//monster 1's move calc
			turnGUI.GetComponent<Text>().text = "Turn: Player 1";
			playerOne.GetComponent<Player>().fatigued = false;
			currentTurn = Turns.playerTwo;
			break;
		case Turns.gameTwo:
			//monster 2's move calc
			turnGUI.GetComponent<Text>().text = "Turn: Player 2";
			playerTwo.GetComponent<Player>().fatigued = false;
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
	}
	
	public void stopTurn(GameState.Turns nextTurn) {
		StopCoroutine( iterateTurns() );
		currentTurn = nextTurn;
		StartCoroutine( iterateTurns() );
	}
}
