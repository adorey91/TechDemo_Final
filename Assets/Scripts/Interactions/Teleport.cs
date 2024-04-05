using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform newPosition;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.SetPositionAndRotation(newPosition.transform.position, newPosition.transform.rotation);
        }
    }

}
