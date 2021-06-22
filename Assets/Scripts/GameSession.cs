using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    // config EXP
    [Header("Player + EXP Config")]
    public GameObject itemEXP;
    public Transform itemExpParent;
    public float currentPlayerExp = 0;
    public float[] levelUpLimit;
    public float expAbsorbed = 10;
    public int maxPlayerLevel = 10;
    public int currentPlayerLevel = 0;

    public float enemyProjectileSpeed = 1f;
    public int currentLevel = 1;
    public int maxLevel = 10;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int gameStatusCount = FindObjectsOfType<GameSession>().Length;
        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
