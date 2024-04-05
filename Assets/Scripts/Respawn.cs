using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    GameObject player;
    SavePosition savePos;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        savePos = player.GetComponent<SavePosition>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.05f);
        player.transform.position = savePos.playerPosition;
    }
}
