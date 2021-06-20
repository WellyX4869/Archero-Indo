using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSet : MonoBehaviour
{
    public static EffectSet Instance // singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectSet>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("EffectSet");
                    instance = instanceContainer.AddComponent<EffectSet>();
                }
            }
            return instance;
        }
    }

    private static EffectSet instance;

    [Header("Monster")]
    public GameObject MonsterDmgText;

    [Header("Player")]
    public GameObject PlayerLevelUpEffect;
}
