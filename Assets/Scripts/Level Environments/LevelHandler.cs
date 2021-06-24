using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public bool isLevelCleared = false;
    [SerializeField] GameObject canvasLose;
    [SerializeField] GameObject canvasWin;
    [SerializeField] GameObject slotCanvas;
    [SerializeField] GameObject joystick;
    CloseDoor closeDoor;
    OpenDoor openDoor;

    GameObject player;

    private void Start()
    {
        isLevelCleared = false;
      

        joystick.SetActive(true);
        slotCanvas.SetActive(false);
        canvasLose.SetActive(false);
        canvasWin.SetActive(false);
    }

    public void GetDoors()
    {
        closeDoor = FindObjectOfType<CloseDoor>();
        openDoor = FindObjectOfType<OpenDoor>();

        closeDoor.gameObject.SetActive(true);
        openDoor.gameObject.SetActive(false);
    }

    public void GetPlayer()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    public void LevelCleared()
    {
        isLevelCleared = true;
        closeDoor.gameObject.SetActive(false);
        openDoor.gameObject.SetActive(true);
    }

    public void GoToNextLevel()
    {
        var gameSession = FindObjectOfType<GameSession>();
        gameSession.currentLevel++;
        if(gameSession.currentLevel > gameSession.maxLevel)
        {
            WinGame();
        }
        else
        {
            PlayerData.Instance.currentHp = PlayerHpBar.Instance.currentHp;
            PlayerData.Instance.maxHp = PlayerHpBar.Instance.maxHp;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            FindObjectOfType<CameraMovement>().CameraNextRoom();
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        var gameSession = FindObjectOfType<GameSession>();
        gameSession.ResetGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayerLevelUp()
    {
        PlayerMovement.Instance.StopPlayer();
        joystick.gameObject.SetActive(false);
        slotCanvas.gameObject.SetActive(true);
    }

    public void PlayerAfterLevelUp()
    {
        slotCanvas.gameObject.SetActive(false);
        joystick.gameObject.SetActive(true);
        PlayerMovement.Instance.MovePlayer();
    }

    public void WinGame()
    {
        joystick.gameObject.SetActive(false);
        Time.timeScale = 0;
        canvasWin.gameObject.SetActive(true);
    }

    public void LoseGame()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        PlayerMovement.Instance.anim.SetTrigger("DEAD");
        yield return new WaitForSeconds(1.5f);
        joystick.gameObject.SetActive(false);
        Time.timeScale = 0;
        canvasLose.gameObject.SetActive(true);
    }
}
