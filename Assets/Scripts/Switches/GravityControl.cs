using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : LeverController
{
    public List<Rigidbody> MazeList;
    public List<Rigidbody> RangeList;

    public bool turnOffGravity;

    public FallControl fallScript;

    public GameObject gravityText;


    public void Update()
    {
        if (turnOffGravity && !leverRotated)
        {
            ResetOtherLever();
            MoveLever(45);
            for (int i = 0; i < RangeList.Count; i++)
            {
                RangeList[i].useGravity = false;
                RangeList[i].isKinematic = false;
            }
            for (int i = 0; i < MazeList.Count; i++)
            {
                MazeList[i].useGravity = false;
                MazeList[i].isKinematic = false;
            }
        }
        if (leverRotated == true)
            gravityText.SetActive(true);
    }

    public void ClearLists()
    {
        MazeList.Clear();
        RangeList.Clear();
    }

    public void AddRigidbodiesFromChildren(GameObject obj, List<Rigidbody> rigidbodyList)
    {
        Rigidbody[] rigidbodies = obj.GetComponentsInChildren<Rigidbody>();
        rigidbodyList.AddRange(rigidbodies);
    }

    void ResetOtherLever()
    {
        fallScript.lever.transform.rotation = fallScript.leverRotation;
        fallScript.fallApartNow = false;
        fallScript.leverRotated = false;
        fallScript.fallText.SetActive(false);
    }
}
