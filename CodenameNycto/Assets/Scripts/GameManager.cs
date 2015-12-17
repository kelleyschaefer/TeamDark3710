using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject Level1Button;
	public Rect LevelSelectWindow;
	public GameObject play;
	public GameObject levelSelect;
	public GameObject exit;
	
	private bool toggleWindow;
	private float width;
	private List<string> unlockedLevels;
	private Rect levelSelectWindow;
	private Rect pauseWindow;
	private bool paused;

	// Use this for initializations
	void Start () {
		unlockedLevels = new List <string>();
		//unlockedLevels.Add("PrototypeLevel");
		//unlockedLevels.Add("Level1");
		unlockedLevels.Add("Tutorial");

		toggleWindow = false;
		paused = false;

		width = Screen.width;
		levelSelectWindow = new Rect((width/2 - ((width/3f))/2f),(Screen.height/8f),width/3f,width/4f);
		pauseWindow = new Rect((width/2 - ((width/3f))/2f),(Screen.height/8f),width/3f,width/4f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName != "MainMenu"){
			//pause behavior 
			if (Input.GetKeyDown(KeyCode.Escape)){
				Pause ();
			}
		}
	}

	private void Pause(){
		if (paused){
			paused = false;
			Time.timeScale = 1.0f;
		}
		else {
			paused = true;
			Time.timeScale = 0.0f;
		}
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
			levelSelectWindow = GUI.Window (0, levelSelectWindow, levelWindowUI, "Level Select");
		}

		if(paused){
			pauseWindow = GUI.Window (0, levelSelectWindow, pauseWindowUI, "Level Select");
		}
	}

	void levelWindowUI(int windowID) {
		if (GUILayout.Button("Tutorial")){
			Application.LoadLevel(1);
		}
		if (GUILayout.Button ("Castle Level 1")){
			Application.LoadLevel (2);
		}
        if (GUILayout.Button("Castle Level 2"))
        {
            Application.LoadLevel(3);
        }
        if (GUILayout.Button("Castle Level 3"))
        {
            Application.LoadLevel(4);
        }
        if (GUILayout.Button("Library Level 1"))
        {
            Application.LoadLevel(5);
        }
        if (GUILayout.Button("Library Level 2"))
        {
            Application.LoadLevel(6);
        }
        if (GUILayout.Button("Library Level 3"))
        {
            Application.LoadLevel(7);
        }
        if (GUILayout.Button ("Cancel")){
			Application.LoadLevel ("MainMenu");
		}
	}

	void pauseWindowUI(int windowID){
		if (GUILayout.Button ("Restart")){
			Pause ();
			RestartLevel();
		}
		if (GUILayout.Button ("Quit")){
			Pause ();
			Application.LoadLevel ("MainMenu");
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
	public void NewGame()
	{
		PlayerPrefs.SetInt("Checkpoint", 0);
		Application.LoadLevel("Tutorial");
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



//Add pause menu
//Add save
//Add load
//Add levels beaten functionality 
