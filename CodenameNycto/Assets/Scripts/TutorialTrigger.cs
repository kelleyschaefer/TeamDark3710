using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {

	Color alpha;

	// Use this for initialization
	void Start () {
		this.GetComponent<TextMesh>().color = new Color(1f, 1f, 1f, 0);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.tag == "Player")
		{
			StartCoroutine("FadeIn");
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if(col.tag == "Player")
		{
			StartCoroutine("FadeOut");
		}
	}

	IEnumerator FadeIn()
	{
		float timer = 0.0f;

		while(timer<1f)
		{
			timer+=Time.deltaTime/2;
			this.GetComponent<TextMesh>().color = new Color(1f, 1f, 1f, timer);
			yield return null;
		}
	}

	IEnumerator FadeOut()
	{
		float timer = 1.0f;
		
		while(timer > 0f)
		{
			timer-=Time.deltaTime/3;
			this.GetComponent<TextMesh>().color = new Color(1f, 1f, 1f, timer);
			yield return null;
		}
		this.gameObject.SetActive(false);
	}
}
