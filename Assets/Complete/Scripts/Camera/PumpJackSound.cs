using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PumpJackSound : MonoBehaviour {

	public AudioClip m_pumpJackSound;
	public AudioClip m_partyMusic;
	public float volume;
	// Use this for initialization
	
	void Start () {
		//AudioSource audio = GetComponent<AudioSource>();
		//audio.dopplerLevel = 0;
		InvokeRepeating("PlayPumpJackSound", 0, 28);
		InvokeRepeating("playHousePartyMusic", 0, 48);

	}

	// Update is called once per frame
	void Update () {
	}

	void PlayPumpJackSound(){
		AudioSource.PlayClipAtPoint(m_pumpJackSound, GameObject.FindGameObjectWithTag("pump1").transform.position, volume);
	}

	void playHousePartyMusic () {
		AudioSource.PlayClipAtPoint(m_partyMusic, GameObject.FindGameObjectWithTag("partybuilding").transform.position, volume);
	}
}