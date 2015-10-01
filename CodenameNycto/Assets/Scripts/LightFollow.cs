using UnityEngine;
using System.Collections;

public class LightFollow : MonoBehaviour {

	public Transform player;
	public Light lightSource;

	public float currentLight = 5;

	public bool currentlyHeld = true;

	//True if player is facing right, False is player is facing left.
	public bool faceForward;

	public Transform lightGround;
	private bool lightGrounded;

	// Use this for initialization
	void Start () {
		currentlyHeld = true;
		lightSource = GetComponent<Light>();
		currentLight = lightSource.intensity;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(Input.GetKeyDown("q"))
		{
			currentlyHeld = false;
		}
		else if(Input.GetKeyDown("e") && nearbyPlayer ())
		{
			currentlyHeld = true;
		}
		*/

		if(currentlyHeld)
		{
			if(faceForward){
			transform.position = new Vector3(player.position.x + 1.25f, player.position.y, player.position.z + -2);
			// Light follows the player with specified offset position
			}
			else{
			transform.position = new Vector3(player.position.x - 1.25f, player.position.y, player.position.z + -2);
			}
		}
		else if(lightGrounded)
		{
			transform.position = new Vector3(lightGround.position.x, lightGround.position.y+1.25f, lightGround.position.z - 2);
		}
	}

	public void followGround(Transform ground, bool status)
	{
		lightGround = ground;
		lightGrounded = status;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "LightDamaging")
		{
			lightDamage(1);
		}
		else if(col.tag == "Waterfall")
		{
			lightDamage(2);
		}
		else if(col.tag == "Lamp")
		{

			if(col.GetComponentInParent<LampCollision>().NotLit)
			{
				if(currentLight < 8)
				{
					currentLight += 1;
					lightSource.intensity +=1f;
					lightSource.range +=1f;
				}
				player.GetComponent<PlayerController>().PlayerDamage(-25);
			}

			col.GetComponentInParent<LampCollision>().lightUp();
		}
		else if(col.tag == "TrickDamaging")
		{
			if(currentLight > 3)
			{
				Destroy (col.gameObject);
			}
		}
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

	public void lightDamage(int multiplier)
	{
		if(currentLight < 2)
		{
			return;
		}
		currentLight -=1*multiplier;
		lightSource.intensity-=1f*multiplier;
		lightSource.range -= 1f*multiplier;
	}
}
