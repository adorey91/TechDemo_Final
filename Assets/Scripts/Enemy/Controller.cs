using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] float rotationSpeed;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void LookTowards(Vector3 direction)
    {
        Vector3 lookDirection = direction - agent.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void MoveToTarget(Transform target)
    {
        MoveToPosition(target.position);
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
        LookTowards(position);
    }

    public void StopMovement()
    {
        agent.isStopped = true;
    }

    public void RunAway(float enemyDistanceRun, Transform target)
    {
            Vector3 dirToPlayer = transform.position - target.position;
            Vector3 newPos = transform.position + dirToPlayer.normalized * enemyDistanceRun;
            MoveToPosition(newPos);
       
    }
}
