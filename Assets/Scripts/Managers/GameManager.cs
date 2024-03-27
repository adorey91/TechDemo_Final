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

    //public GameObject gemPrefab;
    //private GameObject gemInstanceClone;
    //[SerializeField] private List<Vector3> gemPos;

    //[SerializeField] private int countIndex = 0;
    //private int gemHolderCount;

    public void Start()
    {
        //gemHolderCount = 0;
        GameObject.Instantiate(levelBoundaries);
        BuildBuildings();
        //GemIndex();
    }

    public void Update()
    {
        //CreateGem();
    }

    //void CreateGem()
    //{
    //    gemInstanceClone = GameObject.Find("GemPickup(Clone)");

    //    if (gemInstanceClone == null && gemHolderCount < 10)
    //    {
    //        InstantiateGem();
    //        gemHolderCount++;
    //    }
    //}

    //void InstantiateGem()
    //{
    //    gemInstanceClone = Instantiate(gemPrefab, gemPos[countIndex], Quaternion.identity);
    //    countIndex++;
    //    if (countIndex >= gemPos.Count)
    //        countIndex = 0;
    //}

    void BuildBuildings()
    {
        originalMazePosition = new Vector3(31.4f, 8f, -31.19f);
        originalRangePosition = new Vector3(-10f, 8f, -35.61f);

        mazeInstance = Instantiate(originalMazePrefab, originalMazePosition, transform.rotation);
        rangeInstance = Instantiate(originalRangePrefab, originalRangePosition, transform.rotation);

        noGravityScript.AddRigidbodiesFromChildren(mazeInstance, noGravityScript.MazeList);
        noGravityScript.AddRigidbodiesFromChildren(rangeInstance, noGravityScript.RangeList);
    }

    //public void GemIndex()
    //{
    //    gemPos = new List<Vector3>()
    //    {
    //        new Vector3(-7.82f, 1.059f, 11.72f),
    //        new Vector3(14.87f, 1.059f, 4.28f),
    //        new Vector3(-2.18f, 1.059f, 29.64f),
    //        new Vector3(13.03f, 4.095f, 29.15f),
    //        new Vector3(34.03f, 1.059f, -13.09f)
    //    };
    //}
}