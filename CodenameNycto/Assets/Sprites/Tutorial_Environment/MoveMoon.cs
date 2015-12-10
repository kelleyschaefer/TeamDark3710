using UnityEngine;
using System.Collections;

public class MoveMoon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public Transform center;
	public float radius = 5;
	public float angle =0;
	public float period = 6f;
	
	// Update is called once per frame
	void Update () {
		angle += period*Time.deltaTime*Time.deltaTime; 
		float x = Mathf.Cos(angle)*radius + center.transform.position.x; //x=cos(angle)*R+a;
		float y = Mathf.Sin(angle)*radius + center.transform.position.y; //y=sin(angle)*R+b;
		this.gameObject.transform.position = new Vector2 (x,y);
	}
}
