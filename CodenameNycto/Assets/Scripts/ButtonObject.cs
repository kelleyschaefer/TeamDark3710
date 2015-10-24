using UnityEngine;
using System.Collections;

public class ButtonObject : MonoBehaviour {

	public bool oneTime;
	public bool timed;
	public bool heldDown;

	public float timeActive;

	public bool active;
	public bool playerLeft;
	public bool objectLeft;

	public GameObject partnerObject;

	// Use this for initialization
	void Start () {
		playerLeft = true;
		objectLeft = true;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D (Collider2D col)
	{
		if(!active)
		{
		if(col.tag == "Object" || col.tag == "Player")
		{
				if(col.tag == "Object")
				{
					objectLeft = false;
				}
				else if (col.tag == "Player")
				{
					playerLeft = false;
				}
			if(oneTime)
			{
				//ACTIVATE BUTTON
				active = true;
				partnerObject.GetComponent<partnerObject>().activate();
			}
			else if(heldDown)
			{
				active = true;
				partnerObject.GetComponent<partnerObject>().activate();
			}
			else if(timed)
			{
				partnerObject.GetComponent<partnerObject>().activate();
				StartCoroutine("endActive");
			}
		}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(heldDown)
		{
			if(col.tag == "Object")
			{
				objectLeft = true;
			}
			else if (col.tag == "Player")
			{
				playerLeft = true;
			}
			if(playerLeft && objectLeft)
			{
				active = false;
				partnerObject.GetComponent<partnerObject>().deactivate();
			}
		}
	}

	//Activate button for timeActive seconds
	private IEnumerator endActive()
	{
		yield return new WaitForSeconds(timeActive);
		active = false;
		partnerObject.GetComponent<partnerObject>().deactivate();

	}
}
