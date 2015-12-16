using UnityEngine;
using System.Collections;

public class ObjectFollow : MonoBehaviour {

	public Transform player;
	public bool currentlyHeld;

	public bool faceForward;
	//How far away the item should be "held" from the player.
	public float holdOffset;

	//What moving platform the object is attached to
	public Transform objectGround;
	public float offsetY;
	public float offsetX;

	private bool objectGrounded;
	public bool isKey;

	private AudioSource pickupClip;
	private AudioSource dropClip;
	private AudioSource unlockClip;


	// Use this for initialization
	void Start () {
		currentlyHeld = false;
		objectGrounded = false;
		AudioSource[] sources = this.GetComponents<AudioSource>();
		dropClip = sources[0];
		pickupClip = sources[1];
		unlockClip = sources[2];
	}
	
	// Update is called once per frame
	void Update () {

		//IMPORTANT*** REMEMBER TO KEEP IT'S Z POSITION BELOW THE LIGHTS OR IT WON'T BE LIT
		if(currentlyHeld)
		{
			if(faceForward){
				transform.position = new Vector3(player.position.x + holdOffset, player.position.y, player.position.z);
				// Object follows the player with specified offset position
			}
			else{
				transform.position = new Vector3(player.position.x - holdOffset, player.position.y, player.position.z);
			}
		}
		else if(objectGrounded)
		{
			transform.position = new Vector3(objectGround.position.x - offsetX, objectGround.position.y + offsetY, objectGround.position.z);
		}
	
	}

	public void followGround(Transform ground, bool status)
	{
		objectGround = ground;
		objectGrounded = status;
		if(status)
		{
			offsetX = ground.position.x - player.position.x;
			//Play drop clip
			dropClip.Play ();
		}
		else
		{
			pickupClip.Play ();
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		//If object is a key
		if(col.tag == "Keyhole" && isKey)
		{
			col.gameObject.GetComponent<partnerObject>().activate();
			unlockClip.Play ();
			StartCoroutine("waitDelete");
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

	private IEnumerator waitDelete()
	{
		yield return new WaitForSeconds(unlockClip.clip.length);
		Destroy(this.gameObject);
	}
}
