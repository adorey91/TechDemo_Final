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


    [Header("Switch Settings")]
    [SerializeField] private TextMeshPro UseText;
    [SerializeField] private Transform Camera;
    [SerializeField] private float MaxUseDistance = 5f;
    [SerializeField] private LayerMask UseLayers;

    private bool isGamePaused;
    public PauseMenu pauseMenu;

    public void Start()
    {
        isGamePaused = false;
    }

    public void Update()
    {
        CheckLevers();
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

    private void CheckLevers()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            if (hit.collider.TryGetComponent<LeverController>(out LeverController lever))
            {
                if (!lever.IsLeverRotated())
                {
                    UseText.SetText("Press Enter To Activate");
                    UseText.gameObject.SetActive(true);
                    Vector3 midpoint = (hit.point + Camera.position) * 0.5f;
                    UseText.transform.position = midpoint;
                    UseText.transform.rotation = Quaternion.LookRotation((hit.point - Camera.position).normalized);

                    string leverAsString = lever.ToString();
                    if (leverAsString == "TurnOffGravity (GravityControl)")
                    {
                        gravity = true;
                        reset = false;
                        fall = false;
                    }
                    if (leverAsString == "ResetSwitch (ResetBuildings)")
                    {
                        reset = true;
                        gravity = false;
                        fall = false;
                    }
                    if (leverAsString == "FallApart (FallControl)")
                    {
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