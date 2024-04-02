using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform teleportPlace;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.position = teleportPlace.transform.position;
            other.transform.rotation = teleportPlace.transform.rotation;
        }

    }
}
