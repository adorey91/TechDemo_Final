using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public static Player current;
    [SerializeField] HealthBarUI healthBarUI;
    [SerializeField] PauseMenu pauseMenu;
    private Vector2 playerInput;
    private Rigidbody rb;

    [SerializeField] private TMP_Text healthPercentage;

    [Header("Attack")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] float timeBetweenAttacks;
    bool alreadyAttacked;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeedMultipler;
    private bool isRunning;
    [SerializeField] LayerMask ground;
    float distToGround;

    [Header("View Settings")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float upDownLimit;
    [SerializeField] private Camera playerCamera;
    private float verticalRotation;

    [Header("Game Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject gameOverPanel;

    WaypointFollower waypointFollower;

    public void Awake()
    {
        waypointFollower = FindObjectOfType<WaypointFollower>();
        current = this;
        pauseMenu = FindAnyObjectByType<PauseMenu>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        curHp = maxHp;
        playerCamera.transform.rotation = Quaternion.identity;
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y; //finds the distance to ground from the collider
        healthBarUI.Start();
    }

    private void Update()
    {
        healthPercentage.text = $"{((float)curHp / (float)maxHp) * 100} %";
    }

    private void FixedUpdate()
    {
        if (playerInput != null)
            MovePlayer();
    }

    private void MovePlayer()
    {
        isGrounded();

        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = playerCamera.transform.right;

        Vector3 movementVector = (playerInput.y * cameraForward) + (playerInput.x * cameraRight);
        movementVector.Normalize();

        if (isRunning)
            moveSpeed = walkSpeed * moveSpeedMultipler;
        else
            moveSpeed = walkSpeed;


        if (waypointFollower.movingPlatform)
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
        if (context.performed && !pauseMenu.paused)
        {
            Vector2 mouseDelta = context.ReadValue<Vector2>();

            float mouseRotationX = mouseDelta.x * mouseSensitivity;
            this.transform.Rotate(0, mouseRotationX, 0);

            verticalRotation -= mouseDelta.y * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownLimit, upDownLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded() && !waypointFollower.movingPlatform)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, - Vector3.up, distToGround + 0.1f);
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
            isRunning = true;
        if (context.canceled)
            isRunning = false;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed && !waypointFollower.movingPlatform)
        {
            if (!alreadyAttacked)
            {
                Vector3 fireDirection = playerCamera.transform.forward;
                Vector3 spawnPosition = transform.position + fireDirection * 1f;

                GameObject proj = Instantiate(attackPrefab, spawnPosition, Quaternion.identity);
                proj.transform.rotation = Quaternion.LookRotation(fireDirection);
                proj.GetComponent<Projectile>().Setup(this);
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void MovePlayerOnPlatform()
    {
        Vector3 destination = waypointFollower.GetDestination();
        float movingSpeed = waypointFollower.speed;

        Vector3 direction = (destination - rb.position).normalized;

        Vector3 newPosition = Vector3.MoveTowards(rb.position, direction, Time.deltaTime * movingSpeed);
        rb.MovePosition(newPosition);

        Vector3 movementVector = new Vector3(playerInput.x, 0f, playerInput.y);

        Vector3 targetVelocity = (movementVector.x * playerCamera.transform.right + movementVector.z * playerCamera.transform.forward).normalized * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
    }
}
