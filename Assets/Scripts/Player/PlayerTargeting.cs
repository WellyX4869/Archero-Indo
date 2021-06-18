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
    public bool isAttackDebug = true;

    [Header("Projectile Config")]
    public Transform projectileSpawnPoint = null;
    public float projectileSpeed = 10f;
    public float attackSpeed = 1f;
    public Transform projectileParent = null;
    
    [HideInInspector] public List<GameObject> MonsterList = new List<GameObject>();
    private static PlayerTargeting instance;
    Vector3 currentRotation;
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

    private void Update()
    {
        if (MonsterList.Count <= 0)
        {
            return;
        }

        GetTarget();
        SetTarget();
        if (isAttackDebug)
            AttackTarget();
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

    #region UPDATE_FUNCTION
    void AddEnemies()
    {
        var enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            MonsterList.Add(enemy.gameObject);
        }
    }

    private void GetTarget()
    {
        var enemies = FindObjectsOfType<EnemyController>();
        if(enemies.Length != MonsterList.Count)
        {
            MonsterList.Clear();
            AddEnemies();
        }
        
        if(MonsterList.Count <= 0)
        {
            var playerState = playerMovement.playerState;
            playerState = PlayerState.idle;
            playerMovement.anim.SetBool("ATTACK", false);
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

    // IT IS CALLED WITHIN ANIMATION EVENT OF THROWING/SHOOT
    public void Attack()
    {
        playerMovement.anim.SetFloat("AttackSpeed", attackSpeed);

        // Spawn and Shoot projectile
        currentRotation = transform.eulerAngles;
        ShootProjectile();

        // PlayerSkill[1] = MultiShot
        if (PlayerData.Instance.PlayerSkill[1] != 0)
        {
            Invoke("ShootProjectile", 0.2f);
        }
    }
    private void ShootProjectile()
    {
        if (playerMovement.playerState == PlayerState.attack)
        {
            #region forwardProjectilesFailed
            //float offsetX = 1.25f;
            //int forwardProjectiles = PlayerData.Instance.PlayerSkill[2];
            //for(int i = 0; i <= forwardProjectiles; i++)
            //{
            //    Vector3 spawnPos = Vector3.zero;
            //    if(forwardProjectiles == 0)
            //    {
            //        spawnPos = projectileSpawnPoint.position;
            //    }
            //    else
            //    {
            //        spawnPos = projectileSpawnPoint.position;
            //        spawnPos.x += offsetX * (-2) * i;
            //    }
            //    InstantiateProjectile(spawnPos);
            //}
            #endregion
            InstantiateProjectile();
        }
    }

    private void InstantiateProjectile()
    {
        int forwardProjectiles = PlayerData.Instance.PlayerSkill[2];
        var projectile = Instantiate(PlayerData.Instance.playerProjectiles[forwardProjectiles], 
                                    projectileSpawnPoint.position, 
                                    Quaternion.Euler(currentRotation));
        projectile.transform.parent = projectileParent;

        #region SKILL FORWARD + 1 => ISPLAYERPROJECTILE
        if (forwardProjectiles > 0)
        {
            var projectileComponents = projectile.GetComponentsInChildren<Projectile>();
            foreach (Projectile proj in projectileComponents)
            {
                proj.isPlayerProjectile = true;
            }
        }
        else if(forwardProjectiles == 0)
        {
            projectile.GetComponent<Projectile>().isPlayerProjectile = true;
        }
        #endregion

        #region SKILL DIAGONAL + 1 => ISPLAYERPROJECTILE
        int diagonalProjectiles = PlayerData.Instance.PlayerSkill[3];
        if (diagonalProjectiles > 0)
        {
            DiagonalProjectiles(diagonalProjectiles);
        }
        #endregion
    }

    private void DiagonalProjectiles(int diagonalProjectiles)
    {
        // DIAGONAL LEFT PROJECTILE
        var projectileLeft = Instantiate(PlayerData.Instance.playerProjectiles[diagonalProjectiles - 1],
                                projectileSpawnPoint.position,
                                Quaternion.Euler(currentRotation + new Vector3(0, -45f, 0)));
        projectileLeft.transform.parent = projectileParent;

        // DIAGONAL RIGHT PROJECTILE
        var projectileRight = Instantiate(PlayerData.Instance.playerProjectiles[diagonalProjectiles - 1],
                                projectileSpawnPoint.position,
                                Quaternion.Euler(currentRotation + new Vector3(0, 45f, 0)));
        projectileRight.transform.parent = projectileParent;

        if (diagonalProjectiles > 1)
        {
            var projectileLeftComponents = projectileLeft.GetComponentsInChildren<Projectile>();
            foreach (Projectile proj in projectileLeftComponents)
            {
                proj.isPlayerProjectile = true;
            }
            var projectileRightComponents = projectileRight.GetComponentsInChildren<Projectile>();
            foreach (Projectile proj in projectileRightComponents)
            {
                proj.isPlayerProjectile = true;
            }
        }
        else if(diagonalProjectiles == 1)
        {
            projectileLeft.GetComponent<Projectile>().isPlayerProjectile = true;
            projectileRight.GetComponent<Projectile>().isPlayerProjectile = true;
        }
    }
    #endregion
}
