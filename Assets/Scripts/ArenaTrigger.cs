using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    [SerializeField] GameObject playerHealth;
    [SerializeField] bool showHealth;

    void Start()
    {
        playerHealth.SetActive(false);
        showHealth = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (showHealth == true)
            {
                playerHealth.SetActive(false);
                showHealth = false;
            }
            else if (showHealth == false)
            {
                playerHealth.SetActive(true);
                showHealth = true;
            }
        }
    }
}
