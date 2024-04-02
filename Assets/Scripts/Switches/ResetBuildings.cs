using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBuildings : LeverController
{
    public GameManager manager;
    public GravityControl gravityScript;
    public FallControl fallScript;
    public bool resetRequested;
    public AudioSource resetAudio;

    void Update()
    {
        if (resetRequested & !leverRotated)
        {
            MoveLever(45);
            ResetOtherLevers();
            gravityScript.ClearLists();
            FindMazeAndRangeInstance();
            resetAudio.Play();

            if (manager.mazeInstance != null)
                Destroy(manager.mazeInstance);
            if (manager.rangeInstance != null)
                Destroy(manager.rangeInstance);

            manager.mazeInstance = Instantiate(manager.originalMazePrefab, manager.originalMazePosition, transform.rotation);
            manager.rangeInstance = Instantiate(manager.originalRangePrefab, manager.originalRangePosition, transform.rotation);
            gravityScript.AddRigidbodiesFromChildren(manager.mazeInstance, gravityScript.MazeList);
            gravityScript.AddRigidbodiesFromChildren(manager.rangeInstance, gravityScript.RangeList);

            resetRequested = false;  
        }
    }
    void FindMazeAndRangeInstance()
    {
        manager.mazeInstance = GameObject.Find("Maze(Clone)");
        manager.rangeInstance = GameObject.Find("Range(Clone)");

        if (manager.mazeInstance != null)
            manager.originalMazePosition = manager.mazeInstance.transform.position;
        else
            Debug.LogError("Maze GameObject not found in the scene!");

        if (manager.rangeInstance != null)
            manager.originalRangePosition = manager.rangeInstance.transform.position;
        else
            Debug.LogError("Range GameObject not found in scene!");
    }

    void ResetOtherLevers()
    {
        gravityScript.lever.transform.rotation = gravityScript.leverRotation;
        gravityScript.turnOffGravity = false;
        gravityScript.leverRotated = false;
        fallScript.lever.transform.rotation = fallScript.leverRotation;
        fallScript.fallApartNow = false;
        fallScript.leverRotated = false;
    }
}