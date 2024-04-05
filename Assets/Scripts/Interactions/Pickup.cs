using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum Items
    {
        Gems,
        Hearts,
    }
    public Items itemType;

    [Header("Gem Settings")]
    public GameObject gemUIPrefab;
    public GameObject gemUIParent;

    [Header("Heart Settings")]
    public int healAmount;


    // Start is called before the first frame update
    void Start()
    {
        if (itemType == Items.Gems)
        {
            if (gemUIParent == null)
                gemUIParent = GameObject.Find("GemHolder");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (itemType == Items.Gems)
                StartCoroutine(CollectGem());
            if (itemType == Items.Hearts)
            {
                Character hit = other.GetComponent<Character>();
                StartCoroutine(CollectHeart(hit));
            }
        }
    }

    IEnumerator CollectHeart(Character hit)
    {
        //maybe sound
        yield return new WaitForSeconds(0.3f);
        if(hit !=  null)
        {
            hit.Heal(healAmount);
            Destroy(gameObject);
        }
    }

    IEnumerator CollectGem()
    {
        //maybe sound at a later time.
        yield return new WaitForSeconds(0.3f);
        AddGemUI();
        Destroy(this.gameObject);
    }

    void AddGemUI()
    {
        GameObject newUIGem = Instantiate(gemUIPrefab);
        newUIGem.transform.SetParent(gemUIParent.transform);
    }
}
