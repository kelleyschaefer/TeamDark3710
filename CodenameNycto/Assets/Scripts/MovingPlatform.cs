using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public Vector3 endPosition = Vector3.zero;
	public float speed = 1;

	private float _timer = 0;
	private Vector3 _startPosition  = Vector3.zero;
	private bool outgoing = true;

	// Use this for initialization
	void Start () {
		_startPosition = this.gameObject.transform.position;
		endPosition = endPosition + _startPosition;


		float distance = Vector3.Distance(_startPosition, endPosition);
		if(distance != 0)
		{
			speed = speed/distance;
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		_timer += Time.deltaTime * speed;

		if(outgoing)
		{
			this.transform.position = Vector3.Lerp(_startPosition, endPosition, _timer);
			if(_timer >1)
			{
				outgoing = false;
				_timer = 0;
			}
		}
		else
		{
			this.transform.position = Vector3.Lerp(endPosition, _startPosition, _timer);
			if(_timer > 1)
			{
				outgoing = true;
				_timer = 0;
			}
		}
	
	}

	void onDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position, endPosition+this.transform.position);
	}
}
