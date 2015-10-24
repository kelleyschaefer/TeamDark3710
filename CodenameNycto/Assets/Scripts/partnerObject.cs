using UnityEngine;
using System.Collections;

public class partnerObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activate()
	{
		//DO SOMETHING
		this.gameObject.SetActive(false);
	}

	public void deactivate()
	{
		//STOP DOING SOMETHING
		this.gameObject.SetActive(true);
	}
}
