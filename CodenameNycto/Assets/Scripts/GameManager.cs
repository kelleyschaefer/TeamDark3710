using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject Level1Button;

	private List<string> UnlockedLevels;

	// Use this for initializations
	void Start () {
		UnlockedLevels = new List <string>();
		UnlockedLevels.Add("PrototypeLevel");
		UnlockedLevels.Add("Level1");

		LevelSelectPopulate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LevelSelectPopulate()
	{
		foreach (string s in UnlockedLevels)
		{

		}
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

	public void NewGame()
	{
		PlayerPrefs.SetInt ("Checkpoint", 0);
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
