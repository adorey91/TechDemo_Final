using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform teleportPlace;
    [SerializeField] AudioSource teleportAudio;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.position = teleportPlace.transform.position;
            other.transform.rotation = teleportPlace.transform.rotation;
            teleportAudio.Play();
        }

    }
}
