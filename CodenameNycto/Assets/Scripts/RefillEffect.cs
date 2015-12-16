using UnityEngine;
using System.Collections;


public class RefillEffect : MonoBehaviour {

	public ParticleSystem particle_;
	private AudioSource lightClip;

	// Use this for initialization
	void Start () {
		lightClip = this.GetComponent<AudioSource>();
		particle_.enableEmission = false;
	
	}

	public void partEffect()
	{
		particle_.enableEmission = true;
		StartCoroutine("stopParticle");
		lightClip.Play ();
		
	}
	
	private IEnumerator stopParticle()
	{
		yield return new WaitForSeconds(1);
		particle_.enableEmission = false;
	}
}
