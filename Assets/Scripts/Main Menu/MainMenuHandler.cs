using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
	public void StartLevel()
	{
		SceneManager.LoadScene("Game");
	}
	public void Exit()
	{
		Debug.Break();
		Application.Quit();
	}
}
