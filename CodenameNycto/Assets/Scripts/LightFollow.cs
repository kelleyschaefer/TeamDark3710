using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LightFollow : MonoBehaviour {

	public Transform player;
	public Light lightSource;

	public int currentLight = 8;
	public int maxLight = 8;
	public int maxRange = 10;
	public int lampHeal = 1;

	public float offsetX;
	public float offsetY;
	public float holdOffsetx;
	public float holdOffsety;

	public bool currentlyHeld = true;

	public GameObject lightBar;
	
	public Sprite[] lightBars;

	//True if player is facing right, False is player is facing left.
	public bool faceForward;

	public Transform lightGround;
	private bool lightGrounded;

	private AudioSource dropClip;
	private AudioSource pickupClip;
	private AudioSource damageClip;

	// Use this for initialization
	void Start () {
		currentlyHeld = true;
		lightSource = GetComponent<Light>();
		currentLight = maxLight;
		lightSource.intensity = currentLight *1.0f;

		AudioSource[] sources = this.GetComponents<AudioSource>();
		dropClip = sources[0];
		pickupClip = sources[1];
		damageClip = sources[2];
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
			transform.position = new Vector3(player.position.x + holdOffsetx, player.position.y + holdOffsety, player.position.z + -2);
			// Light follows the player with specified offset position
			}
			else{
			transform.position = new Vector3(player.position.x - holdOffsetx, player.position.y + holdOffsety, player.position.z + -2);
			}
		}

		else if(lightGrounded)
		{
			transform.position = new Vector3(lightGround.position.x - offsetX, lightGround.position.y + offsetY, -2f);

		}
	}

	public void followGround(Transform ground, bool status)
	{
		lightGround = ground;
		lightGrounded = status;
		if(status)
		{
			offsetX = (lightGround.position.x - player.position.x);
			dropClip.Play ();
		}
		else
		{
			pickupClip.Play ();
		}
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
				if(currentLight < maxLight)
				{
					lightDamage(-1);
				}
				player.GetComponent<PlayerController>().PlayerDamage(-lampHeal);
			}

			if(col.gameObject.GetComponent<LampCollision>().NotLit)
			{
			col.GetComponentInParent<LampCollision>().lightUp();
			}
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
				lightDamage(1);
			}
		}
		//Player checkpoint, resets spawn location for this instance.
		if (col.tag == "Checkpoint") 
		{
			PlayerPrefs.SetFloat ("xPosition", col.gameObject.transform.position.x);
			PlayerPrefs.SetFloat ("yPosition", col.gameObject.transform.position.y +1f);
			PlayerPrefs.SetInt ("Checkpoint", 1);
			col.gameObject.GetComponent<CheckpointEffect>().Activate();
			
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

	public void refill()
	{
		currentLight = maxLight;
		lightSource.intensity = (float)(maxLight);
		lightSource.range = (float)(maxRange);
		lightBar.GetComponent<Image>().sprite = lightBars[currentLight-1];
		changeColor ();

	}

	public void lightDamage(int damage)
	{
		if(damage > 0)
		{
			damageClip.Play ();
		}
		if(currentLight < 2 && damage > 0)
		{
			return;
		}
		currentLight -= (damage);
		//lightSource.intensity-= (float)(damage);
		//lightSource.range -= (float)(damage);
		lightBar.GetComponent<Image>().sprite = lightBars[currentLight-1];
		changeColor();
	}

	public void changeColor()
	{
		if(currentLight <= 3)
		{
			lightSource.color = Color.red;
		}
		else if(currentLight > 3 && currentLight < 6)
		{
			lightSource.color = Color.yellow;
		}
		else
		{
			lightSource.color = Color.white;
		}
	}
}

