using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting Instance // singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerTargeting>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerTargeting");
                    instance = instanceContainer.AddComponent<PlayerTargeting>();
                }
            }
            return instance;
        }
    }


    public bool getATarget = false;
    public LayerMask enemyMask;
    public LayerMask obstacleMask;

    public List<GameObject> MonsterList = new List<GameObject>();

    [Header("Projectile Config")]
    public Rigidbody playerProjectile = null;
    public Transform projectileSpawnPoint = null;
    public float projectileSpeed = 10f;
    public float attackSpeed = 1f;
    public Transform projectileParent = null;

    private static PlayerTargeting instance;
    float currentDist = 0;
    float closestDist = 100f;
    float TargetDist = 100f;
    int closeDistIndex = 0;
    public int targetIndex = -1;
    PlayerMovement playerMovement;
  
    private void Start()
    {
        AddEnemies();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void AddEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Monster");
        foreach(GameObject enemy in enemies)
        {
            MonsterList.Add(enemy);
        }
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i<MonsterList.Count; i++)
        {
            RaycastHit hit;
            Vector3 direction = MonsterList[i].transform.position - transform.position;
            float distToTarget = Vector3.Distance(MonsterList[i].transform.position, transform.position);
            bool isObstacleHit = Physics.Raycast(transform.position, direction, out hit, distToTarget, obstacleMask);
                
            if(!isObstacleHit)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawRay(transform.position, direction);
        }
    }

    private void Update()
    {
        GetTarget();
        SetTarget();
        AttackTarget();
    }

    private void GetTarget()
    {
        var enemies = FindObjectsOfType<EnemyController>();
        if(enemies.Length != MonsterList.Count)
        {
            MonsterList.Clear();
            AddEnemies();
        }
    }

    private void SetTarget()
    {
        if (MonsterList.Count > 0)
        {
            currentDist = 0;
            closeDistIndex = 0;
            targetIndex = -1;
            closestDist = 100f;
            TargetDist = 100f;

            for (int i = 0; i < MonsterList.Count; i++)
            {
                currentDist = Vector3.Distance(MonsterList[i].transform.position, transform.position);

                RaycastHit hit;
                bool isObstacleHit = Physics.Raycast(transform.position, MonsterList[i].transform.position - transform.position, out hit, currentDist, obstacleMask);

                // pengecekan ray nabrak tembok atau tidak
                if (!isObstacleHit)
                {
                    //if(Physics.Raycast(transform.position, MonsterList[i].transform.position - transform.position, out hit, currentDist, enemyMask))
                    //{
                    //    //hit enemy
                    //}
                    if (currentDist <= TargetDist)
                    {
                        targetIndex = i;
                        TargetDist = currentDist;
                    }
                }

                if (currentDist < closestDist)
                {
                    closeDistIndex = i;
                    closestDist = currentDist;
                }
            }

            if (targetIndex == -1)
            {
                targetIndex = closeDistIndex;
            }

            getATarget = true;
        }
    }

    private void AttackTarget()
    {
        if (MonsterList.Count > 0)
        {
            var playerState = playerMovement.playerState;
            if (getATarget && playerState != PlayerState.walk)
            {
                transform.LookAt(new Vector3(MonsterList[targetIndex].transform.position.x, transform.position.y, MonsterList[targetIndex].transform.position.z));
                //Set animation Attack
                playerMovement.playerState = PlayerState.attack;
                playerMovement.anim.SetBool("ATTACK", true);
            }
            else
            {
                playerState = PlayerState.idle;
                playerMovement.anim.SetBool("ATTACK", false);
            }
        }
    }

    public void Attack()
    {
        //Spawn projectile
        playerMovement.anim.SetFloat("AttackSpeed", attackSpeed);
        var projectile = Instantiate(playerProjectile, projectileSpawnPoint.position, Quaternion.identity) as Rigidbody;
        projectile.gameObject.transform.parent = projectileParent;
        projectile.AddForce(transform.forward * projectileSpeed);
    }
}
