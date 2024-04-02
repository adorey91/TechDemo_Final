using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupGem : MonoBehaviour
{
    public GameObject gemUIPrefab;
    [SerializeField] private GameObject uiParent;
    public AudioClip pickUpClip;
    public AudioSource soundPlayer;

    public void Update()
    {
        if (uiParent == null)
            uiParent = GameObject.Find("GemHolder");
        if (soundPlayer == null)
            soundPlayer = uiParent.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PlayClip());
        }
    }

    IEnumerator PlayClip()
    {
        soundPlayer.clip = pickUpClip;
        soundPlayer.Play();
        yield return new WaitUntil(() => soundPlayer.time >= pickUpClip.length);
        AddGemUI();
        Destroy(this.gameObject);
    }

    void AddGemUI()
    {
        GameObject NewUIGem = Instantiate(gemUIPrefab);
        NewUIGem.transform.SetParent(uiParent.transform);
    }
}