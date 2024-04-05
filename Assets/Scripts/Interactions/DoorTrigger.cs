using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Door door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!door.IsOpen)
                door.Open(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (door.IsOpen)
                door.Close();
        }
    }
}
