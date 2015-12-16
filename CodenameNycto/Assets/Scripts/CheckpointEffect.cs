using UnityEngine;
using System.Collections;

public class CheckpointEffect : MonoBehaviour {
	
	public ParticleSystem[] Particles;

	private AudioSource checkpointClip;
	private bool playedClip;

	// Use this for initialization
	void Start () {
		playedClip = false;
		checkpointClip = this.GetComponent<AudioSource>();

		if(PlayerPrefs.GetInt("Checkpoint") == 1)
		{
			Activate();
		}
		else
		{
			for(int i =0; i < Particles.Length; i++)
			{
				Particles[i].enableEmission = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Activate()
	{
		for(int i =0; i < Particles.Length; i++)
		{
			Particles[i].enableEmission = true;
		}

		if(!playedClip)
		{
			checkpointClip.Play();
			playedClip = true;
		}

	}
}
