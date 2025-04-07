using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    public Animator handsAnim;

    [Header("Speed System")]
    public float walkSpeed = 5.0f;
    public float sneakSpeed = 2.5f;
    public float runSpeed = 8.0f;
    public float crouchWalkSpeed = 3.5f;
    public float crouchRunSpeed = 6.5f;
    public float crouchSneakSpeed = 1f;
    public float jumpSpeed = 6.0f;
    public bool limitDiagonalSpeed = true;
    public bool toggleRun = false;
    public bool toggleSneak = false;
    public bool airControl = false;
    public bool crouching = false;

    public enum MotionState
    {
        Idle,
        Running,
        Jumping
    }

    [Header("Motion System")]
    public MotionState currentMotion;

    [Header("Gravity System")]
    public float gravity = 10.0f;
    public float fallingDamageLimit = 10.0f;

    private bool grounded;
    private Vector3 moveDirection;
    private CharacterController controller;
    private Transform myTransform;
    private float speed;
    private RaycastHit hit;
    private float fallStartLevel;
    private bool falling;
    private Crosshair crosshairScript;

    [Header("Input System")]
    public KeyCode inventoryKey = KeyCode.I;

    [Header("GameObjects")]
    public GameObject camera;
    public GameObject inventory;

    public GameObject armsHolder;
    public GameObject weaponHolder;
    public GameObject dropHolder;

    // Use this for initialization
    void Start()
    {
        currentMotion = MotionState.Idle;
        moveDirection = Vector3.zero;
        grounded = false;
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;
        crosshairScript = camera.GetComponent<Crosshair>();

        // Lock cursor
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyGravity();
    }

    private void Update()
    {
        HandleInventoryToggle();
        HandleCrouchToggle();
        HandleRunToggle();
    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? 0.6701f : 1.0f;

        UpdateAnimationState(inputX, inputY);

        if (grounded)
        {
            HandleGroundedMovement(inputX, inputY, inputModifyFactor);
        }
        else
        {
            HandleAirControl(inputX, inputY);
        }
    }

    private void UpdateAnimationState(float inputX, float inputY)
    {
        if (inputX == 0 && inputY == 0)
        {
            handsAnim.SetBool("idle", true);
            handsAnim.SetBool("running", false);
            currentMotion = MotionState.Idle;
        }
    }

    private void HandleGroundedMovement(float inputX, float inputY, float inputModifyFactor)
    {
        if (falling)
        {
            falling = false;
            CheckFallingDamage();
        }

        DetermineSpeed();

        moveDirection = new Vector3(inputX * inputModifyFactor, 0, inputY * inputModifyFactor);
        moveDirection = myTransform.TransformDirection(moveDirection) * speed;

        HandleJump();
    }

    private void CheckFallingDamage()
    {
        if (myTransform.position.y < (fallStartLevel - fallingDamageLimit))
        {
            FallingDamageAlert(fallStartLevel - myTransform.position.y);
        }
    }

    private void DetermineSpeed()
    {
        if (!toggleRun)
        {
            bool running = Input.GetButton("Run");
            speed = running ? runSpeed : walkSpeed;
            handsAnim.SetBool("running", running);
            currentMotion = running ? MotionState.Running : MotionState.Idle;

            if (running)
            {
                crosshairScript.IncreaseSpread(0.5f);
            }
            else if (crosshairScript.spread != crosshairScript.minSpread)
            {
                crosshairScript.DecreaseSpread(2f);
            }
        }

        if (!toggleSneak)
        {
            bool sneaking = Input.GetButton("Sneak");
            speed = sneaking ? sneakSpeed : speed;
        }

        if (crouching)
        {
            speed = Input.GetButton("Run") ? crouchRunSpeed : crouchWalkSpeed;
            speed = Input.GetButton("Sneak") ? crouchSneakSpeed : speed;
        }
    }

    private void HandleJump()
    {
        if (Input.GetButton("Jump"))
        {
            moveDirection.y = jumpSpeed;
            crosshairScript.IncreaseSpread(10f);
            currentMotion = MotionState.Jumping;
        }
    }

    private void HandleAirControl(float inputX, float inputY)
    {
        if (!falling)
        {
            falling = true;
            fallStartLevel = myTransform.position.y;
        }

        if (airControl)
        {
            moveDirection.x = inputX * speed;
            moveDirection.z = inputY * speed;
            moveDirection = myTransform.TransformDirection(moveDirection);
        }
    }

    private void ApplyGravity()
    {
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
        moveDirection.y -= gravity * Time.deltaTime;
    }

    private void HandleInventoryToggle()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            inventory.SetActive(!inventory.activeSelf);
            DisableControllerIfNeeded();
        }
    }

    private void DisableControllerIfNeeded()
    {
        if (inventory.activeSelf)
        {
            DisableController();
        }
        else
        {
            EnableController();
        }
    }

    private void HandleCrouchToggle()
    {
        if (Input.GetButtonUp("Crouch"))
        {
            crouching = !crouching;
            handsAnim.SetBool("Crouch", crouching);
        }
    }

    private void HandleRunToggle()
    {
        if (toggleRun && grounded && Input.GetButtonDown("Run"))
        {
            speed = (speed == walkSpeed ? runSpeed : walkSpeed);
        }
    }

    private void FallingDamageAlert(float fallDistance)
    {
        Debug.Log("Ouch! Fell " + fallDistance + " units!");
    }

    private void EnableController()
    {
        camera.GetComponent<MouseLook>().enabled = true;
        Cursor.visible = false;
    }

    private void DisableController()
    {
        camera.GetComponent<MouseLook>().enabled = false;
        Cursor.visible = true;
    }
}

// Class handling player's health
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Maximum health of the player
    private int currentHealth;   // Current health of the player

    // Called once at the start of the game
    void Start()
    {
        currentHealth = maxHealth;  // Initialize player's health to the maximum value
    }

    // Function to reduce player's health when taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Subtract damage from current health
        Debug.Log("Player takes " + damage + " damage. Current health: " + currentHealth);  // Debug message

        // If health drops to zero or below, the player dies
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handles player death
    private void Die()
    {
        Debug.Log("Player has died!");  // Debug message for player death
        // Additional death logic like respawning or game over can be added here
    }
}