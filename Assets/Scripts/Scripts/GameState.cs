using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject tileTestPrefab;
	public GameObject tileTestWallPrefab;
	public GameObject enemyPrefab;
	public GameObject playerPrefab;
	public GameObject playerOne;
	public GameObject playerTwo;
	public GameObject[,] tiles;
	public GameObject turnGUI;
	public int mapHeight;
	public int mapWidth;
	public int lightRange;
	public int lightIntensity;
	int lightDeviation;
	int dx;
	int playerY, playerX;

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
		lightRange = 10;
		lightIntensity = 5;

		turnLength = 1.0f;
		timeTurner = 0f;

		originTile = Vector3.zero;


		//generate map
		this.GetComponent<Generation>().GenerateMap();  
		tiles = new GameObject[Generation.Width, Generation.Height];
		//print map
		for(int j = 0; j < Generation.Height; j++) {
			for(int i = 0; i < Generation.Width; i++) {
				if ((int)Generation.Map [j, i] == 1) { 
					tiles[j, i] = Instantiate(tileTestPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
				} else if ((int)Generation.Map[j, i] == 4) { 
					tiles[j, i] = Instantiate(tileTestWallPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
				} else if ((int)Generation.Map [j, i] == 20) { 
					tiles[j, i] = Instantiate(tileTestWallPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
				} else if ((int)Generation.Map [j, i] == 50)  { //enemy
					//player 1 start
					tiles[j, i] = Instantiate(tileTestPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					Instantiate(enemyPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity);
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j; 
					tiles[j, i].GetComponent<TileStat>().occupied = true; 
				} else if ((int)Generation.Map [j, i] == 100)  { //player 1 start
					//player 1 start
					tiles[j, i] = Instantiate(tileTestPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					playerOneStart = tiles[j, i];
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j; 
				} else if ((int)Generation.Map [j, i] == 200) { 
					tiles[j, i] = Instantiate(tileTestPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					playerTwoStart = tiles[j, i];
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
				}
			}
		}

		//tell player where they start
		playerOne = Instantiate(playerPrefab, new Vector3(playerOneStart.transform.position.x, playerOneStart.transform.position.y, 0), Quaternion.identity) as GameObject;
		playerOne.GetComponent<Player>().setPlayerNum(1);
		playerOne.GetComponent<Animus>().setCoords(playerOneStart.GetComponent<TileStat>().x, playerOneStart.GetComponent<TileStat>().y);
		playerOne.GetComponent<Animus>().location = playerOneStart;
		playerOneStart.GetComponent<TileStat>().occupied = true;
		playerOne.transform.parent = this.transform;

		//player2
		playerTwo = Instantiate(playerPrefab, new Vector3(playerTwoStart.transform.position.x, playerTwoStart.transform.position.y, 0), Quaternion.identity) as GameObject;
		playerTwo.GetComponent<Player>().setPlayerNum(2);
		playerTwo.GetComponent<Animus>().setCoords(playerTwoStart.GetComponent<TileStat>().x, playerTwoStart.GetComponent<TileStat>().y);
		playerTwo.GetComponent<Animus>().location = playerTwoStart;
		playerTwoStart.GetComponent<TileStat>().occupied = true;
		playerTwo.transform.parent = this.transform;
	}

	void Start () {
		currentTurn = Turns.playerOne;
		turnGUI.GetComponent<Text>().text = "p1";
		playerOne.GetComponent<Player>().fatigued = false;
		timeTurner = turnLength;
	}

	void Update () {
		timeTurner = timeTurner - Time.deltaTime;
		if (timeTurner <= 0) {
			iterateTurns();
			timeTurner = turnLength;
		}

		//call on tiles within range of players; top
		for(int i = lightRange; i >= 0; i --) {
			for(dx = -1 * (Mathf.Abs(i - lightRange)); dx < Mathf.Abs(i - lightRange); dx++ ) {

				if(isInGrid(playerOne.GetComponent<Animus>().y - i, playerOne.GetComponent<Animus>().x + dx)) {
					tiles[playerOne.GetComponent<Animus>().y - i, playerOne.GetComponent<Animus>().x + dx].GetComponent<TileStat>().updateLight(i, dx);

				}

				if(isInGrid(playerTwo.GetComponent<Animus>().y - i, playerTwo.GetComponent<Animus>().x + dx)) {
					tiles[playerTwo.GetComponent<Animus>().y - i, playerTwo.GetComponent<Animus>().x + dx].GetComponent<TileStat>().updateLight(i, dx);
				}
			}
		}

		//bottom
		for(int i = 0 - lightRange; i < 0; i++) {
			for(dx = -1 * (Mathf.Abs(i + lightRange)); dx < Mathf.Abs(i + lightRange); dx++ ) {
				if(isInGrid(playerOne.GetComponent<Animus>().y - i, playerOne.GetComponent<Animus>().x + dx)) {
					tiles[playerOne.GetComponent<Animus>().y - i, playerOne.GetComponent<Animus>().x + dx].GetComponent<TileStat>().updateLight(i, dx);

				}

				if(isInGrid(playerTwo.GetComponent<Animus>().y - i, playerTwo.GetComponent<Animus>().x + dx)) {
					tiles[playerTwo.GetComponent<Animus>().y - i, playerTwo.GetComponent<Animus>().x + dx].GetComponent<TileStat>().updateLight(i, dx);
				}
			}
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
		piece.GetComponent<Animus>().setCoords(goalTile.GetComponent<TileStat>().x, goalTile.GetComponent<TileStat>().y);
		goalTile.GetComponent<TileStat>().occupied = true;

		stopTurn();
	}

	public void stopTurn() {
		timeTurner = turnLength;
		iterateTurns();
	}

	public int tileDistance(GameObject startTile, GameObject targetTile) {

		return (int) Mathf.Sqrt( (float) (Mathf.Pow((targetTile.GetComponent<TileStat>().x - startTile.GetComponent<TileStat>().x), 2) + Mathf.Sqrt(Mathf.Pow((targetTile.GetComponent<TileStat>().y - startTile.GetComponent<TileStat>().y), 2f))));
	}

	bool isInGrid(int y, int x) {
	if(y > 0 && y < mapHeight && x > 0 && x < mapWidth) {
		return true;
	} else {
		return false;
	}
}
}
