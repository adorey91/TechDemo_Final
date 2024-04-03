using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Building Settings")]
    private bool reset;
    private bool fall;
    private bool gravity;
    ResetBuildings resetBuildings;
    FallControl fallControl;
    GravityControl gravityControl;
    WaypointFollower waypointFollower;

    [Header("Switch Settings")]
    [SerializeField] private TextMeshPro UseText;
    [SerializeField] private Transform Camera;
    [SerializeField] float DistanceFromCamera;
    public LayerMask layerMask;

    private bool isGamePaused;
    public PauseMenu pauseMenu;

    public void Start()
    {
        isGamePaused = false;
        waypointFollower = FindObjectOfType<WaypointFollower>();
        resetBuildings = FindObjectOfType<ResetBuildings>();
        fallControl = FindObjectOfType<FallControl>();
        gravityControl = FindObjectOfType<GravityControl>();
    }

    private void Update()
    {
        if (reset && !resetBuildings.leverRotated)
            ShowText("Press Enter To \nReset Buildings");
        else if (fall && !fallControl.leverRotated)
            ShowText("Press Enter To \nBreak Buildings");
        else if (gravity && !gravityControl.leverRotated)
            ShowText("Press Enter \nTo Turn Off Gravity \nto Buildings");
        else if (waypointFollower.playerOn && !waypointFollower.startPlatform)
            ShowText("Press Enter to\nMove Platform");
        else if (waypointFollower.playerOn && waypointFollower.startPlatform)
            UseText.gameObject.SetActive(false);
        else
            UseText.gameObject.SetActive(false);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (reset)
                resetBuildings.DestroyBuildings();
            if (fall)
                fallControl.FallApart();
            if (gravity)
                gravityControl.TurnOffGravity();
            if (waypointFollower.playerOn)
                waypointFollower.startPlatform = true;
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed && isGamePaused == false)
        {
            pauseMenu.pauseGame();
            isGamePaused = true;
        }
        else if (context.performed && isGamePaused == true)
        {
            pauseMenu.resumeGame();
            isGamePaused = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.name = other.gameObject.name;
        if (other.name == "ResetSwitch" && !resetBuildings.leverRotated)
        {
            reset = true;
            gravity = false;
            fall = false;
        }
        else if (other.name == "FallApart" && !fallControl.leverRotated)
        {
            fall = true;
            reset = false;
            gravity = false;
        }
        else if (other.name == "TurnOffGravity" && !gravityControl.leverRotated)
        {
            gravity = true;
            reset = false;
            fall = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        reset = false;
        gravity = false;
        fall = false;

        UseText.gameObject.SetActive(false);
        resetBuildings.lever.transform.rotation = resetBuildings.leverRotation;
        resetBuildings.leverRotated = false;
    }

    // shows textbox infront of player when inside trigger
    private void ShowText(string sentence)
    {
        UseText.SetText(sentence);
        UseText.gameObject.SetActive(true);
        Vector3 positionOffset = Camera.forward * DistanceFromCamera;
        UseText.transform.SetPositionAndRotation(Camera.position + positionOffset, Quaternion.LookRotation(Camera.forward));
    }
}