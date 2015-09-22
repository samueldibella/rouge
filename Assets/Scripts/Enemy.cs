using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
  

	GameObject manager;
	int rndNum;
	public float maxRange = 5;
	GameObject [] players; 
	bool running = false; 
	public IEnumerator routine;
	float turnLength;

	void Start () {
		manager = transform.parent.gameObject;
		routine = RndMove();
		players = GameObject.FindGameObjectsWithTag("Respawn");
		turnLength = transform.parent.GetComponent<GameState>().turnLength; 
		StartCoroutine(routine); 
		StartCoroutine("watch");
	}

	IEnumerator watch() {
		while(true) {
		RaycastHit hit;
		if(running == false) {
			foreach(GameObject next in players)  {
				if (next != null) {
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
			yield return new WaitForSeconds(turnLength); 
		}
	}

	IEnumerator RndMove() { 
		while(true) {
			bool moved = false; 
			while(moved == false) {
				rndNum = Random.Range(0, 4);
				if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('n').GetComponent<TileStat>().occupied == false) {
					GetComponent<Animus>().dir = 'n'; 
					moved = true;
				} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('e').GetComponent<TileStat>().occupied == false) {
					GetComponent<Animus>().dir = 'e'; 
					moved = true;
				} else if(rndNum == 2 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('w').GetComponent<TileStat>().occupied == false) {
					GetComponent<Animus>().dir = 'w'; 
					moved = true;
				} else if(rndNum == 3 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('s').GetComponent<TileStat>().occupied == false) {
					GetComponent<Animus>().dir = 's'; 
					moved = true;
				} 
			}
			yield return new WaitForSeconds(turnLength); 
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
						GetComponent<Animus>().dir = 's'; 
						moved = true;
					} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('e').GetComponent<TileStat>().occupied == false) {
						GetComponent<Animus>().dir = 'e';
						moved = true;
					} else {
						GetComponent<Animus>().dir = 'e'; 
						moved = true;
					}
				}
			} else if(player_position.x > current_position.x && player_position.y >= current_position.y ){ //north east
				while(moved == false) {
					rndNum = Random.Range(0, 2);
					if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('s').GetComponent<TileStat>().occupied == false) {
						GetComponent<Animus>().dir = 's'; 
						moved = true;
					} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('w').GetComponent<TileStat>().occupied == false) {
						GetComponent<Animus>().dir = 'w'; 
						moved = true;
					} else {
						GetComponent<Animus>().dir = 's'; 
						moved = true;
					}
				}
			} else if(player_position.x <= current_position.x && player_position.y < current_position.y ){ //south west 
				while(moved == false) {
					rndNum = Random.Range(0, 2);
					if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('n').GetComponent<TileStat>().occupied == false) {
						GetComponent<Animus>().dir = 'n'; 
						moved = true;
					} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('e').GetComponent<TileStat>().occupied == false) {
						GetComponent<Animus>().dir = 'e'; 
						moved = true;
					} else {
						GetComponent<Animus>().dir = 'n'; 
						moved = true;
					}
				}
			} else if(player_position.x > current_position.x && player_position.y < current_position.y ){ //south east
				while(moved == false) {
					rndNum = Random.Range(0, 2);
					if(rndNum == 0 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('n').GetComponent<TileStat>().occupied == false) {
						GetComponent<Animus>().dir = 'n'; 
						moved = true;
					} else if(rndNum == 1 && GetComponent<Animus>().location.GetComponent<TileStat>().getNeighbor('w').GetComponent<TileStat>().occupied == false) {
						GetComponent<Animus>().dir = 'w'; 
						moved = true;
					} else {
						GetComponent<Animus>().dir = 'w'; 
						moved = true;
					}
				}
			}
			turnsRunning++;
			yield return new WaitForSeconds (turnLength);
		}
		StartCoroutine("RndMove");
		StartCoroutine("watch");
		running = false; 
	}
}
