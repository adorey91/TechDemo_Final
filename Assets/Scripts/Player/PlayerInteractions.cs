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
    public ResetBuildings resetBuildings;
    public FallControl fallControl;
    public GravityControl gravityControl;
    WaypointFollower waypointFollower;


    [Header("Switch Settings")]
    [SerializeField] private TextMeshPro UseText;
    [SerializeField] private Transform Camera;
    [SerializeField] private float MaxUseDistance = 5f;
    [SerializeField] private LayerMask UseLayers;
    [SerializeField] float DistanceFromCamera;
    
    private bool isGamePaused;
    public PauseMenu pauseMenu;

    public void Start()
    {
        isGamePaused = false;
        waypointFollower = FindObjectOfType<WaypointFollower>();
    }

    public void Update()
    {
        CheckLevers();
        PlatformCheck();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (reset)
                resetBuildings.resetRequested = true;
            if (fall)
                fallControl.fallApartNow = true;
            if (gravity)
                gravityControl.turnOffGravity = true;
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

    private void PlatformCheck()
    {
        if (waypointFollower.playerOn && !waypointFollower.startPlatform)
        {
            UseText.SetText("Press Enter To Activate");
            UseText.gameObject.SetActive(true);

            // Set the position of UseText to be in front of the camera & rotation to camera
            Vector3 positionOffset = Camera.forward * DistanceFromCamera;
            UseText.transform.position = Camera.position + positionOffset;
            UseText.transform.rotation = Quaternion.LookRotation(Camera.forward);
        }
    }

    private void CheckLevers()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<LeverController>(out LeverController lever))
            {
                if (!lever.IsLeverRotated())
                {
                    UseText.SetText("");
                    UseText.gameObject.SetActive(true);
                    Vector3 midpoint = (hit.point + Camera.position) * 0.5f;
                    UseText.transform.position = midpoint;
                    UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);

                    string leverAsString = lever.ToString();
                    if (leverAsString == "TurnOffGravity (GravityControl)")
                    {
                        UseText.SetText("Press Enter \nTo Turn Off Gravity \nto Buildings");
                        gravity = true;
                        reset = false;
                        fall = false;
                    }
                    if (leverAsString == "ResetSwitch (ResetBuildings)")
                    {
                        UseText.SetText("Press Enter To \nReset Buildings");
                        reset = true;
                        gravity = false;
                        fall = false;
                    }
                    if (leverAsString == "FallApart (FallControl)")
                    {
                        UseText.SetText("Press Enter To Watch\n Buildings Fall Apart");
                        fall = true;
                        reset = false;
                        gravity = false;
                    }
                }
            }
        }
        else
        {
            UseText.gameObject.SetActive(false);
            resetBuildings.lever.transform.rotation = resetBuildings.leverRotation;
            resetBuildings.leverRotated = false;
        }
    }
}