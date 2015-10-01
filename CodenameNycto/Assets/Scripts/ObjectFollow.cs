using UnityEngine;
using System.Collections;

public class ObjectFollow : MonoBehaviour {

	public Transform player;
	public bool currentlyHeld;

	public bool faceForward;
	
	public Transform objectGround;
	private bool objectGrounded;


	// Use this for initialization
	void Start () {
		currentlyHeld = false;
		objectGrounded = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(currentlyHeld)
		{
			if(faceForward){
				transform.position = new Vector3(player.position.x + 1.25f, player.position.y, player.position.z + -2);
				// Object follows the player with specified offset position
			}
			else{
				transform.position = new Vector3(player.position.x - 1.25f, player.position.y, player.position.z + -2);
			}
		}
		else if(objectGrounded)
		{
			transform.position = new Vector3(objectGround.position.x, objectGround.position.y+1.25f, objectGround.position.z - 2);
		}
	
	}

	public void followGround(Transform ground, bool status)
	{
		objectGround = ground;
		objectGrounded = status;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		//nothing yet
	}

	public bool nearbyPlayer()
	{
		if(Vector2.Distance (transform.position, player.position) <= 3)
		{
			return true;
		}
		else
			return false;
	}
}
