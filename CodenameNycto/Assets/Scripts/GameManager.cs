using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	// Use this for initializations
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
		PlayerPrefs.SetInt ("Checkpoint", 0);
		Application.LoadLevel("MainMenu");
	}

	public void Play()
	{
		Application.LoadLevel("Level1");
	}

	public void ExitGame()
	{
		PlayerPrefs.SetInt ("Checkpoint", 0);
		Application.Quit();
	}

	public void SaveExit()
	{
		/*
		 * TODO: Save file */
		PlayerPrefs.SetInt ("Checkpoint", 0);
		Application.Quit ();
	}

	public void SaveContinue()
	{
		/*
		 * TODO: Save file */
		int level = Application.loadedLevel;


		//ORGANIZE LEVELS BY NUMBER, VERY IMPORTANT
		PlayerPrefs.SetInt ("Checkpoint", 0);
		level++;
		Application.LoadLevel(level);
	}

	public void SaveMenu()
	{
		/*
		 * TODO: Save file */
		PlayerPrefs.SetInt ("Checkpoint", 0);
		Application.LoadLevel ("MainMenu");
	}
}
