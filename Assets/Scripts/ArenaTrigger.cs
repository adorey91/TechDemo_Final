using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    [SerializeField] GameObject playerHealth;
    [SerializeField] bool showHealth;
    [SerializeField] GameObject arenaCamera;
    [SerializeField] GameObject arenaScreen;

    void Start()
    {
        playerHealth.SetActive(false);
        showHealth = false;
    }

    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (showHealth == true)
            {
                playerHealth.SetActive(false);
                showHealth = false;
                arenaCamera.SetActive(false);
                arenaScreen.SetActive(false);
            }
            else if (showHealth == false)
            {
                playerHealth.SetActive(true);
                showHealth = true;
                arenaCamera.SetActive(true);
                arenaScreen.SetActive(true);
            }
        }
    }

    void MovePlayerCamera()
    {

    }
}
