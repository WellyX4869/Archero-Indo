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
    public List<int> PlayerSkill = new List<int>();
    public GameObject[] playerProjectiles;

    /*
    PlayerSkill[0] = Ricochet
    PlayerSkill[1] = MultiShot
    PlayerSkill[2] = Forward Projectiles + 1
    PlayerSkill[3] = Diagonal Projectiles + 1
    PlayerSkill[4] = Bouncy Wall
    */
}
