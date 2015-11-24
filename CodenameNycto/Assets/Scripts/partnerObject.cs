using UnityEngine;
using System.Collections;

public class partnerObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(this.gameObject.tag == "MovingPlatform")
		{
			this.gameObject.GetComponent<MovingPlatform>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activate()
	{
		if(this.gameObject.tag == "MovingPlatform")
		{
			this.gameObject.GetComponent<MovingPlatform>().enabled = true;
		}
		else
		{
			this.gameObject.SetActive(false);
		}
		
	}

	public void deactivate()
	{

		if(this.gameObject.tag == "MovingPlatform")
		{
			this.gameObject.GetComponent<MovingPlatform>().enabled = false;
		}
		//STOP DOING SOMETHING
		this.gameObject.SetActive(true);
	}
}
