using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupGem : MonoBehaviour
{
    public GameObject gemUIPrefab;
    [SerializeField] private GameObject uiParent;

    public void Update()
    {
        if (uiParent == null)
            uiParent = GameObject.Find("GemHolder");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            AddGemUI();
            Destroy(this.gameObject);
        }
    }

    void AddGemUI()
    {
        GameObject NewUIGem = Instantiate(gemUIPrefab);
        NewUIGem.transform.SetParent(uiParent.transform);
    }
}