using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public enum SwitchType
    {
        Reset,
        FallControl,
        GravityControl,
    }
    public SwitchType type;

    [Header("Switch Settings")]
    public string interactionText;
    public GameObject lever;
    [SerializeField] float leverRotationAngle = 50;
    [SerializeField] float rotationDuration = 0.3f;
    [SerializeField] SwitchController switchToReset;
    internal Quaternion leverRotation;
    internal bool leverRotated;
    bool rotating;


    SwitchController[] allSwitches;
    BuildingManager buildingManager;
    SoundManager soundManager;

    public void Start()
    {
        leverRotation = Quaternion.identity;
        buildingManager = FindAnyObjectByType<BuildingManager>();
        soundManager = FindAnyObjectByType<SoundManager>();

        if (this.type == SwitchType.Reset)
        {
            allSwitches = FindObjectsOfType<SwitchController>();
        }
    }

    /// <summary>
    /// Depending on the type of switch the player is interacting with, one of these 3 states will occur.
    /// </summary>
    /// <param name="type"></param>
    public void ActivateLever(SwitchType type)
    {
        MoveLever();

        switch (type)
        {
            case SwitchType.Reset:
                soundManager.PlayBuildingReset();
                StartCoroutine(buildingManager.DestroyBuildings());
                StartCoroutine(ReturnLever());
                break;
            case SwitchType.GravityControl:
                soundManager.PlayBuildingOther();
                for (int i = 0; i < buildingManager.MazeRangeList.Count; i++)
                {
                    buildingManager.MazeRangeList[i].useGravity = false;
                    buildingManager.MazeRangeList[i].isKinematic = false;
                } break;
            case SwitchType.FallControl:
                soundManager.PlayBuildingOther();
                for (int i = 0; i < buildingManager.MazeRangeList.Count; i++)
                {
                    buildingManager.MazeRangeList[i].useGravity = true;
                    buildingManager.MazeRangeList[i].isKinematic = false;
                } break;
        }
    }

    public void MoveLever()
    {
        ResetOtherLever();

        StartCoroutine(RotateLever(leverRotationAngle));
    }

    IEnumerator RotateLever(float targetAngle)
    {
        if (rotating)
            yield break;

        rotating = true;

        Vector3 startRotation = lever.transform.rotation.eulerAngles;
        Vector3 targetRotation = startRotation + new Vector3(targetAngle, 0, 0);

        float counter = 0;

        while (counter < rotationDuration)
        {
            counter += Time.deltaTime;
            lever.transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, counter / rotationDuration);
            yield return null;
        }
        rotating = false;
        this.leverRotated = true;
    }

    IEnumerator ReturnLever()
    {
        yield return new WaitForSeconds(1f);
        lever.transform.rotation = leverRotation;
        leverRotated = false;
    }

    void ResetOtherLever()
    {
        if (this.type == SwitchType.Reset)
        {
            for (int i = 0; i < allSwitches.Length; i++)
            {
                allSwitches[i].ReturnLever();
            }
        }
        else
        {
            switchToReset.ReturnLever();
        }
    }
}
