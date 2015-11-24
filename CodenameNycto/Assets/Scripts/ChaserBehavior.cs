using UnityEngine;
using System.Collections;

public class ChaserBehavior : MonoBehaviour {

	public Vector2 endPosition = Vector2.zero;
	public float speed = 1;
	public float aggroSpeed = 4;
	public float nonAggroSpeed = 1;
	public float aggroDistance = 6;
	public GameObject player;

	public bool aggro = false;

	public Vector2 _startPosition  = Vector2.zero;
	public bool outgoing = true;
	
	// Use this for initialization
	void Start () {
		_startPosition = this.gameObject.transform.position;
		//Slighty offset start position so the chaser doesn't immediately go left
		_startPosition.x = _startPosition.x - .1f;
		
		
		float distance = Vector2.Distance(_startPosition, endPosition);
		if(distance != 0)
		{
			nonAggroSpeed = speed/distance;
			aggroSpeed = aggroSpeed/distance;
			speed = nonAggroSpeed;
		}
		
	}

	//Update is called once per frame
	void Update()
	{
		if(Vector2.Distance(this.transform.position, player.transform.position) < aggroDistance)
		{
			if(speed > 0)
			{
			speed = aggroSpeed;
			}
			else
			{
				speed = -aggroSpeed;
			}
		}
		else if(speed == aggroSpeed)
		{
			if(speed > 0 )
			{
				speed = nonAggroSpeed;
			}
			else
			{
				speed = -nonAggroSpeed;
			}

		}

		if(this.transform.position.x <= _startPosition.x)
		{
			speed = -speed;
		}
		else if(this.transform.position.x >= endPosition.x)
		{
			speed = -speed;
		}

		this.transform.Translate(speed, 0 , 0);
	}

	public void hitLight()
	{
		speed = -speed;
	}


}
