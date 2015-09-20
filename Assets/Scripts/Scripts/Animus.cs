using UnityEngine;
using System.Collections;

public class Animus : MonoBehaviour {
  	public GameObject location;
    public int x;
    public int y;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

  public void setCoords(int newX, int newY) {
    x = newX;
    y = newY;
  }
}
