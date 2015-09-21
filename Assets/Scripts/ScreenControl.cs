using UnityEngine;
using System.Collections;

public class ScreenControl : MonoBehaviour {

	public Texture2D end1;
	public Texture2D end2;
	public Texture2D end3;
	public Texture2D end4;
	public Texture2D tile;

	public IEnumerator LoseScreen() {
		this.transform.GetComponent<Renderer>().enabled = true;
		this.transform.GetComponent<Renderer>().material.SetTexture("_MainTex", end1);
		yield return new WaitForSeconds(.1f);
		this.transform.GetComponent<Renderer>().material.SetTexture("_MainTex", end2);
		yield return new WaitForSeconds(.1f);
		this.transform.GetComponent<Renderer>().material.SetTexture("_MainTex", end3);
		yield return new WaitForSeconds(.1f);
		this.transform.GetComponent<Renderer>().material.SetTexture("_MainTex", end4);
	}
}
