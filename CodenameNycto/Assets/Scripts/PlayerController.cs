using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float gravity = -35;
	public float jumpHeight = 3;
	public float walkSpeed = 3;
	public int maxHealth = 100;

	public GameObject gameOverPanel;

	public GameObject healthBar;

	public GameObject gameCamera;

	public GameObject playerLight;
	public GameObject playerHolding;
	public bool playerControl = true;

	private CharacterController2D _controller;
	private AnimationController2D _animator;
	private Rigidbody2D _rigidbody;
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
		_rigidbody = gameObject.GetComponent<Rigidbody2D>();
		currentHealth = maxHealth;

		playerHolding = null;
		nearbyObject = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
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

		if(Input.GetKeyDown("q"))
		{
			if(_controller.isGrounded && _controller.ground!=null && _controller.ground.tag == "MovingPlatform")
			{
				if(playerHolding == null && playerLight.GetComponent<LightFollow>().currentlyHeld)
				{
				playerLight.GetComponent<LightFollow>().followGround(_controller.ground.transform, true);
					playerLight.GetComponent<LightFollow>().currentlyHeld = false;
				}
				else if(playerHolding != null)
				{
					playerHolding.GetComponent<ObjectFollow>().followGround(_controller.ground.transform, true);
					playerHolding.GetComponent<ObjectFollow>().currentlyHeld = false;
					playerHolding = null;
				}
			}
			else if(playerLight.GetComponent<LightFollow>().currentlyHeld)
			{
					playerLight.GetComponent<LightFollow>().followGround(null, false);
			}
			if(playerLight.GetComponent<LightFollow>().currentlyHeld)
			{
				playerLight.GetComponent<LightFollow>().currentlyHeld = false;
			}
			else if(playerHolding != null)
			{
				playerHolding.GetComponent<ObjectFollow>().followGround(null,false);
				playerHolding.GetComponent<ObjectFollow>().currentlyHeld = false;
				playerHolding = null;
			}
		}
		else if(Input.GetKeyDown("e"))
		{
			if(playerLight.GetComponent<LightFollow>().nearbyPlayer())
			{
				playerLight.GetComponent<LightFollow>().currentlyHeld = true;
			}

			else if(playerHolding == null && nearbyObject !=null)
			{
				playerHolding = nearbyObject;
				playerHolding.GetComponent<ObjectFollow>().currentlyHeld = true;
			}
		}
		//velocity.x = 0;
		
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
		
		velocity.x *= 0.8f;
		
		velocity.y += gravity*Time.deltaTime;

		return velocity;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "killZ")
		{
			PlayerFallDeath();
		}
		else if (col.tag == "Damaging")
		{
			PlayerDamage(10);
			if(playerLight.GetComponent<LightFollow>().currentlyHeld)
			{
			playerLight.GetComponent<LightFollow>().lightDamage(1);
			}
		}
		else if(col.tag == "TrickDamaging")
		{
			PlayerDamage(10);

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
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag =="Object")
		{
			nearbyObject = null;
		}
	}

	private void PlayerFallDeath()
	{
		currentHealth = 0;
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth()*256, 32);
		gameCamera.GetComponent<CameraFollow>().stopFollow();
		playerControl = false;
		gameOverPanel.SetActive(true);
	}

	public void PlayerDamage(int damage)
	{
		if(currentHealth == maxHealth && damage < 0)
		{
			return;
		}
		currentHealth -= damage;
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth()*256, 32);

		Knockback();
		if(currentHealth <= 0)
		{
			PlayerDeath();
		}

	}

	private float normalizedHealth()
	{
		return (float)currentHealth/(float)maxHealth;
	}

	private void PlayerDeath()
	{
		_animator.setAnimation("Player_death");
		playerControl = false;

		gameOverPanel.SetActive(true);
	}

	private void Knockback()
	{
		playerControl = false;
		if(_animator.getFacing() == "Right")
			{
			_controller.move(new Vector2(-knockbackX, knockbackY)*Time.deltaTime);
			}
		else
			{
			_controller.move(new Vector2(knockbackX, knockbackY)*Time.deltaTime);
			}
		playerControl = true;
		knockedBack = true;
	}


	private IEnumerator waitKnockback()
	{
		yield return  new WaitForSeconds(knockbackDuration);
		knockedBack = false;
	}


}
