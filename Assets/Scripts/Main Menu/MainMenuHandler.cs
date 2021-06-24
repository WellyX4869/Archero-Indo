using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
	[SerializeField] Dropdown dropdown;

	public void StartLevel()
	{
		SceneManager.LoadScene("Sandbox");
	}

	public void ChangeDifficulty()
    {
		GameSettings.difficulty = dropdown.value;
	}

	public void Exit()
	{
		Debug.Break();
		Application.Quit();
	}
}
