using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerData>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("PlayerData");
                    instance = instanceContainer.AddComponent<PlayerData>();
                }
            }
            return instance;
        }
    }
    private static PlayerData instance;

    public float damage = 50;
    public float damageIncrease = 50f;
    public float critRNG = 0.8f;
    public List<int> PlayerSkill = new List<int>();
    public List<int> PlayerSkillLimit = new List<int>();
    public GameObject[] playerProjectiles;
    public float currentHp = 1000;
    public float maxHp = 1000;

    /*
    PlayerSkill[0] = Ricochet
    PlayerSkill[1] = MultiShot
    PlayerSkill[2] = Forward Projectiles + 1
    PlayerSkill[3] = Diagonal Projectiles + 1
    PlayerSkill[4] = Bouncy Wall
    PlayerSkill[5] = Rear Arrow + 1
    PlayerSkill[6] = Side Arrow + 1
    PlayerSkill[7] = Level Up Increase
    PlayerSkill[8] = Attack Boost
    PlayerSkill[9] = Attack Speed Boost
    PlayerSkill[10] = Health Boost
    PlayerSkill[11] = Critical Boost
    */
    public void AttackBoost()
    {
        damage += damageIncrease;
    }

    public void CriticalBoost()
    {
        critRNG -= 0.05f;
    }
}
