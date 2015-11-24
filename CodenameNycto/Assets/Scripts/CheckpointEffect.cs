using UnityEngine;
using System.Collections;

public class CheckpointEffect : MonoBehaviour {
	
	public ParticleSystem[] Particles;

	// Use this for initialization
	void Start () {
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
	}
}
