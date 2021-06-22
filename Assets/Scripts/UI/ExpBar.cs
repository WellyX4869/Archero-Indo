using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public static ExpBar Instance // singleton     
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ExpBar>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("ExpBar");
                    instance = instanceContainer.AddComponent<ExpBar>();
                }
            }
            return instance;
        }
    }

    private static ExpBar instance;
    GameSession gameSession;
    LevelHandler levelHandler;
    [SerializeField] Slider expBar;
    [SerializeField] Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        levelHandler = FindObjectOfType<LevelHandler>();
        CalculateExpBar();
    }
    
    public void AddExp()
    {
        gameSession.currentPlayerExp += gameSession.expAbsorbed;
        CalculateExpBar();
    }

    private void CalculateExpBar()
    {
        float treshold = 0;
        if (gameSession.currentPlayerLevel > 0)
            treshold = gameSession.levelUpLimit[gameSession.currentPlayerLevel - 1];
        else treshold = 0;
        
        float playerExp = gameSession.currentPlayerExp - treshold;
        float dividerPlayerExp = gameSession.levelUpLimit[gameSession.currentPlayerLevel] - treshold;
        expBar.value = playerExp / dividerPlayerExp;
        if(gameSession.currentPlayerLevel <= gameSession.maxPlayerLevel)
        {
            levelText.text = "LV." + (gameSession.currentPlayerLevel + 1);
        }
        else
        {
            levelText.text = "LV.MAX";
        }
        LevelUpChecker();
    }

    private void LevelUpChecker()
    {
        if (expBar.value >= 1 && gameSession.currentPlayerLevel <= gameSession.maxPlayerLevel)
        {
            levelHandler.PlayerLevelUp();
            gameSession.currentPlayerLevel++;
        }
    }
}
