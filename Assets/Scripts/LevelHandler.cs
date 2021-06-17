using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public bool isLevelCleared = false;
    public GameObject canvasLose;
    public GameObject canvasWin;
    [SerializeField] GameObject joystick;

    private void Start()
    {
        canvasLose.gameObject.SetActive(false);
        canvasWin.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        canvasLose.gameObject.SetActive(true);
        joystick.gameObject.SetActive(false);

    }

}
