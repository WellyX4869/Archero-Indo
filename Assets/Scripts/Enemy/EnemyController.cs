using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController: MonoBehaviour
{

    #region PRIVATE_VAR
    string currentStateName;
    public readonly IdleState idleState = new IdleState();
    State currentState;

    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject player = null;
    [HideInInspector] public float hitLength;
    [HideInInspector] public float exitTime = 0.625f;
    [HideInInspector] public bool firstAttack = true;
    #endregion

    #region PUBLIC_VAR
    [Header("Related GameObjects")]
    public GameObject enemyCanvasGo;
    public TMP_Text stateText = null;

    [Header("Enemy Config")]
    public bool isIdleDebug = false;
    public float attackRange = 20f;
    public float attackCooldown = 2f;
    public int damage = 100;

    [Header("Enemy Ranged Config")]
    public bool isRanged;
    [Range(1,2)]
    public int rangedType = 1;
    LineRenderer lineRend = null;
    public float lineRendWidth = 1f;
    public Rigidbody enemyProjectile = null;
    public float projectileSpeed = 100f;
    public Transform projectileSpawnPoint = null;
    public LayerMask layerMask;
    #endregion

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if(isRanged)
        {
            SetUpLineRenderer();
            hitLength = GetAnimationClipLength("throw");
        }
        else
        {
            firstAttack = true;
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.black;

        Vector3 startPos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.forward) * attackRange;
        Gizmos.DrawRay(startPos, direction);
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

    #region MELEE_FUNCTION
    public void MeleeHit()
    {
        if (IsPlayerWithinAttackRange())
        {
            player.GetComponent<PlayerHpBar>().GetAttacked(damage);
        }
    }
    #endregion

    #region RANGED_FUNCTION
    private void SetUpLineRenderer()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.startColor = new Color(1, 0, 0, 0.5f);
        lineRend.endColor = new Color(1, 0, 0, 0.5f);
        lineRend.startWidth = lineRendWidth;
        lineRend.endWidth = lineRendWidth;
    }
    public void ShootDangerMarker1()
    {
        transform.LookAt(player.transform.position);
        //Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        //RaycastHit hit;
        //Physics.Raycast(NewPosition, transform.forward, out hit, 500f, layerMask);

        //GameObject DangerMarkerClone = Instantiate(DangerMarker, NewPosition, transform.rotation);
        //DangerMarkerClone.GetComponent<DangerLine>().EndPosition = hit.point;
        transform.LookAt(player.transform.position);
        Vector3 newPos = projectileSpawnPoint.position;
        float y = newPos.y;
        Vector3 newDir = transform.forward;
        lineRend.positionCount = 1;
        Vector3 startPos = new Vector3(transform.position.x, 1f, transform.position.z);
        lineRend.SetPosition(0, startPos);
       
        RaycastHit hit;
        Physics.Raycast(newPos, newDir, out hit, 500f, layerMask);
        lineRend.positionCount++;
        newPos = new Vector3(hit.point.x, y, hit.point.z);
        lineRend.SetPosition(1, newPos);
        newDir = Vector3.Reflect(newDir, hit.normal);
    }
    public void ShootDangerMarker2()
    {
        transform.LookAt(player.transform.position);
        Vector3 newPos = projectileSpawnPoint.position;
        float y = newPos.y;
        Vector3 newDir = transform.forward;
        lineRend.positionCount = 1;
        lineRend.SetPosition(0, transform.position);
        for(int i = 1; i<4; i++)
        {
            RaycastHit hit;
            Physics.Raycast(newPos, newDir, out hit, 500f, layerMask);
            lineRend.positionCount++;
            newPos = new Vector3(hit.point.x, y, hit.point.z);
            lineRend.SetPosition(i, newPos);
            newDir = Vector3.Reflect(newDir, hit.normal);
        }
    }
    public void DangerMarkerDeactivate()
    {
        lineRend.positionCount = 0;
    }
    public float GetAnimationClipLength(string clipName)
    {
        float time = 0f;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == clipName)        //If it has the same name as your clip
            {
                time = ac.animationClips[i].length;
            }
        }
        return time;
    }
    public void ShootProjectile()
    {
        Vector3 currentRotation = transform.eulerAngles;
        var projectile = Instantiate(enemyProjectile, projectileSpawnPoint.position, Quaternion.Euler(currentRotation)) as Rigidbody;
        projectile.transform.parent = transform.parent;
        projectile.AddForce(transform.forward * projectileSpeed);
    }
    #endregion
}
