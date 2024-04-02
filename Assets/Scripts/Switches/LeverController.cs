using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("Lever Settings")]
    public GameObject lever;
    [SerializeField] internal float leverRotationAngle = 45;
    [SerializeField] float rotationDuration = 0.3f;
    internal bool leverRotated;
    private bool rotating;
    internal Quaternion leverRotation;


    [Header("Scripts Referenced")]
    internal GameManager manager;
    internal WaypointFollower platform;
    internal GravityControl gravityScript;
    internal FallControl fallScript;

    public void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
        platform = FindAnyObjectByType<WaypointFollower>();
        gravityScript = FindAnyObjectByType<GravityControl>();
        fallScript = FindAnyObjectByType<FallControl>();
    }

    // Moves lever to target angle over time
    public void MoveLever(float targetAngle)
    {
        StartCoroutine(RotateLever(targetAngle));
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
    }
}

