using UnityEngine;
using System.Collections;

public class LampCollision : MonoBehaviour {

	public Light lightSource;
	public int startLit;
	public int becomeLit;
	public bool NotLit;
	public ParticleSystem[] Particles;

	// Use this for initialization
	void Start () {
		if(NotLit)
		{
			for(int i =0; i < Particles.Length; i++)
			{
				Particles[i].enableEmission = false;
			}
			lightSource.intensity = 0;
		}
		else
		{
			lightSource.intensity = startLit;
			for(int i = 0; i < Particles.Length; i++)
			{
				Particles[i].enableEmission = true;
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void lightUp()
	{
		lightSource.intensity = becomeLit;
		NotLit = false;
		for(int i = 0; i < Particles.Length; i++)
		{
			Particles[i].enableEmission = true;
		}
	}
}
