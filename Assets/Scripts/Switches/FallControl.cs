using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallControl : LeverController
{
    public AudioSource fallAudio;

    public void FallApart()
    {
        if (!leverRotated)
        {
            ResetOtherLever();
            MoveLever(leverRotationAngle);
            leverRotated = true;
            fallAudio.Play();

            for (int i = 0; i < manager.MazeRangeList.Count; i++)
            {
                manager.MazeRangeList[i].useGravity = true;
                manager.MazeRangeList[i].isKinematic = false;
            }
        }
    }

    void ResetOtherLever()
    {
        gravityScript.lever.transform.rotation = gravityScript.leverRotation;
        gravityScript.leverRotated = false;
    }
}
