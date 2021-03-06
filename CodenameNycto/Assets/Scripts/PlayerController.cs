﻿using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float gravity = -35;
	public float jumpHeight = 3;
	public float walkSpeed = 3;
	public int maxHealth = 8;
	public bool invincible;
	public bool nearLadder;
	public bool onLadder;

	public Sprite[] healthBars;


	public GameObject gameOverPanel;
	public GameObject FinishedLevel;
	public GameObject gameManager;

	public GameObject healthBar;

	public GameObject gameCamera;

	public GameObject playerLight;
	public GameObject playerHolding;
	public bool playerControl = true;

	private CharacterController2D _controller;
	private AnimationController2D _animator;
	public int currentHealth = 0;

	public bool knockedBack;
	public float knockbackX = 20;
	public float knockbackY = 10;
	public float knockbackDuration = 1;

	private AudioSource walkClip;
	private AudioSource jumpClip;
	private AudioSource hurtClip;
	private AudioSource levelClip;
	private AudioSource ladderClip;
	private AudioSource gameoverClip;

	private bool playingClimb;

	private GameObject nearbyObject;
	private int walkWait;

	public bool hardFloor;
	private bool paused;


	// Use this for initialization
	void Start () {
		_controller = gameObject.GetComponent<CharacterController2D>();
		_animator = gameObject.GetComponent<AnimationController2D>();
		paused = false;

		//Gather audio sources and set the private variables
		AudioSource[] sources = this.GetComponents<AudioSource>();
		if(hardFloor)
		{
			walkClip = sources[4];
		}
		else
		{
			walkClip = sources[0];
		}

		jumpClip = sources[2];
		hurtClip = sources[1];
		levelClip = sources[3];
		ladderClip = sources[5];
		gameoverClip = sources[6];

		playingClimb = false;


		currentHealth = maxHealth;

		playerHolding = null;
		nearbyObject = null;
		invincible = false;
		onLadder = false;
		nearLadder = false;
		if(PlayerPrefs.GetInt("Checkpoint") == 1)
		{
			this.transform.position = new Vector2(PlayerPrefs.GetFloat("xPosition"), PlayerPrefs.GetFloat("yPosition"));
		}
		walkWait = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 velocity;
		//If the player was knocked back, temporarily disable control for the duration
		if(knockedBack)
		{
			StartCoroutine("waitKnockback");
		}

		if(!playingClimb && onLadder)
		{
			ladderClip.Play ();
			playingClimb = true;
		}

		if(playerControl)
		{
			velocity = PlayerInput();
			_controller.move(velocity*Time.deltaTime);
			if(_controller.isGrounded && _controller.ground!= null && velocity.magnitude >= 8f && walkWait >= 24)
			{
				//_audioPlayer.volume = Random.Range (0.25f, 5f);
				walkClip.pitch = Random.Range (0.5f, .8f);
				walkClip.Play();
				walkWait = 0;
			}
		}
		
		walkWait++;

	}

	private Vector3 PlayerInput()
	{
		//Pause functionality
		if(Input.GetKey(KeyCode.Escape))
		{
			if(Time.timeScale > 0)
			{
				gameManager.GetComponent<GameManager>().Pause();
				paused = true;
			}
		}

		if(!nearLadder || _controller.isGrounded)
		{
			onLadder = false;
			ladderClip.Stop ();
			playingClimb = false;
		}
		if(!onLadder)
		{
			gravity = -35;
		}

		Vector3 velocity = _controller.velocity;

		//If we're grounded and the ground is a moving platform, move with the platform
		if(_controller.isGrounded && _controller.ground!=null && _controller.ground.tag == "MovingPlatform")
		{
			this.transform.parent = _controller.ground.transform;
		}
		else
		{
			if(this.transform.parent != null)
			{
				this.transform.parent = null;
			}
		}


		if(Input.GetMouseButtonUp(0) && !paused)
		{
			if(playerLight.GetComponent<LightFollow>().currentlyHeld)
			{
				//Holding light, and object close enough
				if(nearbyObject != null)
				{
					playerHolding = nearbyObject;
					playerHolding.GetComponent<ObjectFollow>().currentlyHeld = true;
				}
					playerLight.GetComponent<LightFollow>().followGround(_controller.ground.transform, true);
					playerLight.GetComponent<LightFollow>().currentlyHeld = false;
					_animator.setAnimation("Pick_Up_");
			}
			else if (playerHolding != null)
			{
				if(playerLight.GetComponent<LightFollow>().nearbyPlayer ())
				{
					playerLight.GetComponent<LightFollow>().followGround(null, false);
					playerLight.GetComponent<LightFollow>().currentlyHeld = true;
				}
					playerHolding.GetComponent<ObjectFollow>().followGround(_controller.ground.transform, true);
					playerHolding.GetComponent<ObjectFollow>().currentlyHeld = false;
					playerHolding = null;
					_animator.setAnimation("Pick_Up_");
			}
			else
			{
				if(playerLight.GetComponent<LightFollow>().nearbyPlayer ())
				{
					playerLight.GetComponent<LightFollow>().followGround(null, false);
					playerLight.GetComponent<LightFollow>().currentlyHeld = true;
					_animator.setAnimation ("Pick_Up_");
				}
				else if(nearbyObject != null)
				{
					playerHolding = nearbyObject;
					playerHolding.GetComponent<ObjectFollow>().followGround(null, false);
					playerHolding.GetComponent<ObjectFollow>().currentlyHeld = true;
					_animator.setAnimation("Pick_Up_");
				}
			}
		}



		//Facing left and not currently knocked back
		if(Input.GetAxis("Horizontal") < 0 && !knockedBack && !onLadder)
		{
			if(_controller.isGrounded)
			{
				_animator.setAnimation("Walk");
			}
			velocity.x = -walkSpeed;
			_animator.setFacing("Left");
			playerLight.GetComponent<LightFollow>().faceForward = false;
			if(playerHolding != null)
			{
				playerHolding.GetComponent<ObjectFollow>().faceForward = false;
			}
		}
		//Facing right and not currently knocked back
		else if(Input.GetAxis("Horizontal") > 0 && !knockedBack && !onLadder)
		{
			if(_controller.isGrounded)
			{
				_animator.setAnimation("Walk");
			}
			velocity.x = walkSpeed;
			_animator.setFacing("Right");
			playerLight.GetComponent<LightFollow>().faceForward = true;
			if(playerHolding != null)
			{
				playerHolding.GetComponent<ObjectFollow>().faceForward = true;
			}
		}
		else
		{
			if(_controller.isGrounded)
				_animator.setAnimation("Idle");
		}
		if(Input.GetAxis("Jump") > 0 && _controller.isGrounded)
		{
			_animator.setAnimation("Jump_1");
			velocity.y = Mathf.Sqrt (2f * jumpHeight *-gravity);
			if(!jumpClip.isPlaying)
			{
				jumpClip.pitch = Random.Range (0.8f, 1.0f);
				jumpClip.Play();
			}
		}

		if(Input.GetAxis ("Vertical") > 0 && !playerHolding && !playerLight.GetComponent<LightFollow>().currentlyHeld && nearLadder)
		{
			_animator.setAnimation("Climb");
			onLadder = true;
			Vector2 upVector = new Vector2(0, 5);
			this.transform.Translate(upVector*Time.deltaTime);
			velocity.y = 0;
			gravity = 0;
		}
		if(Input.GetAxis("Vertical") < 0 && !playerHolding && !playerLight.GetComponent<LightFollow>().currentlyHeld && nearLadder)
		{
			_animator.setAnimation ("Climb");
			onLadder = true;
			_controller.ignoreOneWayPlatformsThisFrame = true;
			velocity.y = -walkSpeed;
		}

		//Simulate gravity
		velocity.x *= 0.8f;
		
		velocity.y += gravity*Time.deltaTime;

		return velocity;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		//Instant death
		if(col.tag == "killZ")
		{
			PlayerFallDeath();
		}
		//Player touched something damaging
		else if (col.tag == "Damaging" && !invincible)
		{
			PlayerDamage(2);
			if(playerLight.GetComponent<LightFollow>().currentlyHeld)
			{
			playerLight.GetComponent<LightFollow>().lightDamage(1);
			}
		}
		//In this case, the player touched a trick obstacles -without- their lantern destroying it.
		else if(col.tag == "TrickDamaging" && !invincible)
		{
			PlayerDamage(2);

			/*
			 * Not actually necessary to damage light here
			if(playerLight.GetComponent<LightFollow>().currentlyHeld)
			{
				playerLight.GetComponent<LightFollow>().lightDamage(1);
			}
			*/
		}
		//Interactable object, either key or otherwise
		else if(col.tag == "Object")
		{
			nearbyObject = col.gameObject;
		}
		//Damaging enemy, either Chaser or Follower
		else if(col.tag =="Enemy" && !invincible)
		{
			PlayerDamage (2);
		}
		if(col.tag == "Ladder")
		{
			nearLadder = true;
		}
		if (col.tag == "LightRefill") 
		{
			playerLight.GetComponent<LightFollow>().refill();
			col.gameObject.GetComponent<RefillEffect>().partEffect();
		}
		//Player checkpoint, resets spawn location for this instance.
		if (col.tag == "Checkpoint") 
		{
			PlayerPrefs.SetFloat ("xPosition", col.gameObject.transform.position.x);
			PlayerPrefs.SetFloat ("yPosition", col.gameObject.transform.position.y +1f);
			PlayerPrefs.SetInt ("Checkpoint", 1);
			col.gameObject.GetComponent<CheckpointEffect>().Activate();

		}
		//End of level!

		if(col.tag == "Follower" && !invincible)
		{
			PlayerDeath();
		}
		if(col.tag == "LevelGoal" && playerLight.GetComponent<LightFollow>().currentlyHeld)
		{
			invincible = true;
			playerControl = false;
			RenderSettings.ambientLight = Color.red;
			FinishedLevel.SetActive(true);
			PlayerPrefs.SetInt("Checkpoint", 0);
			levelClip.Play ();
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag =="Object")
		{
			nearbyObject = null;
		}
		if(col.tag == "Ladder")
		{
			nearLadder = false;
		}
	}

	//Standard fall death. Stops camera follow, reduces health bar, stops player control, opens a Gameover panel
	private void PlayerFallDeath()
	{
		currentHealth = 0;
		_animator.setAnimation("Hurt");
		playerControl = false;
		invincible = true;
		healthBar.GetComponent<Image>().sprite = healthBars[0];
		gameCamera.GetComponent<CameraFollow>().stopFollow();
		playerControl = false;
		gameOverPanel.SetActive(true);
		gameoverClip.Play ();
	}

	//Reduces player's health and transforms the healthbar.
	//Also induces knockback and death if applicable.
	public void PlayerDamage(int damage)
	{
		if(damage > 0)
		{
			hurtClip.Play ();
		}
		
		if(currentHealth == maxHealth && damage < 0)
		{
			return;
		}

		if(damage < 0)
		{
			currentHealth -= damage;
			healthBar.GetComponent<Image>().sprite = healthBars[currentHealth-1];
			return;
		}
		else
		{
			currentHealth -= damage;
			if(currentHealth <= 0)
			{
				PlayerDeath();
			}
			else if(damage > 0)
			{
			healthBar.GetComponent<Image>().sprite = healthBars[currentHealth-1];
			Knockback();
			}
		}

	}

	//Health helper method
	private float normalizedHealth()
	{
		return (float)currentHealth/(float)maxHealth;
	}

	//Stop control, set Gameover
	private void PlayerDeath()
	{
		_animator.setAnimation("Hurt");
		playerControl = false;
		invincible = true;
		healthBar.GetComponent<Image>().sprite = healthBars[0];

		gameOverPanel.SetActive(true);
		gameoverClip.Play ();
	}

	//Knockback the player when taking damage (not lamp damage)
	private void Knockback()
	{
		//Make sure the player can't move (otherwise they'll just bounce on top of damage continually)
		playerControl = false;
		_animator.setAnimation("Hurt");
		//Easiest way is just to tell where we're facing based off of what we already set in movement
		if(_animator.getFacing() == "Right")
			{
			_controller.move(new Vector2(-knockbackX, knockbackY)*Time.deltaTime);
			}
		else
			{
			_controller.move(new Vector2(knockbackX, knockbackY)*Time.deltaTime);
			}
		playerControl = true;
		//Set to true for control delay.
		knockedBack = true;
		//Player temporarily invincible from damage
		invincible = true;
	}


	//Executes after knockbackDuration seconds, returning knockback status to false
	private IEnumerator waitKnockback()
	{
		yield return  new WaitForSeconds(knockbackDuration);
		knockedBack = false;
		StartCoroutine("stopInvincibility");

	}

	private IEnumerator stopInvincibility()
	{
		yield return new WaitForSeconds(2);
		/*
		 * TODO: Add flashing animation??
		 * */
		invincible = false;
	}


}
