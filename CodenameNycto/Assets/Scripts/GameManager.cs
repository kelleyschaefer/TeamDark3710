using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject Level1Button;
	public Rect LevelSelectWindow;
	public GameObject play;
	public GameObject levelSelect;
	public GameObject exit;

	private bool hasFocus;
	private bool toggleWindow;
	private float width;
	private List<string> unlockedLevels;
	private Rect levelSelectWindow;

	// Use this for initializations
	void Start () {
		unlockedLevels = new List <string>();
		unlockedLevels.Add("PrototypeLevel");
		unlockedLevels.Add("Level1");

		toggleWindow = false;
		hasFocus = false;

		//play = GetComponent<ButtonObject>();

		width = Screen.width;
		levelSelectWindow = new Rect((width/2 - ((width/2f))/2f),(Screen.height/10),width/2f,width/1.5f);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void LevelSelect(){
		toggleWindow = true;
		disableButtons();
	}

	private void disableButtons(){
		play.SetActive(false);
		levelSelect.SetActive(false);
		exit.SetActive(false);
	}

	private void enableButtons(){
		play.SetActive(true);
		levelSelect.SetActive(true);
		exit.SetActive(true);
	}

	void OnGUI(){
		if(toggleWindow){
			levelSelectWindow = GUI.Window (0, levelSelectWindow, window, "Level Select");
		}
	}

	void window(int windowID) {
		if (GUILayout.Button("Prototype Level")){
			Application.LoadLevel(1);
		}
		if (GUILayout.Button ("Level 1")){
			Application.LoadLevel (2);
		}
	}

	public void LevelSelectPopulate(){
		foreach (string s in unlockedLevels){

		}
	}

	public void RestartLevel(){
		Application.LoadLevel(Application.loadedLevel);
	}

	public void ExitLevel(){
		PlayerPrefs.SetInt ("Checkpoint", 0);
		Application.LoadLevel("MainMenu");
	}

	public void NewGame(){
		Application.LoadLevel("Level1");
	}

	public void ExitGame(){
		PlayerPrefs.SetInt ("Checkpoint", 0);
		Application.Quit();
	}

	public void SaveExit(){
		/*
		 * TODO: Save file */
		PlayerPrefs.SetInt ("Checkpoint", 0);
		Application.Quit ();
	}

	public void SaveContinue(){
		/*
		 * TODO: Save file */
		int level = Application.loadedLevel;


		//ORGANIZE LEVELS BY NUMBER, VERY IMPORTANT
		PlayerPrefs.SetInt ("Checkpoint", 0);
		level++;
		Application.LoadLevel(level);
	}

	public void SaveMenu(){
		/*
		 * TODO: Save file */
		PlayerPrefs.SetInt ("Checkpoint", 0);
		Application.LoadLevel ("MainMenu");
	}
}




//Add cancel button to level select menu