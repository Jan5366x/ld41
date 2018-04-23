using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
	public string GameScene = "MainGame";
	public string CreditsScene = "Credits";
	
	public void StartGame()
	{
		SceneManager.LoadScene(GameScene);
	}

	public void ShowCredits()
	{
		SceneManager.LoadScene(CreditsScene);
	}
	
	public void ExitGame()
	{
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
