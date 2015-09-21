using UnityEngine;
using System.Collections;

public class DominoSound : MonoBehaviour {

	AudioSource audio;
	public AudioClip clip1, clip2, clip3, clip4, clip5;
	AudioClip chosenClip;

	void Start() {
		audio = GetComponent<AudioSource>();
	}

	public void moveNoise() {

		audio.clip = clip1;
		audio.Play();
	}

	public void captureNoise() {
		int num = Random.Range(1,5);
		switch(num) {
			case 1:
				chosenClip = clip2;
				break;
			case 2:
				chosenClip = clip3;
				break;
			case 3:
				chosenClip = clip4;
				break;
			case 4:
				chosenClip = clip5;
				break;
		}

		audio.clip = chosenClip;
		audio.Play();
	}

}
