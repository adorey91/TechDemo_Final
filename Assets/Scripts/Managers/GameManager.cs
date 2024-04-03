using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject levelBoundaries;

    public GameObject MazeRangePrefab;
    public GameObject MazeRangeInstance;
    Vector3 MazeRangePosition = new (29, 6, -35);

    public GameObject gemPrefab;
    private GameObject gemInstanceClone;
    public List<Vector3> gemPos = new()
    {
            new Vector3(6.55f, 1f, 3.33f),
            new Vector3(17.15f, 2.5f, 15.13f),
            new Vector3(20.14f, 4f, 19.31f),
            new Vector3(20.14f, 8.4f, 19.31f),
            new Vector3(4.62f, 8.4f, 13.41f),
            new Vector3(-23.14f, 8.4f, 13.41f),
            new Vector3(-66.34f, 8.4f, 13.41f),
    };

    private int countIndex = 0;
    private int gemHolderCount;

    internal List<Rigidbody> MazeRangeList = new();

    public void Start()
    {
        GameObject.Instantiate(levelBoundaries);
        BuildBuildings();
        gemHolderCount = 0;
    }

    public void Update()
    {
        CreateGem();
    }

    void CreateGem() // creates a new instance if one doesn't exist upto the amount of positions are placed
    {
        gemInstanceClone = GameObject.Find("GemPickup(Clone)");

        if (gemInstanceClone == null && gemHolderCount < gemPos.Count)
        {
            InstantiateGem();
            gemHolderCount++;
        }
    }

    void InstantiateGem()
    {
        gemInstanceClone = Instantiate(gemPrefab, gemPos[countIndex], Quaternion.identity);
        countIndex++;
    }

    public void BuildBuildings() // creates a new building
    {
        MazeRangeInstance = Instantiate(MazeRangePrefab, MazeRangePosition, transform.rotation);
        AddRigidbodiesFromChildren(MazeRangeInstance, MazeRangeList);
    }

    internal void AddRigidbodiesFromChildren(GameObject obj, List<Rigidbody> rigidbodyList)
    {
        Rigidbody[] rigidbodies = obj.GetComponentsInChildren<Rigidbody>();
        rigidbodyList.AddRange(rigidbodies);
    }

    internal void ClearLists()
    {
        MazeRangeList.Clear();
    }
}