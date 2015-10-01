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

	private CharacterController2D _controller;
	private AnimationController2D _animator;
	private int currentHealth = 0;
	private bool playerControl = true;

	// Use this for initialization
	void Start () {

		gameCamera.GetComponent<CameraFollow2D>().startCameraFollow();
		_controller = gameObject.GetComponent<CharacterController2D>();
		_animator = gameObject.GetComponent<AnimationController2D>();

		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
	{
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
		//velocity.x = 0;
		
		if(Input.GetAxis("Horizontal") < 0)
		{
			if(_controller.isGrounded)
			{
				_animator.setAnimation("Player_run");
			}
			velocity.x = -walkSpeed;
			_animator.setFacing("Left");
		}
		else if(Input.GetAxis("Horizontal") > 0)
		{
			if(_controller.isGrounded)
			{
				_animator.setAnimation("Player_run");
			}
			velocity.x = walkSpeed;
			_animator.setFacing("Right");
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
		
		velocity.x *= 0.90f;
		
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
			PlayerDamage(25);
		}
	}

	private void PlayerFallDeath()
	{
		currentHealth = 0;
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth()*256, 32);
		gameCamera.GetComponent<CameraFollow2D>().stopCameraFollow();
		playerControl = false;
		gameOverPanel.SetActive(true);
	}

	private void PlayerDamage(int damage)
	{
		currentHealth -= damage;
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(normalizedHealth()*256, 32);

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

}
