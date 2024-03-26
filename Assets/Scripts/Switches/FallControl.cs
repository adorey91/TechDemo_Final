using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallControl : LeverController
{
    public GravityControl gravityScript;
    public bool fallApartNow;
    public GameObject fallLever;

    public GameObject fallText;

    public void Update()
    {
        if (fallApartNow && !leverRotated)
        {
            ResetOtherLever();
            MoveLever(50);

            for (int i = 0; i < gravityScript.RangeList.Count; i++)
            {
                gravityScript.RangeList[i].isKinematic = false;
                gravityScript.RangeList[i].useGravity = true;
            }
            for (int i = 0; i < gravityScript.MazeList.Count; i++)
            {
                gravityScript.MazeList[i].isKinematic = false;
                gravityScript.MazeList[i].useGravity = true;
            }
        }
        if(leverRotated == true)
            fallText.SetActive(true);
    }

   
    void ResetOtherLever()
    {
        gravityScript.lever.transform.rotation = gravityScript.leverRotation;
        gravityScript.turnOffGravity = false;
        gravityScript.leverRotated = false;
        gravityScript.gravityText.SetActive(false);
    }
}
