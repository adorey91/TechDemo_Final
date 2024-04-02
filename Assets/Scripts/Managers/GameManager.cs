using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject levelBoundaries;

    public GameObject originalMazePrefab; // Reference to the original Maze prefab
    public GameObject originalRangePrefab; // Reference to the original Range prefab

    public GameObject mazeInstance; // Reference to the instantiated Maze object
    public GameObject rangeInstance; // Reference to the instantiated Range object

    public Vector3 originalMazePosition;
    public Vector3 originalRangePosition;

    public GravityControl noGravityScript;

    public GameObject gemPrefab;
    private GameObject gemInstanceClone;
    [SerializeField] private List<Vector3> gemPos;

    private int countIndex = 0;
    private int gemHolderCount;

    public void Start()
    {
        GameObject.Instantiate(levelBoundaries);
        BuildBuildings();
        gemHolderCount = 0;
        GemIndex();
    }

    public void Update()
    {
        CreateGem();
    }

    void CreateGem()
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

    void BuildBuildings()
    {
        originalMazePosition = new Vector3(31.4f, 6f, -31.19f);
        originalRangePosition = new Vector3(-10f, 6f, -35.61f);

        mazeInstance = Instantiate(originalMazePrefab, originalMazePosition, transform.rotation);
        rangeInstance = Instantiate(originalRangePrefab, originalRangePosition, transform.rotation);

        noGravityScript.AddRigidbodiesFromChildren(mazeInstance, noGravityScript.MazeList);
        noGravityScript.AddRigidbodiesFromChildren(rangeInstance, noGravityScript.RangeList);
    }

    public void GemIndex()
    {
        gemPos = new List<Vector3>()
        {
            new Vector3(6.55f, 1.059f, 3.33f),
            new Vector3(17.15f, 2.889f, 15.13f),
            new Vector3(20.14f, 4.229f, 19.31f),
            new Vector3(20.14f, 8.609f, 19.31f),
            new Vector3(4.62f, 8.609f, 13.41f),
            new Vector3(-23.14f, 8.609f, 13.41f),
            new Vector3(-66.34f, 8.609f, 13.41f),
        };
    }
}