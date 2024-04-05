using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    #region Declarations

    GameManager gameManager;
    public static Player current;
    private Vector2 playerInput;
    private Rigidbody rb;
    [SerializeField] Platform platform;


    [Header("Movement Settings")]
    [SerializeField] float walkSpeed;
    [SerializeField] float moveSpeedMulitplier;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask ground;
    float distToGround;
    float moveSpeed;
    bool isRunning;

    [Header("View Settings")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float upDownLimit;
    [SerializeField] Camera playerCamera;
    float verticalRotation;

    [Header("Interactions")]
    public LayerMask layerMask;
    public TMP_Text InteractText;
    bool reset;
    bool fall;
    bool gravity;
    private SwitchController interactingWith;

    public bool escapeInteracted;
    #endregion

    public void Awake()
    {
        current = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindAnyObjectByType<GameManager>();
        curHp = maxHp;
        playerCamera.transform.rotation = Quaternion.identity;
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y; // finds distance to the ground from collider
    }

    private void Update()
    {
        if (curHp <= 0)
            gameManager.LoadState("Gameover");

        healthPercentage.text = $"{((float)curHp / (float)maxHp) * 100:F0} %";
    }

    void FixedUpdate()
    {
        if (playerInput != null && !gameManager.IsGamePaused())
            MovePlayer();
    }

    private void MovePlayer()
    {
        IsGrounded();

        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = playerCamera.transform.right;

        Vector3 movementVector = (playerInput.y * cameraForward) + (playerInput.x * cameraRight);
        movementVector.Normalize();

        if (isRunning)
            moveSpeed = walkSpeed * moveSpeedMulitplier;
        else
            moveSpeed = walkSpeed;

        if (platform.IsMoving())
            MovePlayerOnPlatform();
        else
            rb.MovePosition(rb.position + movementVector.normalized * moveSpeed * Time.fixedDeltaTime);
    }


    public void Look(InputAction.CallbackContext context)
    {
        if (context.performed && !gameManager.IsGamePaused())
        {
            Vector2 mouseDelta = context.ReadValue<Vector2>();

            float mouseRotationX = mouseDelta.x * mouseSensitivity;
            this.transform.Rotate(0, mouseRotationX, 0);

            verticalRotation -= mouseDelta.y * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownLimit, upDownLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
    }

    #region Move/Jump/Run/Grounded

    public void Move(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded() && !gameManager.IsGamePaused())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed && !gameManager.IsGamePaused())
            isRunning = true;
        if (context.canceled)
            isRunning = false;
    }

    private void MovePlayerOnPlatform()
    {
        Vector3 destination = platform.GetDestination();
        float movingSpeed = platform.speed;

        Vector3 direction = (destination - rb.position).normalized;

        Vector3 movementVector = new(playerInput.x, 0f, playerInput.y);

        Vector3 targetVelocity = (movementVector.x * playerCamera.transform.right + movementVector.z * playerCamera.transform.forward).normalized * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
    #endregion

    #region Interactions
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed && !gameManager.IsGamePaused())
        {
            if (!alreadyAttacked)
            {
                Vector3 fireDirection = playerCamera.transform.forward;
                Vector3 spawnPosition = transform.position + fireDirection * 1f;

                GameObject proj = Instantiate(attackPrefab, spawnPosition, Quaternion.identity, transform);
                proj.transform.rotation = Quaternion.LookRotation(fireDirection);
                proj.GetComponent<Projectile>().Setup(this);
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && !gameManager.IsGamePaused())
        {
            if (interactingWith != null && !interactingWith.leverRotated)
                interactingWith.ActivateLever(interactingWith.type);
            if (platform.IsPlayerOn())
                platform.startPlatform = true;
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
            gameManager.EscapeState();
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        other.name = other.gameObject.name;
        interactingWith = other.GetComponent<SwitchController>();

        switch(other.name)
        {
            case "ResetSwitch":
                reset = true;
                if(interactingWith.leverRotated == false)
                {
                    InteractText.gameObject.SetActive(true);
                    InteractText.SetText(interactingWith.interactionText);
                }
                break;
            case "FallApart":
                fall = true;
                if (interactingWith.leverRotated == false)
                {
                    InteractText.gameObject.SetActive(true);
                    InteractText.SetText(interactingWith.interactionText);
                }
                break;
            case "TurnOffGravity":
                gravity = true;
                if (interactingWith.leverRotated == false)
                {
                    InteractText.gameObject.SetActive(true);
                    InteractText.SetText(interactingWith.interactionText);
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        reset = false;
        gravity = false;
        fall = false;

        interactingWith = null;
        InteractText.gameObject.SetActive(false);
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
