using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AI_Pathfinding : MonoBehaviour {

	//map tiles
	public enum Type
	{
		Floor = 1,
		Wall = 4,
		Stone = 20,
		Enemy = 50, 
		Start = 100,
		End = 200,
		None = 0
	}
	// pathfinding cost thing
	private int[,] cost;
	private List<Vector2> currentPath;
	Vector2 current_position; 

	// Use this for initialization
	void Start () {
		StartCoroutine("Move");
		current_position = new Vector2(Generation.StartPostion.x, Generation.StartPostion.y);
	}
	
	// Update is called once per framels

	void Update () {

	}

	IEnumerator Move() {
		while(true) {
			Pathfind ((int)Generation.StartPostion.x, (int)Generation.StartPostion.y, (int)Generation.EndPostion.x, (int)Generation.EndPostion.y, true);
			transform.position = new Vector3(currentPath.ElementAt(1).x, 1.5f, currentPath.ElementAt(1).y);
			yield return new WaitForSeconds (.1f);
		}
	}

	public void Pathfind (int x, int y, int x2, int y2, bool reverse)
	{
		cost = new int[Generation.Width, Generation.Height];
		cost [x, y] = (int)Type.Floor;

		List<Vector2> active = new List<Vector2> ();
		active.Add (new Vector2 (x, y));
		// pathfind
		while (true) {
			// get lowest cost point in active list
			Vector2 point = active [0];
			for (int i = 1; i < active.Count; i ++) {
				Vector2 p = active [i];
				if (cost [(int)p.x, (int)p.y] < cost [(int)point.x, (int)point.y])
					point = p;
			}
			
			// if end point
			if (point.x == x2 && point.y == y2)
				break;
			
			// move in directions
			int currentCost = cost [(int)point.x, (int)point.y];
			if (point.x - 1 >= 0 && cost [(int)point.x - 1, (int)point.y] == 0) {
				active.Add (new Vector2 (point.x - 1, point.y));
				cost [(int)point.x - 1, (int)point.y] = currentCost + (int)Generation.Map [(int)point.x - 1, (int)point.y];
			}
			if (point.x + 1 < Generation.Width && cost [(int)point.x + 1, (int)point.y] == 0) {
				active.Add (new Vector2 (point.x + 1, point.y));
				cost [(int)point.x + 1, (int)point.y] = currentCost + (int)Generation.Map [(int)point.x + 1, (int)point.y];
			}
			if (point.y - 1 >= 0 && cost [(int)point.x, (int)point.y - 1] == 0) {
				active.Add (new Vector2 (point.x, point.y - 1));
				cost [(int)point.x, (int)point.y - 1] = currentCost + (int)Generation.Map [(int)point.x, (int)point.y - 1];
			}
			if (point.y + 1 < Generation.Height && cost [(int)point.x, (int)point.y + 1] == 0) {
				active.Add (new Vector2 (point.x, point.y + 1));
				cost [(int)point.x, (int)point.y + 1] = currentCost + (int)Generation.Map [(int)point.x, (int)point.y + 1];
			}
			
			active.Remove (point);
		}
		
		// work backwards and find path
		List<Vector2> points = new List<Vector2> ();
		Vector2 current = new Vector2 (x2, y2);
		points.Add (current);
		
		while (current.x != x || current.y != y) {
			int highest = cost [(int)current.x, (int)current.y];
			int left = highest, right = highest, up = highest, down = highest;
			
			// get cost of each direction
			if (current.x - 1 >= 0 && cost [(int)current.x - 1, (int)current.y] != 0) {
				left = cost [(int)current.x - 1, (int)current.y];
			}
			if (current.x + 1 < Generation.Width && cost [(int)current.x + 1, (int)current.y] != 0) {
				right = cost [(int)current.x + 1, (int)current.y];
			}
			if (current.y - 1 >= 0 && cost [(int)current.x, (int)current.y - 1] != 0) {
				up = cost [(int)current.x, (int)current.y - 1];
			}
			if (current.y + 1 < Generation.Height && cost [(int)current.x, (int)current.y + 1] != 0) {
				down = cost [(int)current.x, (int)current.y + 1];
			}
			
			// move in the lowest direction
			if (left <= Mathf.Min (up, down, right)) {
				points.Add (current = new Vector2 (current.x - 1, current.y));
			} else if (right <= Mathf.Min (left, down, up)) {
				points.Add (current = new Vector2 (current.x + 1, current.y));
			} else if (up <= Mathf.Min (left, right, down)) {
				points.Add (current = new Vector2 (current.x, current.y - 1));
			} else {
				points.Add (current = new Vector2 (current.x, current.y + 1));
			}
		}
		if(reverse == true) 
		{
			print ("test");
			points.Reverse ();
			currentPath = points;
		} else if(points.Count > currentPath.Count ) 
		{ 
			currentPath = points;
		}
	}
}
