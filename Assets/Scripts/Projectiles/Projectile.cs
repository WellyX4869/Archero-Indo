using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float delayUntilDestroyed = 3f;
    public bool isPlayerProjectile = false;
    public bool isBouncy = false;
    public float damage = 100f;
    [HideInInspector] public Rigidbody rb;

    #region HIDDEN_PRIVATE_MEMBER_VAR
    // HIDDEN PRIVATE MEMBER VARIABLES
    int bounceTimes = 3;
    int ricochetTimes = 2;
    private float ricochetRange = 35f;
    private float projectileSpeed = 2000f;
    private List<int> enemyIndexes = new List<int>();
    Vector3 newDir;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        newDir = transform.forward;

        // SETUP PLAYER PROJECTILE
        if (isPlayerProjectile)
        {
            projectileSpeed = PlayerTargeting.Instance.projectileSpeed + (200f * PlayerData.Instance.PlayerSkill[9]);
            damage = PlayerData.Instance.damage;
            rb.AddForce(transform.forward * projectileSpeed);
        }

        if (isBouncy)
        {
            projectileSpeed = transform.parent.GetComponentInChildren<EnemyController>().projectileSpeed;
            rb.velocity = newDir;
        }
        else
        {
            Destroy(gameObject, delayUntilDestroyed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 startPos = transform.position;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 20f;
        Gizmos.DrawRay(startPos, direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayerProjectile)
        {
            if (collision.transform.CompareTag("Wall"))
            {
                if(PlayerData.Instance.PlayerSkill[4] != 0)
                {
                    if(bounceTimes > 0)
                    {
                        bounceTimes--;
                        damage *= 0.7f;
                        Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.contacts[0].normal).normalized;
                        EulerDirection(reflectDir);
                        rb.AddForce(transform.eulerAngles * projectileSpeed);
                        return;
                    }
                }
                Destroy(gameObject);
            }
        }
        else
        {
            if (!isBouncy && collision.gameObject.tag != "Projectile")
            {
                Destroy(gameObject);
            }
            else
            {
                if (collision.gameObject.CompareTag("Wall") && bounceTimes > 0)
                {
                    bounceTimes--;
                    Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.contacts[0].normal).normalized;
                    EulerDirection(reflectDir);
                    rb.AddForce(transform.eulerAngles * projectileSpeed);
                }

                if (collision.gameObject.tag != "Wall" || bounceTimes <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // PROJECTILE PLAYER TO ENEMY
        if(isPlayerProjectile && other.transform.CompareTag("Monster"))
        {
            HitEnemy(other);
            // PlayerSkill[0] = Richocet
            if (PlayerData.Instance.PlayerSkill[0] != 0 && PlayerTargeting.Instance.MonsterList.Count >= 2)
            {
                int myIndex = PlayerTargeting.Instance.MonsterList.IndexOf(other.gameObject);
                if(ricochetTimes > 0)
                {
                    ricochetTimes--;
                    damage *= 0.7f;
                    newDir = RicochetDirection(myIndex);
                    EulerDirection(newDir);
                    rb.velocity = newDir;
                    rb.AddForce(transform.forward * projectileSpeed);
                    return;
                }
            }
            rb.velocity = Vector3.zero;
            Destroy(gameObject);
        }
        else if(isPlayerProjectile && other.transform.CompareTag("Wall"))
        {
            if(PlayerData.Instance.PlayerSkill[4] == 0)
            {
                rb.velocity = Vector3.zero;
                Destroy(gameObject);
            }
        }

        // PROJECTILE ENEMY TO PLAYER
        if (!isPlayerProjectile && other.transform.CompareTag("Player"))
        {
            HitPlayer(other);
            Destroy(gameObject);
        }
    }

    private void EulerDirection(Vector3 direction)
    {
        float rot = 90 - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, rot, 0);
    }
    Vector3 RicochetDirection(int index)
    {
        int closestIndex = -1;
        float closestDistance = 500f;
        float currentDistance = 0f;

        enemyIndexes.Add(index);

        for (int i = 0; i < PlayerTargeting.Instance.MonsterList.Count; i++)
        {
            // Check if already hit that enemy
            bool isAlreadyHit = false;
            for (int j = 0; j < enemyIndexes.Count; j++)
            {
                if (i == enemyIndexes[j])
                {
                    isAlreadyHit = true;
                    break;
                }
            }
            if (isAlreadyHit) continue;

            currentDistance = Vector3.Distance(PlayerTargeting.Instance.MonsterList[i].transform.position, transform.position);
            if (currentDistance > ricochetRange) continue;

            if (closestDistance > currentDistance)
            {
                closestDistance = currentDistance;
                closestIndex = i;
            }
        }

        if (closestIndex == -1)
        {
            Destroy(gameObject);
            return Vector3.zero;
        }
        return (PlayerTargeting.Instance.MonsterList[closestIndex].transform.position - transform.position).normalized;
    }

    private void HitPlayer(Collider player)
    {
        var playerHP = FindObjectOfType<PlayerHpBar>();
        playerHP.GetAttacked((int)(Mathf.Floor(damage)));
    }
    private void HitEnemy(Collider enemy)
    {
        // Spawn Floating Damage Text
        var enemyPos = enemy.transform.position;
        enemyPos.y += 20f;
        var damageText = Instantiate(EffectSet.Instance.MonsterDmgText, enemyPos, Quaternion.identity) as GameObject;
        float damageCrit = damage;
        bool isCritical = false;
        if(Random.value > PlayerData.Instance.critRNG) // damage is critical
        {
            float critDamage = PlayerData.Instance.PlayerSkill[11] * 40f;
            damageCrit = (damage*2) + critDamage;
            isCritical = true;
        }
        damageText.GetComponent<DamageText>().DisplayDamage(damageCrit, isCritical);

        // HitEnemy With Damage
        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController.enemyCanvasGo.GetComponent<EnemyHpBar>().GetAttacked(damageCrit);
    }
}
