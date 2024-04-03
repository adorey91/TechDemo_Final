using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    private SavePosition savePos;
    [SerializeField] AudioSource respawnAudioSource;



    public void Start()
    {
        player = GameObject.Find("Player");
        savePos = player.GetComponent<SavePosition>();
        respawnAudioSource.playOnAwake = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlayAudioThenRespawn());
        }
    }

    IEnumerator PlayAudioThenRespawn()
    {
        respawnAudioSource.Play ();
        yield return new WaitForSeconds(0.05f);
        //yield return new WaitUntil(() => respawnAudioSource.time >= respawnAudioSource.clip.length);
        player.transform.position = savePos.playerPosition;
    }
}
