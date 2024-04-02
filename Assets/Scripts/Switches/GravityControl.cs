using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : LeverController
{
    [Header("Audio")]
    public AudioSource gravityAudio;

    public void TurnOffGravity()
    {
        if (!leverRotated)
        {
            ResetOtherLever();
            MoveLever(leverRotationAngle);
            gravityAudio.Play();
            leverRotated = true;

            for(int i = 0; i <manager.MazeRangeList.Count; i++)
            {
                manager.MazeRangeList[i].useGravity = false;
                manager.MazeRangeList[i].isKinematic = false;
            }
        }
    }

    void ResetOtherLever()
    {
        fallScript.lever.transform.rotation = fallScript.leverRotation;
        fallScript.leverRotated = false;
    }
}
