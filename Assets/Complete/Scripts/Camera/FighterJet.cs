using UnityEngine;
using System.Collections;

public class FighterJet : MonoBehaviour {
	public AudioClip jetSound;
	public float minTime;
	public float maxTime;
	public float radius;	
	private float currentTime;
	private float nextTime;
	private Vector3 position;
	public int Volume;

	// Use this for initialization
	void Start () {
		nextTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		currentTime = Time.time;
		if ( currentTime >= nextTime ) {
		
			// Sets the position to be somewhere inside a sphere
			// with a given radius and the center at zero.
			position = Random.insideUnitSphere * radius;
			AudioSource.PlayClipAtPoint(jetSound, position, Volume);
			
			// The timeInterval to the next sound event is choosen randomly 
			// between minTime and maxTime.
			float timeInterval = Random.Range(minTime, maxTime);
			nextTime = nextTime + timeInterval;
		}
	}
}