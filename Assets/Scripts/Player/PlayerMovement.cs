using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 playerInput;
    private CapsuleCollider player;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float standingHeight;
    private bool isCrouched;
    private bool isRunning;
    [SerializeField] LayerMask ground;


    [Header("View Settings")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float upDownLimit;
    [SerializeField] private Camera playerCamera;
    private float verticalRotation;

    [SerializeField] private WaypointFollower waypointFollower;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controlsPanel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FixedUpdate()
    {
        MovePlayer();

        if (playerInput != null)
            moveSpeed = walkSpeed;
    }

    private void MovePlayer()
    {
        isGrounded();

        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = playerCamera.transform.right;

        Vector3 movementVector = playerInput.y * cameraForward + playerInput.x * cameraRight;
        movementVector.Normalize();

        if (isRunning)
            moveSpeed = walkSpeed * 2;
        if (isCrouched)
            moveSpeed = walkSpeed / 2;
        if (!isRunning && !isCrouched)
            moveSpeed = walkSpeed;

        if (waypointFollower.playerOn)
            MovePlayerOnPlatform();
        else
            rb.MovePosition(rb.position + movementVector.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        if(!pausePanel.activeSelf && !controlsPanel.activeSelf)
        {
            if (context.performed)
            {
                Vector2 mouseDelta = context.ReadValue<Vector2>();

                float mouseRotationX = mouseDelta.x * mouseSensitivity;
                this.transform.Rotate(0, mouseRotationX, 0);

                verticalRotation -= mouseDelta.y * mouseSensitivity;
                verticalRotation = Mathf.Clamp(verticalRotation, -upDownLimit, upDownLimit);

                playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
            }
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
            isRunning = true;
        if (context.canceled)
            isRunning = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouched = true;
            player.height = crouchHeight;
        }
        else if (context.canceled)
        {
            RaycastHit hit;

            if (!Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), Vector3.up, out hit, 2f))
            {
                isCrouched = false;
                player.height = standingHeight;
            }
        }
    }

    bool isGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.1f, ground);
    }

    void MovePlayerOnPlatform()
    {
        Vector3 destination = waypointFollower.GetDestination();
        float movingSpeed = waypointFollower.speed;

        Vector3 direction = (destination - rb.position).normalized;

        Vector3 newPosition = Vector3.MoveTowards(rb.position, destination, Time.deltaTime * movingSpeed);
        rb.MovePosition(newPosition);

        Vector3 movementVector = new Vector3(playerInput.x, 0f, playerInput.y);

        Vector3 targetVelocity = (movementVector.x * playerCamera.transform.right + movementVector.z * playerCamera.transform.forward).normalized * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
    }
}