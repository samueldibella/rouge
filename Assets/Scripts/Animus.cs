using UnityEngine;
using System.Collections;

public class Animus : MonoBehaviour {
	public GameObject location;
	public char dir;
	public char lastDir;
	public int x;
	public int y;
	public bool moved = false;
	
	// Use this for initialization
	void Start () {
		dir = ' ';
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void setCoords(int newX, int newY) {
		x = newX;
		y = newY;
	}
	
	public IEnumerator movementAnimation() {
		Vector3 original = new Vector3( this.transform.GetChild(0).transform.localScale.x, this.transform.GetChild(0).transform.localScale.y, this.transform.GetChild(0).transform.localScale.z
		                               );
		int time = 0;
		float scaleAlt = .3f;
		//float smallScale = scaleAlt/10;
		this.transform.GetChild(0).transform.localScale += new Vector3(scaleAlt, scaleAlt, 0);
		
		while(true) {
			this.transform.GetChild(0).transform.localScale = Vector3.Lerp(this.transform.GetChild(0).transform.localScale, original, .2f);
			
			if(time > 3) {
				this.transform.GetChild(0).transform.localScale = new Vector3(2, 2, this.transform.GetChild(0).transform.localScale.z);
				yield break;
			}
			
			time++;
			yield return 0;
		}
	}
}