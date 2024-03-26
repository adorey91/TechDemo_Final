using System;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


public abstract class Enemy : Character
{
    public enum State
    {
        Patrolling,
        Chasing,
        Attacking,
        Dead,
    }

    private Controller controller;

    [Header("Enemy Settings")]
    [SerializeField] HealthBarUI healthBarUI;
    [SerializeField] float walkSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] int healthPanic;
    NavMeshAgent agent;

    [Header("Target Settings")]
    Vector3 targetPosition;
    float targetDistance;

    public void Start()
    {
        target = Player.current;
    }
}