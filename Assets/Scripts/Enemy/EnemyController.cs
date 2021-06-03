using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController: MonoBehaviour
{
    [SerializeField] string currentStateName;
    public readonly IdleState idleState = new IdleState();
    State currentState;

    public Rigidbody rigidBody;
    public NavMeshAgent navAgent;
    public Animator animator;

    public GameObject enemyCanvasGo;
    public TMP_Text stateText = null;
    public GameObject player = null;

    [Header("Enemy Config")]
    public float attackRange = 20f;
    public float attackCooldown = 2f;
    public int damage = 100;

    [Header("Enemy Ranged Config")]
    public bool isRanged;
    public GameObject DangerMarker = null;
    public Rigidbody enemyProjectile = null;
    public float projectileSpeed = 100f;
    public Transform enemyProjectileSpawnPoint = null;
    public LayerMask layerMask;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        TransitionToState(idleState);
    }

    private void Update()
    {
        currentState.Update(this);
        currentStateName = currentState.ToString();
        stateText.text = currentStateName;
    }

    public void TransitionToState(State state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {
            if(collision.gameObject == null)
            {
                return;
            }
            enemyCanvasGo.GetComponent<EnemyHpBar>().GetAttacked(collision.gameObject.GetComponent<Projectile>().damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public bool IsPlayerWithinAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            return true;
        }
        return false;
    }

    public void ShootDangerMarker()
    {
        transform.LookAt(player.transform.position);
        Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        RaycastHit hit;
        Physics.Raycast(NewPosition, transform.forward, out hit, 500f, layerMask);
      
        GameObject DangerMarkerClone = Instantiate(DangerMarker, NewPosition, transform.rotation);
        DangerMarkerClone.GetComponent<DangerLine>().EndPosition = hit.point;
    }

    public void ShootProjectile()
    {
        transform.LookAt(player.transform.position);
        Vector3 CurrentRotation = transform.eulerAngles;
        var projectile = Instantiate(enemyProjectile, enemyProjectileSpawnPoint.position, Quaternion.Euler(CurrentRotation)) as Rigidbody;
        projectile.AddForce(transform.forward * projectileSpeed);
    }
}
