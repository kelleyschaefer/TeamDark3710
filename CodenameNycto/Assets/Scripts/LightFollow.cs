using UnityEngine;
using System.Collections;

public class LightFollow : MonoBehaviour {

	public Transform player;
	public Light lightSource;

	public float currentLight = 8;
	public float maxLight = 8;

	public float offsetX;
	public float offsetY;
	public float holdOffset;

	public bool currentlyHeld = true;

	public GameObject LightBar;

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
			transform.position = new Vector3(player.position.x + holdOffset, player.position.y, player.position.z + -2);
			// Light follows the player with specified offset position
			}
			else{
			transform.position = new Vector3(player.position.x - holdOffset, player.position.y, player.position.z + -2);
			}
		}
		else if(lightGrounded)
		{
			transform.position = new Vector3(lightGround.position.x + offsetX, lightGround.position.y + offsetY, lightGround.position.z - 2);
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
			lightDamage(1f);
		}
		else if(col.tag == "Waterfall")
		{
			lightDamage(2f);
		}
		else if(col.tag == "Lamp")
		{

			if(col.GetComponentInParent<LampCollision>().NotLit)
			{
				if(currentLight < 8)
				{
					lightDamage(-1f);
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
		else if(col.tag =="Enemy")
		{
			if(currentLight > 3)
			{
				col.gameObject.GetComponent<ChaserBehavior>().hitLight();
				lightDamage(1f);
			}
		}
	}

	public bool nearbyPlayer()
	{
		if(Vector2.Distance (transform.position, player.position) <= 2)
		{
			return true;
		}
		else
			return false;
	}

	private float normalizedLight()
	{
		return (float)currentLight / (float)maxLight;
	}

	public void lightDamage(float damage)
	{
		if(currentLight < 2)
		{
			return;
		}
		currentLight -= damage;
		lightSource.intensity-= damage;
		lightSource.range -= damage;
		LightBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedLight()*256, 32);
	}
}
