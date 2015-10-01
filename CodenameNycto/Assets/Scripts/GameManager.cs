using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void ExitLevel()
	{
		Application.LoadLevel("MainMenu");
	}

	public void Play()
	{
		Application.LoadLevel("PrototypeLevel");
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
