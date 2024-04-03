using System;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Unity.VisualScripting;

public enum State
{
    Patrolling,
    Chasing,
    Searching,
    Attacking,
    Retreating
}

public class Enemy : Character
{
    private Controller controller;

    [Header("Enemy Settings")]
    [SerializeField] HealthBarUI healthBarUI;
    [SerializeField] float walkSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] int healthPanic;
    [SerializeField] float enemyDistanceRun;
    NavMeshAgent agent;

    [Header("Target Settings")]
    Vector3 targetPosition;
    Vector3 lastKnownPlayerPosition;
    float targetDistance;

    [Header("State Settings")]
    public State currentState;

    [Header("Patroling")]
   // [SerializeField] Vector3[] wayPointPos = new Vector3[4];
    [SerializeField] Transform[] wayPointPos = new Transform[4];
    [SerializeField] int wayPointInc;
    Vector3 lastPosition;
    float distanceThreshold = 0.1f;

    [Header("Chasing")]
    [SerializeField] float chaseRange = 12;

    [Header("Searching")]
    [SerializeField] private float searchDuration = 5f;
    private float searchTimer;

    [Header("Attacking")]
    [SerializeField] float attackRange = 5f;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] float timeBetweenAttacks;
    bool alreadyAttacked;

    [Header("RunAway")]
    [SerializeField] float healthIncreaseTime;
    float runTimer;
    bool healed;

    public void Start()
    {
        curHp = maxHp;
        healthBarUI.Start();
        controller = GetComponent<Controller>();
        target = Player.current;
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        UpdateStats();

        if (curHp <= healthPanic)
        {
            healed = false;
            AgentSetup(runSpeed, Color.white, "Running Away & Healing");
            if (runTimer > 0)
            {
                if (targetDistance < enemyDistanceRun)
                    controller.RunAway(enemyDistanceRun, target.transform);
                else
                    Debug.Log("Not running away - target distance is greater than enemyDistanceRun");
            }
            else
            {
                Heal();
                healed = true;
            }

            runTimer -= Time.deltaTime;
            currentState = State.Retreating;
        }
        else
        {
            switch (currentState)
            {
                case State.Patrolling:
                    PatrollingUpdate();
                    break;
                case State.Chasing:
                    ChasingUpdate();
                    break;
                case State.Searching:
                    SearchingUpdate();
                    break;
                case State.Attacking:
                    AttackingUpdate();
                    break;
                case State.Retreating:
                    RetreatingUpdate();
                    break;
            }
        }
    }

    private void UpdateStats()
    {
        targetPosition = target.transform.position;
        targetDistance = Vector3.Distance(transform.position, target.transform.position);
        healthBarUI.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        if (healed == true)
        {
            healed = false;
            runTimer = healthIncreaseTime;
        }
    }

    private void PatrollingUpdate()
    {
        AgentSetup(walkSpeed, Color.blue, "Patroling");
        controller.MoveToPosition(wayPointPos[wayPointInc].position);

        if (Vector3.Distance(transform.position, wayPointPos[wayPointInc].position) <= distanceThreshold)
        {
            wayPointInc++;
            if (wayPointInc == wayPointPos.Length)
                wayPointInc = 0;
        }

        if (targetDistance <= chaseRange)
        {
            lastPosition = transform.position;
            currentState = State.Chasing;
        }
    }

    private void ChasingUpdate()
    {
        AgentSetup(chaseSpeed, new Color(1, 0.6f, 0), "Chasing");
        controller.MoveToTarget(target.transform);
        lastKnownPlayerPosition = targetPosition;

        if (targetDistance >= chaseRange)
        {
            searchTimer = searchDuration;
            currentState = State.Searching;
        }
        if (targetDistance < attackRange)
            currentState = State.Attacking;
    }

    private void SearchingUpdate()
    {
        AgentSetup(walkSpeed, Color.gray, "Searching");

        if (Vector3.Distance(transform.position, lastKnownPlayerPosition) <= distanceThreshold)
            searchTimer -= Time.deltaTime;

        if (searchTimer <= 0)
            currentState = State.Retreating;
        else if (targetDistance <= attackRange)
            currentState = State.Attacking;
        else if (targetDistance <= chaseRange)
            currentState = State.Chasing;
        else
            controller.MoveToPosition(lastKnownPlayerPosition);
    }

    private void AttackingUpdate()
    {
        AgentSetup(0, Color.red, "Attacking");
        controller.StopMovement();
        controller.LookTowards(targetPosition);

        if (!alreadyAttacked)
        {
            Vector3 directionToTarget = targetPosition - transform.position;
            directionToTarget.Normalize();
            Vector3 spawnPosition = transform.position + directionToTarget * 1f;

            GameObject proj = Instantiate(attackPrefab, spawnPosition, Quaternion.LookRotation(targetPosition - transform.position));
            proj.GetComponent<Projectile>().Setup(this);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        if (targetDistance >= attackRange)
        {
            if (targetDistance >= chaseRange)
            {
                searchTimer = searchDuration;
                currentState = State.Searching;
            }
            if (targetDistance <= chaseRange)
                currentState = State.Chasing;
        }
    }

    private void RetreatingUpdate()
    {
        AgentSetup(runSpeed, Color.cyan, "Retreating");
        controller.MoveToPosition(lastPosition);

        if (Vector3.Distance(transform.position, lastPosition) <= distanceThreshold)
            currentState = State.Patrolling;
        if (targetDistance <= chaseRange)
            currentState = State.Chasing;
    }

    public void AgentSetup(float moveSpeed, Color color, string enemyState)
    {
        agent.speed = moveSpeed;
        mesh.material.color = color;
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
