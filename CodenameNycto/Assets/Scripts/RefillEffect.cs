using UnityEngine;
using System.Collections;


public class RefillEffect : MonoBehaviour {

	public ParticleSystem particle_;

	// Use this for initialization
	void Start () {

		particle_.enableEmission = false;
	
	}

	public void partEffect()
	{
		particle_.enableEmission = true;
		StartCoroutine("stopParticle");
		
	}
	
	private IEnumerator stopParticle()
	{
		yield return new WaitForSeconds(1);
		particle_.enableEmission = false;
	}
}
