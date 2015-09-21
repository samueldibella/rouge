using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Generation : MonoBehaviour
{
	// map tile types and their a* pathfinding cost
	// so, for example, if it wanted to move through a Wall, it'd have to be better than moving through 4 Floor tiles
	public enum Type
	{
		Floor = 1,
		Wall = 4,
		Stone = 20,
		Enemey = 50,
		Start = 100,
		End = 200,
		None = 0
	}
	
	// map data
	public static int Width = 40;
	public static int Height = 40;
	public static Type[,] Map;
	public int RoomCount;
	public int MinRoomSize;
	public int MaxRoomSize;
	public int Grid = 16;
	public int Padding = 4;
	public static Vector2 StartPostion; 
	public static Vector2 EndPostion; 

	public static Vector2 QuadPoint1; 
	public static Vector2 QuadPoint2; 
	public static Vector2 QuadPoint3; 
	public static Vector2 QuadPoint4; 
	
	public int maxEnemies = 5;

	// list of active rooms and the room xml templates
	public List<Rect> Rooms;
	//public List<XmlElement> RoomTemplates;
	
	// pathfinding cost thing
	private int[,] cost;
	private List<Vector2> currentPath;
	
	// coroutine
	private Coroutine coroutine;
	
	// Called when the generation is complete
	public delegate void Callback ();
	
	private Callback call;
	
	//		public Generation(List<XmlElement> templates)
	//		{
	//			RoomTemplates = templates;
	//		}
	
	#region map generation
	public void GenerateMap ()
	{
		Generate (Width, Height);
		genEnemies();
		genStartStop(); 
	}
	
	void Update ()
	{
		
	}
	//create spawn and exit
	public void genStartStop() {
		Rect firstRoom = Rooms.First();
		StartPostion = firstRoom.center;
		foreach(Rect next in Rooms) {
			Pathfind ((int)(StartPostion.x), (int)(StartPostion.y), (int)(next.center.x), (int)(next.center.y), false);
		}
		Map[(int)StartPostion.x, (int)StartPostion.y] = Type.Start;  
		Map[(int)currentPath.First().x, (int)currentPath.First().y] = Type.End;
		EndPostion = new Vector2(currentPath.First().x, currentPath.First().y); 
	}

	public void findQuadPoints() {


	}
	public void genEnemies() {
		for (int i = 0; i < maxEnemies; i++) {
			Vector2 rnd = GetRandomPosition();
			Map[(int)rnd.x, (int)rnd.y] = Type.Enemey;  
		}
	}
	
	/// <summary>
	/// Generates the room of the given width and height
	/// </summary>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <param name="callback">Called on generation-complete</param>
	public void Generate (int width, int height)
	{
		// create the empty map and an empty list of rooms
		Map = new Type[width, height];
		Rooms = new List<Rect> ();
		
		// fill with Wall tiles
		FillMap (Type.Wall);
		
		// set the min/max room size and the room count. Play around with these values
		MinRoomSize = 6;
		MaxRoomSize = (int)(Mathf.Min (width, height) / 4f);
		RoomCount = (int)(Mathf.Sqrt (width * height) / 2f);
		
		// Start the coroutine of generating the segments
		GenerateSegments ();
	}
	
	/// <summary>
	/// Generates everything
	/// </summary>
	private void GenerateSegments ()
	{
		// dig stuff
		PlaceRooms ();
		PlaceTunnels ();
	}
	
	/// <summary>
	/// Fills the entire map with the given type
	/// </summary>
	/// <param name="type"></param>
	public void FillMap (Type type)
	{
		for (int i = 0; i < Width; i++) {
			for (int j = 0; j < Height; j++)
				Map [i, j] = type;
		}
	}
	
	#endregion
	
	#region Room generation
	
	/// <summary>
	/// Places all the rooms we're supposed to place
	/// </summary>
	public void PlaceRooms ()
	{
		// place rooms
		int placed = 0;
		int count = 0;
		while (placed < RoomCount) {
			// choose to place a rectangle room, or a templated room
			//if (Calc.Random.Choose<int>(0, 0) == 0)
			if (0 == 0) {
				if (PlaceRectRoom ())
					placed++;
			}
			
			// this is for debug stuff - shouldn't ever happen
			count++;
			if (count > 1000)
				break;
		}
	}
	
	/// <summary>
	/// Digs out a rectangular room
	/// </summary>
	/// <returns></returns>
	public bool PlaceRectRoom ()
	{
		int width = Random.Range (MinRoomSize, MaxRoomSize);
		int height = Random.Range (MinRoomSize, MaxRoomSize);
		Rect room = new Rect (Random.Range (Padding, Width - width - Padding * 2), Random.Range (Padding, Height - height - Padding * 2), width, height);
		
		// check room
		if (Overlaps (room)) {
			Rooms.Add (room);
			DigRoom (room);
			return true;
		}
		return false;
	}
	

	public void DigRoom (Rect room)
	{
		// place floors
		for (int i = 0; i < room.width; i++) {
			for (int j = 0; j < room.height; j++) {
				Set ((int)room.x + i, (int)room.y + j, Type.Floor);
			}
		}
		
		// place stone around the entire thing
		for (int i = 0; i < room.width; i++) {
			Set ((int)(room.x + i), (int)(room.y - 1), Type.Stone);
			Set ((int)(room.x + i), (int)(room.y + room.height), Type.Stone);
		}
		
		for (int i = 0; i < room.height; i++) {
			Set ((int)(room.x - 1), (int)(room.y + i), Type.Stone);
			Set ((int)(room.x + room.width), (int)(room.y + i), Type.Stone);
		}
		
		// make some doors
		int doors = 2;
		for (int i = 0; i < doors; i++) {
			if (Random.Range (0, 2) == 0) {
				int wallChoice = -1; 
				if (Random.Range (0, 2) == 0) {
					wallChoice = (int)room.height;
				}
				Set ((int)(room.x + Random.Range (0, (int)room.width)), (int)(room.y + wallChoice), Type.Wall);
			} else {
				int wallChoice = -1; 
				if (Random.Range (0, 2) == 0) {
					wallChoice = (int)room.width;
				}
				Set ((int)(room.x + wallChoice), (int)(room.y + Random.Range (0, (int)room.height)), Type.Wall);
			}
		}
	}

	
	#endregion
	
	#region generate tunnels
	
	/// <summary>
	/// Places and digs out the tunnels from room-to-room
	/// </summary>
	public void PlaceTunnels ()
	{
		int count = 0;
		
		// pathfind tunnels
		Rect prev = Rooms [Rooms.Count - 1];
		foreach (Rect next in Rooms) {
			count++;
			
			// pathfind from the center of the previous room to the center of the next room
			Pathfind ((int)(prev.x + prev.width / 2), (int)(prev.y + prev.height / 2), (int)(next.x + next.width / 2), (int)(next.y + next.height / 2), true);
			
			// dig out the tunnel we just found
			int size = Random.Range (1, 3);
			foreach (Vector2 point in currentPath) {
				Set ((int)(point.x - size / 2), (int)(point.y - size / 2), size, size, Type.Floor, Type.Stone);
			}
			
			prev = next;
		}
	}
	
	/// <summary>
	/// Finds a path from the first poisition to the 2nd position and stores it in the currentPath variable
	/// NOTE: This is probably super horrible A* Pathfinding algorithm. I'm sure there's WAY better ways of writing this
	/// </summary>
	public void Pathfind (int x, int y, int x2, int y2, bool reverse)
	{
		cost = new int[Width, Height];
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
				cost [(int)point.x - 1, (int)point.y] = currentCost + (int)Map [(int)point.x - 1, (int)point.y];
			}
			if (point.x + 1 < Width && cost [(int)point.x + 1, (int)point.y] == 0) {
				active.Add (new Vector2 (point.x + 1, point.y));
				cost [(int)point.x + 1, (int)point.y] = currentCost + (int)Map [(int)point.x + 1, (int)point.y];
			}
			if (point.y - 1 >= 0 && cost [(int)point.x, (int)point.y - 1] == 0) {
				active.Add (new Vector2 (point.x, point.y - 1));
				cost [(int)point.x, (int)point.y - 1] = currentCost + (int)Map [(int)point.x, (int)point.y - 1];
			}
			if (point.y + 1 < Height && cost [(int)point.x, (int)point.y + 1] == 0) {
				active.Add (new Vector2 (point.x, point.y + 1));
				cost [(int)point.x, (int)point.y + 1] = currentCost + (int)Map [(int)point.x, (int)point.y + 1];
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
			if (current.x + 1 < Width && cost [(int)current.x + 1, (int)current.y] != 0) {
				right = cost [(int)current.x + 1, (int)current.y];
			}
			if (current.y - 1 >= 0 && cost [(int)current.x, (int)current.y - 1] != 0) {
				up = cost [(int)current.x, (int)current.y - 1];
			}
			if (current.y + 1 < Height && cost [(int)current.x, (int)current.y + 1] != 0) {
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
			points.Reverse ();
			currentPath = points;
		} else if(points.Count > currentPath.Count ) 
		{ 
			currentPath = points;
		}
	}
	
	#endregion
	
	#region map data - set/check
	
	/// <summary>
	/// Sets the given cell to the given type, if it's not already set to the untype
	/// </summary>
	public void Set (int x, int y, Type type)
	{
		if (x < 0 || y < 0 || x >= Width || y >= Height)
			return;
		else
			Map [x, y] = type;
	}
	
	/// <summary>
	/// Sets the given rectangle of cells to the type
	/// </summary>
	public void Set (int x, int y, int w, int h, Type type, Type untype = Type.None)
	{
		for (int i = x; i < x + w; i++) {
			for (int j = y; j < y + h; j++) {
				Set (i, j, type);
			}
		}
	}
	
	/// <summary>
	/// Makes sure the area doesn't overlap any floors
	/// </summary>
	/// <param name="area"></param>
	/// <returns></returns>
	public bool Overlaps (Rect area)
	{
		for (int i = 0; i < area.width; i++) {
			for (int j = 0; j < area.height; j++) {
				if (Map [(int)area.x + i, (int)area.y + j] != Type.Wall && Map [(int)area.x + i, (int)area.y + j] != Type.Stone)
					return false;
			}
		}
		return true;
	}
	
	/// <summary>
	/// Returns false if the position is outside the map
	/// </summary>
	/// <returns></returns>
	public bool CellOutside (int x, int y)
	{
		return x < 0 || y < 0 || x >= Width || y >= Height;
	}
	
	/// <summary>
	/// Gets the type of the given cell
	/// </summary>
	/// <returns></returns>
	public Type GetCell (int x, int y)
	{
		if (!CellOutside (x, y))
			return Map [x, y];
		else
			return Type.None;
	}
	
	#endregion
	
	/// <summary>
	/// Returns a 2D bool array used for checking collisions in entities
	/// </summary>
	/// <returns></returns>
	public bool[,] GetCollideData ()
	{
		bool[,] data = new bool[Width, Height];
		for (int i = 1; i < Width - 1; i++) {
			for (int j = 1; j < Height - 1; j++) {
				// this looks a bit weird, but it basically just only places "Edge" walls (it doesn't fill the insides)
				// so it just checks to make sure each tile is adjacent to a floor tile
				if (Map [i, j] != Type.Floor) {
					if (Map [i - 1, j] == Type.Floor || Map [i - 1, j - 1] == Type.Floor || Map [i, j - 1] == Type.Floor || Map [i + 1, j - 1] == Type.Floor
					    || Map [i + 1, j] == Type.Floor || Map [i + 1, j + 1] == Type.Floor || Map [i, j + 1] == Type.Floor || Map [i - 1, j + 1] == Type.Floor) {
						data [i, j] = true;
					}
				}
				
			}
		}
		return data;
	}
	
	/// <summary>
	/// Gets a random position in the map that is a Floor (not a wall)
	/// </summary>
	/// <returns></returns>
	public Vector2 GetRandomPosition ()
	{
		int room = Random.Range (0, Rooms.Count - 1);
		Vector2 point = Vector2.zero;
		while (point == Vector2.zero || Map[(int)point.x, (int)point.y] != Type.Floor) {
			point = new Vector2 (
				(int)Random.Range (Rooms [room].x, Rooms [room].x + Rooms [room].width),
				(int)Random.Range (Rooms [room].y, Rooms [room].y + Rooms [room].height)
				);
		}
		
		return point;
	}
}
