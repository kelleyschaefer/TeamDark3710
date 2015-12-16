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

	public Sprite onButton;
	public Sprite offButton;

	public GameObject partnerObject;
	public SpriteRenderer _SpriteRend;

	private AudioSource clickClip;
	private AudioSource timerClip;

	private bool Ticking;

	// Use this for initialization
	void Start () {
		playerLeft = true;
		objectLeft = true;
		_SpriteRend = this.GetComponent<SpriteRenderer>();
		AudioSource[] sources = this.GetComponents<AudioSource>();
		clickClip = sources[0];
		timerClip = sources[1];
		Ticking = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Ticking)
		{
			timerClip.pitch += 0.1f * Time.deltaTime;
		}
		else
		{
			timerClip.pitch = 1.0f;
		}
	
	}

	void OnTriggerEnter2D (Collider2D col)
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
				if(!active)
				{
					clickClip.pitch = 0.8f;
					clickClip.Play ();
				}

				//ACTIVATE BUTTON
				active = true;
				_SpriteRend.sprite = onButton;
				partnerObject.GetComponent<partnerObject>().activate();

			}
			else if(heldDown)
			{
				if(!active)
				{
				clickClip.pitch = 0.8f;
				clickClip.Play ();
				}

				active = true;
				_SpriteRend.sprite = onButton;
				partnerObject.GetComponent<partnerObject>().activate();

			}
			else if(timed && !active)
			{
				if(!active)
				{
					StartCoroutine("endActive");
					clickClip.pitch = 0.8f;
					clickClip.Play ();
					timerClip.Play ();
					Ticking = true;
				}
				active = true;
				_SpriteRend.sprite = onButton;
				partnerObject.GetComponent<partnerObject>().activate();

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
				_SpriteRend.sprite = offButton;
				partnerObject.GetComponent<partnerObject>().deactivate();
				if(col.tag == "Player")
				{
				clickClip.pitch = 1.1f;
				clickClip.Play ();
				}
			}
		}
	}

	//Activate button for timeActive seconds
	private IEnumerator endActive()
	{
		yield return new WaitForSeconds(timeActive);
		active = false;
		_SpriteRend.sprite = offButton;
		partnerObject.GetComponent<partnerObject>().deactivate();
		timerClip.Stop ();
		clickClip.pitch = 1.1f;
		clickClip.Play ();
		Ticking = false;
	}
}
