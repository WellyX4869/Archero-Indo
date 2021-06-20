using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 5f);
    }

    public void DisplayDamage(float damage, bool isCritical)
    {
        var damageText = GetComponent<TextMesh>();
        damageText.text = "-"+damage.ToString();
        if (isCritical)
            damageText.color = Color.red;
        else
            damageText.color = Color.white;
    }
}
