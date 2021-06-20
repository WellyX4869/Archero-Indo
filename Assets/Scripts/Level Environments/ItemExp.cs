using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExp : MonoBehaviour
{
    LevelHandler levelHandler;

    // Start is called before the first frame update
    void Start()
    {
        levelHandler = FindObjectOfType<LevelHandler>();
        StartCoroutine(WaitLevelCleared());
    }

    IEnumerator WaitLevelCleared()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            while (levelHandler.isLevelCleared)
            {
                transform.position = Vector3.Lerp(transform.position, PlayerTargeting.Instance.transform.position, 0.2f);
                yield return null;
            }
        }
    }
}
