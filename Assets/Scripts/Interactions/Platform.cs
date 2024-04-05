using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    GameManager gameManager;
    Player player;

    [Header("Movement Positions")]
    Vector3 destination;
    Vector3 startPos;
    Vector3 endPos;
    int moveCount;

    [Header("Movement Speed")]
    public float speed;

    internal bool startPlatform;
    [SerializeField] bool isPlayerOn = false;
    [SerializeField] bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(-39.7f, 7.5f, 13.9f);
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startPlatform && isPlayerOn)
        {
            MovePlatform();
            isMoving = true;
        }
        else
            isMoving = false;

        IsMoving();
        IsPlayerOn();
    }

    private void MovePlatform()
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
        if (gameManager.currentState != GameManager.GameState.Gameplay && gameManager.currentState != GameManager.GameState.Pause)
            gameManager.LoadState("Gameplay");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerOn = true;
            other.transform.SetParent(transform);

            if (!isMoving)
            {
                player.InteractText.gameObject.SetActive(true);
                player.InteractText.SetText("Press Enter To Start Platform");
            }
            else
                player.InteractText.gameObject.SetActive(false);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerOn = false;
        other.transform.SetParent(null);
    }

    public bool IsMoving() { return isMoving; }

    public bool IsPlayerOn() { return isPlayerOn; }

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
