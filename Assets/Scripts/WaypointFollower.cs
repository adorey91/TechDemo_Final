using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    internal Vector3 startPos;
    private Vector3 endPos;
    private Vector3 destination;
    private int moveCount;

    public float speed = 1f;
    internal bool playerOn;
    internal bool startPlatform;
    internal bool movingPlatform = false;
    [SerializeField] AudioClip platformNoise;
    [SerializeField] AudioSource platformSource;
    bool isAudioPlaying = false;

    private void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(-39.7f, 7.5f, 13.9f);
    }

    private void Update()
    {
        if (startPlatform && playerOn)
        {
            MovePlatform();
            movingPlatform = true;
        }
        else
            movingPlatform = false;
    }

    public void MovePlatform()
    {
        if (Vector3.Distance(transform.position, GetDestination()) < 0.1f)
        {
            moveCount++;
            platformSource.Stop();
            startPlatform = false;
            isAudioPlaying = false;
        }
        else if (!isAudioPlaying)
        {
            platformSource.clip = platformNoise;
            platformSource.Play();
            isAudioPlaying = true;
            platformSource.volume = 0.15f;
            platformSource.pitch = 0.19f;
            platformSource.loop = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, GetDestination(), speed * Time.deltaTime);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
