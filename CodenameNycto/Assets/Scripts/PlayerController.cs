using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float gravity = -35;
	public float jumpHeight = 3;
	public float walkSpeed = 3;
	public int maxHealth = 100;
	public bool invincible;

	public GameObject gameOverPanel;
	public GameObject FinishedLevel;

	public GameObject healthBar;

	public GameObject gameCamera;

	public GameObject playerLight;
	public GameObject playerHolding;
	public bool playerControl = true;

	private CharacterController2D _controller;
	private AnimationController2D _animator;
	private int currentHealth = 0;

	public bool knockedBack;
	public float knockbackX = 20;
	public float knockbackY = 10;
	public float knockbackDuration = 1;


	private GameObject nearbyObject;

	// Use this for initialization
	void Start () {
		_controller = gameObject.GetComponent<CharacterController2D>();
		_animator = gameObject.GetComponent<AnimationController2D>();
		currentHealth = maxHealth;

		playerHolding = null;
		nearbyObject = null;
		invincible = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//If the player was knocked back, temporarily disable control for the duration
		if(knockedBack)
		{
			StartCoroutine("waitKnockback");
		}

		if(playerControl)
		{
			Vector3 velocity = PlayerInput();
			_controller.move(velocity*Time.deltaTime);
		}
	}

	private Vector3 PlayerInput()
	{
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

		//Drop key 
		if(Input.GetKeyDown("q"))
		{
			//if we're on a moving platform, we'll want to have it follow it too
			if(_controller.isGrounded && _controller.ground!=null && _controller.ground.tag == "MovingPlatform")
			{
				//If we're not holding an object but holding the Lantern, have the Lantern follow
				if(playerHolding == null && playerLight.GetComponent<LightFollow>().currentlyHeld)
				{
					playerLight.GetComponent<LightFollow>().followGround(_controller.ground.transform, true);
					playerLight.GetComponent<LightFollow>().currentlyHeld = false;
				}
				//If we're holding an object (should NOT be holding Lantern) then have Object follow the platform
				else if(playerHolding != null)
				{
					playerHolding.GetComponent<ObjectFollow>().followGround(_controller.ground.transform, true);
					playerHolding.GetComponent<ObjectFollow>().currentlyHeld = false;
					playerHolding = null;
				}
			}
			//If not a moving platform, just place it down.
			else if(playerLight.GetComponent<LightFollow>().currentlyHeld && _controller.ground!=null)
			{
				playerLight.GetComponent<LightFollow>().followGround(_controller.ground.transform, true);
				playerLight.GetComponent<LightFollow>().currentlyHeld = false;
			}

			//Otherwise, put object down on non-moving platform, mark it as not held.
			else if(playerHolding != null && _controller.ground!=null)
			{
				playerHolding.GetComponent<ObjectFollow>().followGround(_controller.ground.transform,true);
				playerHolding.GetComponent<ObjectFollow>().currentlyHeld = false;
				playerHolding = null;
			}
		}
		//Pickup key
		else if(Input.GetKeyDown("e"))
		{
			//If the Lantern is close enough to be picked up and player is not holding something, pick it up
			if(playerLight.GetComponent<LightFollow>().nearbyPlayer() && playerHolding == null)
			{
				playerLight.GetComponent<LightFollow>().currentlyHeld = true;
			}
			//Else, if there is an object within range ot be picked up, pick it up.
			else if(playerHolding == null && nearbyObject !=null)
			{
				playerHolding = nearbyObject;
				playerHolding.GetComponent<ObjectFollow>().currentlyHeld = true;
			}
		}

		//Facing left and not currently knocked back
		if(Input.GetAxis("Horizontal") < 0 && !knockedBack)
		{
			if(_controller.isGrounded)
			{
				_animator.setAnimation("Player_run");
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
		else if(Input.GetAxis("Horizontal") > 0 && !knockedBack)
		{
			if(_controller.isGrounded)
			{
				_animator.setAnimation("Player_run");
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
				_animator.setAnimation("Player_idle");
		}
		if(Input.GetAxis("Jump") > 0 && _controller.isGrounded)
		{
			_animator.setAnimation("Player_jump");
			velocity.y = Mathf.Sqrt (2f * jumpHeight *-gravity);
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
			PlayerDamage(25);
			if(playerLight.GetComponent<LightFollow>().currentlyHeld)
			{
			playerLight.GetComponent<LightFollow>().lightDamage(1);
			}
		}
		//In this case, the player touched a trick obstacles -without- their lantern destroying it.
		else if(col.tag == "TrickDamaging" && !invincible)
		{
			PlayerDamage(25);

			/*
			 * Not actually necessary to damage light here
			if(playerLight.GetComponent<LightFollow>().currentlyHeld)
			{
				playerLight.GetComponent<LightFollow>().lightDamage(1);
			}
			*/
		}
		else if(col.tag == "Object")
		{
			nearbyObject = col.gameObject;
		}
		else if(col.tag =="Enemy")
		{
			PlayerDamage (25);
		}
		if(col.tag == "LevelGoal")
		{
			playerControl = false;
			FinishedLevel.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag =="Object")
		{
			nearbyObject = null;
		}
	}

	//Standard fall death. Stops camera follow, reduces health bar, stops player control, opens a Gameover panel
	private void PlayerFallDeath()
	{
		currentHealth = 0;
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth()*256, 32);
		gameCamera.GetComponent<CameraFollow>().stopFollow();
		playerControl = false;
		gameOverPanel.SetActive(true);
	}

	//Reduces player's health and transforms the healthbar.
	//Also induces knockback and death if applicable.
	public void PlayerDamage(int damage)
	{
		if(currentHealth == maxHealth && damage < 0)
		{
			return;
		}
		currentHealth -= damage;
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth()*256, 32);

		if(damage > 0)
		{
		Knockback();
		}
		if(currentHealth <= 0)
		{
			PlayerDeath();
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
		_animator.setAnimation("Player_death");
		playerControl = false;

		gameOverPanel.SetActive(true);
	}

	//Knockback the player when taking damage (not lamp damage)
	private void Knockback()
	{
		//Make sure the player can't move (otherwise they'll just bounce on top of damage continually)
		playerControl = false;
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
		/*
		 * TODO: Add flashing animation??
		 * 
		 * */
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
