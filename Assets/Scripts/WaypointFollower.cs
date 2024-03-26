using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
   [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    private Vector3 destination;

    public float speed = 1f;

    public bool playerOn;

    void FixedUpdate()
    {
        if(playerOn)
        {
            if(Vector3.Distance(transform.position, GetDestination()) < 0.1f)
            {
                currentWaypointIndex++;

                if(currentWaypointIndex >= waypoints.Length)
                    currentWaypointIndex = 0;
            }
        }


        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOn = true;
            collision.transform.SetParent(transform);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOn = false;
            collision.transform.SetParent(null);

        }
    }

    public Vector3 GetDestination()
    {
        destination = waypoints[currentWaypointIndex].transform.position;
        return destination;
    }
}
