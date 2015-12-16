using UnityEngine;
using System.Collections;

public class FollowerBehavior : MonoBehaviour {

	public GameObject victim;
	public float speed;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if(Vector2.Distance(this.transform.position, victim.transform.position) > 15)
		{
			this.GetComponent<AudioSource>().volume = 0;
		}
		else
		{
			this.GetComponent<AudioSource>().volume = 1;
		}

		if(victim.GetComponent<PlayerController>().currentHealth == 0)
		{
			this.gameObject.SetActive (false);
		}
		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards (transform.position, victim.GetComponent<PlayerController>().transform.position, step);
	}
}
