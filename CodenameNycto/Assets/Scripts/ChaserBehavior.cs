using UnityEngine;
using System.Collections;

public class ChaserBehavior : MonoBehaviour {

	public Vector2 endPosition = Vector2.zero;
	public float speed = 1;
	public float aggroSpeed = 4;
	public float nonAggroSpeed = 1;
	public float aggroDistance = 6;
	public GameObject lantern;
	public GameObject player;

	private bool lightHit;

	public bool aggro = false;

	public Vector2 _startPosition  = Vector2.zero;
	public bool outgoing = true;

	private AudioSource aggroClip;
	private AudioSource pauseClip;

	private bool aggroPlaying;
	
	// Use this for initialization
	void Start () {

		AudioSource[] sources = this.GetComponents<AudioSource>();
		aggroClip = sources[0];
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

		lightHit = false;
		aggroPlaying = false;
		
	}

	//Update is called once per frame
	void Update()
	{

		if(Vector2.Distance(this.transform.position, player.transform.position) < aggroDistance && !aggroPlaying)
		{
			aggroClip.Play ();
			aggroPlaying = true;
		}

		if(Vector2.Distance(this.transform.position, player.transform.position) < aggroDistance && !lightHit)
		{
			this.transform.LookAt(player.transform);
			if(this.transform.position.x - player.transform.position.x < 1)
			{
				speed = aggroSpeed;
			}
			else
			{
				speed = -aggroSpeed;
			}
		}

		else if(this.transform.position.x <= _startPosition.x || this.transform.position.x >= endPosition.x)
		{
			if(this.transform.position.x <= _startPosition.x)
			{
				this.transform.position = _startPosition;
				speed = nonAggroSpeed;
				aggroPlaying = false;
			}
			else
			{
				this.transform.position = endPosition;
				speed = -nonAggroSpeed;
				aggroPlaying = false;
			}
		}
		this.transform.Translate(speed, 0 , 0);


	}

	public void hitLight()
	{
		lightHit = true;
		speed = 0;
		StartCoroutine("pause");
	}

	private bool playerFace()
	{
		if(lantern.GetComponent<LightFollow>().faceForward)
		{
			return false;
		}
		else
			return true;
	}

	IEnumerator pause()
	{
		yield return new WaitForSeconds(1.5f);
		speed = nonAggroSpeed;
		lightHit = false;
	}


}
