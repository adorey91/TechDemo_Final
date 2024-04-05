using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Movement Positions")]
    Vector3 destination;
    Vector3 startPos;
    Vector3 endPos;
    int moveCount;

    [Header("Movement Speed")]
    public float speed;

    internal bool startPlatform;
    bool isPlayerOn = false;
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerOn = true;
            other.transform.SetParent(transform);
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
