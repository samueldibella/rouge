using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject tileTestWallPrefab;
	public GameObject enemyPrefab;
	public GameObject playerPrefab;
	public GameObject screen;
	public GameObject playerOne;
	public GameObject playerTwo;
	public GameObject[,] tiles;
	GameObject[] enemies;
	public GameObject turnGUI;
	public int mapHeight;
	public int mapWidth;
	public int lightRange;
	public int lightIntensity;
	int lightDeviation;
	int playerY, playerX;
	int dx, dy;

	public static int highScore = 0;
	public int eatenEnemies = 0;

	//game state vars
	public float turnLength;
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
	bool notAllowed = false;

	Vector3 originTile;

	// Use this for initialization
	void Awake () {
		mapHeight = 40;
		mapWidth = 40;
		tileScale = 1;
		lightRange = 8;
		lightIntensity = 5;
		turnLength = .1f;
		originTile = Vector3.zero;


		//generate map
		this.GetComponent<Generation>().GenerateMap();
		tiles = new GameObject[Generation.Width, Generation.Height];
		//print map
		for(int j = 0; j < Generation.Height; j++) {
			for(int i = 0; i < Generation.Width; i++) {
				if ((int)Generation.Map [j, i] == 1) {
					tiles[j, i] = Instantiate(tilePrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
				} else if ((int)Generation.Map[j, i] == 4) {
					tiles[j, i] = Instantiate(tileTestWallPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					tiles[j, i].GetComponent<TileStat>().occupant = tiles[j,i];
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
				} else if ((int)Generation.Map [j, i] == 20) {
					tiles[j, i] = Instantiate(tileTestWallPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					tiles[j, i].GetComponent<TileStat>().occupant = tiles[j,i];
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
				} else if ((int)Generation.Map [j, i] == 50)  { //enemy
					//Enemy start
					tiles[j, i] = Instantiate(tilePrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					tiles[j, i].GetComponent<TileStat>().occupant = Instantiate(enemyPrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					tiles[j, i].GetComponent<TileStat>().occupant.transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().occupant.GetComponent<Animus>().x = i;
					tiles[j, i].GetComponent<TileStat>().occupant.GetComponent<Animus>().y = j;
					tiles[j, i].GetComponent<TileStat>().occupant.GetComponent<Animus>().location = tiles[j,i];
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
					tiles[j, i].GetComponent<TileStat>().occupied = true;
				} else if ((int)Generation.Map [j, i] == 100)  { //player 1 start
					//player 1 start
					tiles[j, i] = Instantiate(tilePrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
					playerOneStart = tiles[j, i];
					tiles[j, i].transform.parent = this.transform;
					tiles[j, i].GetComponent<TileStat>().x = i;
					tiles[j, i].GetComponent<TileStat>().y = j;
				} else if ((int)Generation.Map [j, i] == 200) { //player 2 start
					tiles[j, i] = Instantiate(tilePrefab, new Vector3(originTile.x + (i * tileScale), originTile.y + (j * tileScale), 0), Quaternion.identity) as GameObject;
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
		playerOneStart.GetComponent<TileStat>().occupant = playerOne;
		playerOne.transform.parent = this.transform;
		playerOne.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color = Color.black;

		//player2
		playerTwo = Instantiate(playerPrefab, new Vector3(playerTwoStart.transform.position.x, playerTwoStart.transform.position.y, 0), Quaternion.identity) as GameObject;
		playerTwo.GetComponent<Player>().setPlayerNum(2);
		playerTwo.GetComponent<Animus>().setCoords(playerTwoStart.GetComponent<TileStat>().x, playerTwoStart.GetComponent<TileStat>().y);
		playerTwo.GetComponent<Animus>().location = playerTwoStart;
		playerTwoStart.GetComponent<TileStat>().occupied = true;
		playerTwoStart.GetComponent<TileStat>().occupant = playerTwo;
		playerTwo.transform.parent = this.transform;
	}

	void Start () {
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		StartCoroutine( Turns() );
	}

	void Update () {
		if(eatenEnemies == Generation.maxEnemies) {
			Generation.level++;
			Generation.maxEnemies = Generation.maxEnemies + (Generation.level/2);
			Application.LoadLevel(Application.loadedLevel);
		}
		for ( int y = playerOne.GetComponent<Animus>().y - lightRange; y < playerOne.GetComponent<Animus>().y + lightRange; y++) {
			dy = y -  playerOne.GetComponent<Animus>().y;
			dx = (int) Mathf.Sqrt(lightRange * lightRange - dy * dy );

			for( int x = playerOne.GetComponent<Animus>().x - dx; x < playerOne.GetComponent<Animus>().x + dx; x++) {
				if(isInGrid(x,y)) {
					tiles[y,x].GetComponent<TileStat>().updateLight(Mathf.Abs(x), Mathf.Abs(y));
					tiles[y,x].GetComponent<TileStat>().lightUpdated = true;
				}
			}
		}

		for ( int y = playerTwo.GetComponent<Animus>().y - lightRange; y < playerTwo.GetComponent<Animus>().y + lightRange; y++) {
			dy = y -  playerTwo.GetComponent<Animus>().y;
			dx = (int) Mathf.Sqrt(lightRange * lightRange - dy * dy );

			for( int x = playerTwo.GetComponent<Animus>().x - dx; x < playerTwo.GetComponent<Animus>().x + dx; x++) {
				if(isInGrid(x,y) && !(y == 0 && (x == playerTwo.GetComponent<Animus>().x - dx || x ==  playerTwo.GetComponent<Animus>().x + dx))) {
					tiles[y,x].GetComponent<TileStat>().updateLight(Mathf.Abs(x), Mathf.Abs(y));
					tiles[y,x].GetComponent<TileStat>().lightUpdated = true;
				}
			}
		}

		for( int y = 0; y < mapHeight; y++) {
			for ( int x = 0; x < mapWidth; x++) {
				if(!tiles[y,x].GetComponent<TileStat>().lightUpdated) {
						tiles[y,x].GetComponent<TileStat>().deprecateLight();
				}
			}
		}

		for( int y = 0; y < mapHeight; y++) {
			for ( int x = 0; x < mapWidth; x++) {
				tiles[y,x].GetComponent<TileStat>().lightUpdated = false;
			}
		}
	}

	public void move(GameObject piece, char dir) {
		Transform segmentTraversal;
		GameObject originalTile = piece.GetComponent<Animus>().location;
		GameObject goalTile = originalTile;
		int goalX = goalTile.GetComponent<TileStat>().x;
		int goalY = goalTile.GetComponent<TileStat>().y;

		switch(dir) {
		case 'n':
			if(goalY < mapHeight - 1) {
				goalTile = tiles[goalY + 1, goalX];
				piece.GetComponent<Animus>().lastDir = dir; //for no reverse movement

			}
			break;
		case 'w':
			if(goalX > 0) {
				goalTile = tiles[goalY, goalX - 1];
				piece.GetComponent<Animus>().lastDir = dir;
			}
			break;
		case 'e':
			if(goalX < mapWidth - 1) {
				goalTile = tiles[goalY, goalX + 1];
				piece.GetComponent<Animus>().lastDir = dir;
			}
			break;
		case 's':
			if(goalY > 0) {
				goalTile = tiles[goalY - 1, goalX];
				piece.GetComponent<Animus>().lastDir = dir;
			}
			break;
		default:
			break;
		}

		if(goalTile.GetComponent<TileStat>().occupied) {
			if (piece.gameObject.tag == "Player" && goalTile.GetComponent<TileStat>().occupant.gameObject.tag == "Enemy") {
				piece.GetComponent<Player>().grow(piece.GetComponent<Animus>().location);
				Destroy(goalTile.GetComponent<TileStat>().occupant);
				shiftPiece(piece, goalTile);
				eatenEnemies++;
				Generation.score++;
			}else {
				if (piece.gameObject.tag == "Player" && piece.GetComponent<Animus>().moved) {
					StartCoroutine( LoseAnimation(piece) );
				}
			}
		} else {
			shiftPiece(piece, goalTile);
		}
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

	//not used for turns, just raw movement
	void shiftPiece(GameObject piece, GameObject newTile) {
		GameObject originalTile = piece.GetComponent<Animus>().location;

		piece.GetComponent<Animus>().location.GetComponent<TileStat>().occupied = false;
		piece.GetComponent<Animus>().location.GetComponent<TileStat>().occupant = null;
		piece.GetComponent<Animus>().location = newTile;
		piece.transform.position = newTile.transform.position;

		newTile.GetComponent<TileStat>().occupied = true;
		newTile.GetComponent<TileStat>().occupant = piece;
		piece.GetComponent<Animus>().setCoords(newTile.GetComponent<TileStat>().x, newTile.GetComponent<TileStat>().y);

		if(piece.gameObject.tag == "Player") {
			if(piece.GetComponent<Player>().length > 1) {
				foreach (Transform child in piece.transform) {
					if(child.gameObject.tag == "Segment") {
						shiftPiece(child.gameObject, originalTile);
					}
				}
			}

			StartCoroutine ( piece.GetComponent<Animus>().movementAnimation() );
		}

		if (piece.gameObject.tag == "Segment") {
			if(piece.gameObject.transform.childCount > 1) {
				shiftPiece(piece.gameObject.transform.GetChild(1).gameObject, originalTile);
			}
		}

		piece.GetComponent<Animus>().moved = true;
	}

	IEnumerator Turns() {
		while(!notAllowed) {
			//calc enemies move
			if(enemies!= null) {
				foreach (GameObject enemy in enemies) {
					if(enemy != null) {
						move(enemy, enemy.GetComponent<Animus>().dir);
					}
				}
			}

			yield return new WaitForSeconds(turnLength);

			move(playerOne, playerOne.GetComponent<Animus>().dir);
			move(playerTwo, playerTwo.GetComponent<Animus>().dir);
			Camera.main.transform.GetChild(0).GetComponent<DominoSound>().moveNoise();

			yield return new WaitForSeconds(turnLength);
		}
	}

	IEnumerator LoseAnimation(GameObject player) {
		Camera.main.transform.GetChild(0).GetComponent<DominoSound>().deathNoise();
		StopCoroutine( "Turns" );

		notAllowed = true;
		highScore = Generation.score;

		player.GetComponent<Animus>().location.GetComponent<Renderer>().material = player.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel("Ending");
	}
}
