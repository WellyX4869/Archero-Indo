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
        if(currentHp <= 0f)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        Invoke("BackHpFun", 0.5f);
    }

    void BackHpFun()
    {
        backHpHit = true;
    }
}
