using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [Header("Collectable Gems")]
    public GameObject gemPrefab;
    private GameObject gemInstanceClone;
    private List<Vector3> gemPos = new()
    {
            new Vector3(6.55f, 1f, 3.33f),
            new Vector3(15.75f, 2.5f, 15.13f),
            new Vector3(20.14f, 4f, 19.31f),
            new Vector3(20.14f, 8.4f, 19.31f),
            new Vector3(4.62f, 8.4f, 13.41f),
            new Vector3(-23.14f, 8.4f, 13.41f),
            new Vector3(-66.34f, 8.4f, 13.41f),
    };
    private int countIndex = 0;
    private int gemHolderCount;

    [Header("Collectable Health")]
    public GameObject hearts;
    public GameObject arena;
    private GameObject heartInstance;

    public Transform[] heartPos = new Transform[4];

    int heartCount = 0;

    void Start()
    {
        gemHolderCount = 0;
    }

    void Update()
    {
        CreateGem();
        CreateHealth(); 
    }

    /// <summary>
    /// Creates a new gem if there currently isn't on on the map and the current amount is less than the amount of positions on the map.
    /// </summary>
    void CreateGem()
    {
        gemInstanceClone = GameObject.Find("GemPickup(Clone)");

        if (gemInstanceClone == null && gemHolderCount < gemPos.Count)
        {

            InstantiateGem();
            gemHolderCount++;
        }
    }

    void CreateHealth()
    {
        heartInstance = GameObject.Find("HealingHeart(Clone)");

        if(heartInstance == null)
            SpawnHealing();
    }

    void InstantiateGem()
    {
        gemInstanceClone = Instantiate(gemPrefab, gemPos[countIndex], Quaternion.identity);
        countIndex++;
    }

    /// <summary>
    /// Spawns hearts in the arena
    /// </summary>
    public void SpawnHealing()
    {
        Instantiate(hearts, heartPos[heartCount].position,Quaternion.identity, arena.transform);
        heartCount++;
        if (heartCount > heartPos.Length - 1)
            heartCount = 0;
    }
}
