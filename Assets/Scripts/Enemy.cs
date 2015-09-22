using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
  

	GameObject manager;
	int rndNum;
	public float maxRange = 100;
	GameObject [] players; 
	bool running = false; 
	public IEnumerator routine;
	int i; 

	void Start () {
		manager = transform.parent.gameObject;
		routine = RndMove();
		players = GameObject.FindGameObjectsWithTag("Respawn");
		StartCoroutine(routine); 
	}

	void Update () {
		RaycastHit hit;
		if(running == false) {
			foreach(GameObject next in players)  {
				if(Physics.Raycast(transform.position, (next.transform.position - transform.position), out hit, maxRange))
				{
					if(hit.transform == next.transform && running == false)
					{
						StopCoroutine(routine);
						StartCoroutine(SeenMove(next));
						running = true; 
						yield break; 
					}
				}
			}
		}
	}

	IEnumerator RndMove() { 
		while(true) {
			bool moved = false; 
			while(moved == false) {
				rndNum = Random.Range(0, 4);
				if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('n').GetComponent<TileStat>().occupied == false) {
					manager.GetComponent<GameState>().move(this.gameObject, 'n');
					moved = true;
				} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('e').GetComponent<TileStat>().occupied == false) {
					manager.GetComponent<GameState>().move(this.gameObject, 'e');
					moved = true;
				} else if(rndNum == 2 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('w').GetComponent<TileStat>().occupied == false) {
					manager.GetComponent<GameState>().move(this.gameObject, 'w');
					moved = true;
				} else if(rndNum == 3 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('s').GetComponent<TileStat>().occupied == false) {
					manager.GetComponent<GameState>().move(this.gameObject, 's');
					moved = true;
				} 
			}
			yield return new WaitForSeconds(.5f); 
		}
	}

	IEnumerator SeenMove(GameObject player) { 
		int turnsRunning = 0; 
		while(turnsRunning < 20) {
			bool moved = false;
			int rndNum = -1; 
			Vector2 player_position = new Vector2(player.transform.parent.GetComponent<Animus>().x, player.transform.parent.GetComponent<Animus>().y); 
			Vector2 current_position = new Vector2(GetComponent<Animus>().x, GetComponent<Animus>().y); 

			if(player_position.x <= current_position.x && player_position.y >= current_position.y ){ //north west 
				while(moved == false) {
					rndNum = Random.Range(0, 2);
					if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('s').GetComponent<TileStat>().occupied == false) {
						manager.GetComponent<GameState>().move(this.gameObject, 's');
						moved = true;
					} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('e').GetComponent<TileStat>().occupied == false) {
						manager.GetComponent<GameState>().move(this.gameObject, 'e');
						moved = true;
					} else {
						manager.GetComponent<GameState>().move(this.gameObject, 'e');
						moved = true;
					}
				}
			} else if(player_position.x > current_position.x && player_position.y >= current_position.y ){ //north east
				while(moved == false) {
					rndNum = Random.Range(0, 2);
					if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('s').GetComponent<TileStat>().occupied == false) {
						manager.GetComponent<GameState>().move(this.gameObject, 's');
						moved = true;
					} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('w').GetComponent<TileStat>().occupied == false) {
						manager.GetComponent<GameState>().move(this.gameObject, 'w');
						moved = true;
					} else {
						manager.GetComponent<GameState>().move(this.gameObject, 's');
						moved = true;
					}
				}
			} else if(player_position.x <= current_position.x && player_position.y < current_position.y ){ //south west 
				while(moved == false) {
					rndNum = Random.Range(0, 2);
					if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('n').GetComponent<TileStat>().occupied == false) {
						manager.GetComponent<GameState>().move(this.gameObject, 'n');
						moved = true;
					} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('e').GetComponent<TileStat>().occupied == false) {
						manager.GetComponent<GameState>().move(this.gameObject, 'e');
						moved = true;
					} else {
						manager.GetComponent<GameState>().move(this.gameObject, 'n');
						moved = true;
					}
				}
			} else if(player_position.x > current_position.x && player_position.y < current_position.y ){ //south east
				while(moved == false) {
					rndNum = Random.Range(0, 2);
					if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('n').GetComponent<TileStat>().occupied == false) {
						manager.GetComponent<GameState>().move(this.gameObject, 'n');
						moved = true;
					} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('w').GetComponent<TileStat>().occupied == false) {
						manager.GetComponent<GameState>().move(this.gameObject, 'w');
						moved = true;
					} else {
						manager.GetComponent<GameState>().move(this.gameObject, 'w');
						moved = true;
					}
				}
			}
			turnsRunning++;
			yield return new WaitForSeconds (.5f);
		}
		print ("It was nothing...");
		StartCoroutine("RndMove");
		running = false; 
	}
}
