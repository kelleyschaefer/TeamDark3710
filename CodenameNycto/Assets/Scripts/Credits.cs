using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	public GameObject endGame;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!(this.gameObject.transform.position.y <= -70))
		{
			this.gameObject.transform.Translate(new Vector2(0, -1/60f));
		}
		else
		{
			endGame.SetActive(true);
		}

	
	}
}
