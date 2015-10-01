using UnityEngine;
using System.Collections;

public class LampGroundFollow : MonoBehaviour {

	public Transform lampGround;

	public float y_offset = 0;
	public float x_offset = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if(lampGround)
		{
			transform.position = new Vector3(lampGround.position.x + x_offset, lampGround.position.y+y_offset, lampGround.position.z - 2);
		}
	}
}
