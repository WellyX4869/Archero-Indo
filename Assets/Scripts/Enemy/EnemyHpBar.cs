using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider backHpSlider;
    public Transform enemy;
    public float maxHp = 1000f;
    public float currentHp = 1000f;
    public bool backHpHit;

    private Vector3 offset;

    private void Start()
    {
        backHpHit = false;
        offset = transform.position;
        int level = FindObjectOfType<GameSession>().currentLevel;
        currentHp += (level * 40f);
    }

    private void Update()
    {
        transform.position = enemy.position;
        hpSlider.value = Mathf.Lerp(hpSlider.value, currentHp / maxHp, Time.deltaTime * 5f);

        if (backHpHit)
        {
            backHpSlider.value = Mathf.Lerp(backHpSlider.value, hpSlider.value, Time.deltaTime * 10f);
            if (hpSlider.value >= backHpSlider.value - 0.01f)
            {
                backHpHit = false;
                backHpSlider.value = hpSlider.value;
            }
        }
    }

    public void GetAttacked(float damage)
    {
        currentHp -= damage;
        Invoke("BackHpFun", 0.5f);
        if (currentHp <= 0f)
        {
            // Spawn EXP
            Vector3 currentPos = new Vector3(transform.position.x, 3f, transform.position.z);
            var gameSession = FindObjectOfType<GameSession>();
            for(int i = 0; i < (gameSession.currentPlayerLevel/10 + 2 + Random.Range(0,3)); i++)
            {
                var expClone = Instantiate(gameSession.itemEXP, currentPos, transform.rotation);
                expClone.transform.parent = gameSession.itemExpParent;
            }
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    void BackHpFun()
    {
        backHpHit = true;
    }
}
