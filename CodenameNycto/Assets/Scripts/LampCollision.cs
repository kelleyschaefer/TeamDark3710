using UnityEngine;
using System.Collections;

public class LampCollision : MonoBehaviour {

	public Light lightSource;
	public int startLit;
	public int becomeLit;
	public bool NotLit;

	// Use this for initialization
	void Start () {
		if(NotLit)
		{
			lightSource.intensity = 0;
		}
		else
		{
			lightSource.intensity = startLit;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void lightUp()
	{
		lightSource.intensity = becomeLit;
		NotLit = false;
	}
}
