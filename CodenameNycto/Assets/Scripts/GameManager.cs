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

	public void SaveExit()
	{
		/*
		 * TODO: Save file */
		Application.Quit ();
	}

	public void SaveContinue()
	{
		/*
		 * TODO: Save file */
		int level = Application.loadedLevel;

		//ORGANIZE LEVELS BY NUMBER, VERY IMPORTANT
		level++;
		Application.LoadLevel(level);
	}

	public void SaveMenu()
	{
		/*
		 * TODO: Save file */
		Application.LoadLevel ("MainMenu");
	}
}
