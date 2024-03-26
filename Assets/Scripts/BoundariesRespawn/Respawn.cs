using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    private SavePosition savePos;

    public void Start()
    {
        player = GameObject.Find("Player");
        savePos = player.GetComponent<SavePosition>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = savePos.playerPosition;
        }
     
    }
}
