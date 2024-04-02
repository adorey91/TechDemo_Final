using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBuildings : LeverController
{
    [Header("Reset Settings")]
    public AudioSource resetAudio;

    public void DestroyBuildings()
    {
        if (!leverRotated)
        {
            MoveLever(leverRotationAngle);
            ResetOtherLevers();
            ResetPlatform();
            manager.ClearLists();
            resetAudio.Play();
            leverRotated = true;

            if (manager.MazeRangeInstance != null)
                StartCoroutine(DestroyInstances());
        }
    }

    void ResetPlatform()
    {
        platform.transform.position = platform.startPos;
    }

    //Resets all other levers to their default position
    void ResetOtherLevers()
    {
        gravityScript.lever.transform.rotation = gravityScript.leverRotation;
        gravityScript.leverRotated = false;
        fallScript.lever.transform.rotation = fallScript.leverRotation;
        fallScript.leverRotated = false;
    }

    IEnumerator DestroyInstances()
    {
        Destroy(manager.MazeRangeInstance);
        yield return new WaitForSeconds(0.5f);

        manager.BuildBuildings();
    }
}