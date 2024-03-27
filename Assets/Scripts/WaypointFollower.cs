using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    private Vector3 destination;
    [SerializeField] private int moveCount;

    public float speed = 1f;
    public bool playerOn;
    public bool startPlatform;

    private void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(-39.7f, 7.5f, 13.9f);
    }

    private void Update()
    {
        if (startPlatform && playerOn)
            MovePlatform();
    }

    public void MovePlatform()
    {
        if (Vector3.Distance(transform.position, GetDestination()) < 0.1f)
        {
            moveCount++;
            startPlatform = false;
        }
        transform.position = Vector3.MoveTowards(transform.position, GetDestination(), speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerOn = true;
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerOn = false;
        other.transform.SetParent(null);
    }

    public Vector3 GetDestination()
    {
        if (moveCount > 1)
            moveCount = 0;

        if (moveCount == 0)
            destination = endPos;
        else if (moveCount == 1)
            destination = startPos;

        return destination;
    }
}
