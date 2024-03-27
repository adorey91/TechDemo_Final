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

    [Header("View Settings")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float upDownLimit;
    [SerializeField] private Camera playerCamera;
    private float verticalRotation;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controlsPanel;
    //[SerializeField] private GameObject gameOverPanel;

    public void Awake()
    {
        current = this;
        pauseMenu = FindAnyObjectByType<PauseMenu>();

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        curHp = maxHp;
       // healthBarUI.Start();
    }
    private void Update()
    {
       // healthPercentage.text = $"{((float)curHp / (float)maxHp) * 100} %";
    }

    private void FixedUpdate()
    {
        
        MovePlayer();
        if (playerInput != null)
            moveSpeed = walkSpeed;
    }


    private void MovePlayer()
    {
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

        rb.MovePosition(rb.position + movementVector.normalized * moveSpeed * Time.deltaTime);
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
        if (context.performed && isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool isGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.1f, ground);
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
        if (context.performed)
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
}
