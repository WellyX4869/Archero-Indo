using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSessionData : MonoBehaviour
{
    GameSession data;
    public Text currentLevelValue;

    private void OnEnable()
    {
        data = FindObjectOfType<GameSession>();
        currentLevelValue.text = data.currentLevel.ToString();
    }
 
}
