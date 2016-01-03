using UnityEngine;
using System.Collections;

public class PumpJackSound : MonoBehaviour {

	public AudioClip m_pumpJackSound;
	// Use this for initialization
	void Start () {
		InvokeRepeating("PlayPumpJackSound", 0, 28);
	}

	// Update is called once per frame
	void Update () {
	}

	void PlayPumpJackSound(){
		AudioSource.PlayClipAtPoint(m_pumpJackSound, GameObject.FindGameObjectWithTag("pump1").transform.position, 10.0F);

	}
}