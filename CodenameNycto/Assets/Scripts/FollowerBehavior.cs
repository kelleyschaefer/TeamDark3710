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
		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards (transform.position, victim.GetComponent<PlayerController>().transform.position, step);
	}
}
