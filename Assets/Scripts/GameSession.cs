using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    // config params
    [SerializeField] TMP_Text coinText;
    [SerializeField] int expPerEnemyDestroyed = 10;

    // state variables
    [SerializeField] int currentCoin = 0;
    [SerializeField] float currentPlayerExp = 0;

    // public var
    public float enemyProjectileSpeed = 1f;

    private void Awake()
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


    // Start is called before the first frame update
    void Start()
    {
        coinText.text = currentCoin.ToString();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin(int coins)
    {
        currentCoin += coins;
        coinText.text = currentCoin.ToString();
    }
}
