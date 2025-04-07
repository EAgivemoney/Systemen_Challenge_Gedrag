using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    // Enum to represent different states of the zombie's behavior
    private enum ZombieState { Wander, Chase, Attack, Die }
    private ZombieState currentState;  // Current state of the zombie

    // Public variables to tweak zombie behavior in Unity Inspector
    public float wanderRadius = 10f;     // Radius within which the zombie wanders
    public float wanderTimer = 5f;       // Timer to change the wandering destination
    public float detectionRadius = 15f;  // Radius within which the zombie detects the player
    public float attackRange = 2f;       // Range within which the zombie can attack
    public float attackCooldown = 1f;    // Time between zombie attacks
    public int maxHealth = 100;          // Maximum health of the zombie
    public int attackDamage = 10;        // Amount of damage the zombie deals when attacking

    // Private variables to track internal zombie state
    private float timer;                 // Timer for wandering behavior
    private float lastAttackTime;        // Time of the last attack
    private int currentHealth;           // Current health of the zombie
    private Transform player;            // Reference to the player's transform
    private NavMeshAgent navMeshAgent;   // Reference to the NavMeshAgent component
    private bool playerInRange;          // Flag to check if player is within attack range

    private Animator animator;           // Reference to the Animator component for controlling animations
    private bool hasDied = false;        // Flag to ensure the zombie dies only once

    void Start()
    {
        // Initialize the zombie state and variables
        currentState = ZombieState.Wander;                 // Set initial state to wander
        currentHealth = maxHealth;                         // Set current health to max health
        navMeshAgent = GetComponent<NavMeshAgent>();       // Get the NavMeshAgent component for movement
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Find the player using the "Player" tag
        animator = GetComponent<Animator>();               // Get the Animator component for animation control
        timer = wanderTimer;                               // Initialize the wander timer
    }

    void Update()
    {
        // Check if zombie is dead and switch to Die state if necessary
        if (currentHealth <= 0 && !hasDied)
        {
            currentState = ZombieState.Die;  // Switch to Die state if health reaches 0
            hasDied = true;                  // Ensure this only happens once
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }

        // Handle behavior based on the current state
        switch (currentState)
        {
            case ZombieState.Wander:
                Wander();   // Call the wandering behavior
                break;
            case ZombieState.Chase:
                ChasePlayer();  // Call the chasing behavior
                break;
            case ZombieState.Attack:
                AttackPlayer();  // Call the attack behavior
                break;
            case ZombieState.Die:
                Die();  // Handle the zombie's death
                break;
        }
    }

    // Wander around randomly within a defined radius
    private void Wander()
    {
        timer += Time.deltaTime;  // Increment the wander timer

        // If the timer exceeds the wanderTimer, choose a new destination
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);  // Find a random position within the radius
            navMeshAgent.SetDestination(newPos);  // Set the new destination
            timer = 0;  // Reset the timer
        }

        // If the player is within the detection radius, switch to Chase state
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            currentState = ZombieState.Chase;
        }
    }

    // Chase the player if detected
    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);  // Set the player's position as the destination

        // If within attack range, switch to Attack state
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = ZombieState.Attack;
        }
        // If the player is out of detection range, switch back to Wander state
        else if (Vector3.Distance(transform.position, player.position) > detectionRadius)
        {
            currentState = ZombieState.Wander;
        }
    }

    // Attack the player if within range
    private void AttackPlayer()
    {
        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;  // Update the last attack time
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();  // Get the player's health component

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);  // Apply damage to the player
            }

            Debug.Log("Zombie valt aan en doet " + attackDamage + " schade!");  // Log the attack
        }

        // If the player moves out of attack range, switch back to Chase state
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = ZombieState.Chase;
        }
    }

    // Handle the zombie's death
    private void Die()
    {
        // Set the Animator bool parameter 'isDead' to trigger the death animation
        animator.SetBool("isDead", true);

        // Stop the zombie's movement
        navMeshAgent.isStopped = true;

        // Start a coroutine to remove the zombie after the death animation finishes
        StartCoroutine(RemoveZombieAfterDeath());
    }

    // Coroutine to wait for the death animation to finish before removing the zombie
    private IEnumerator RemoveZombieAfterDeath()
    {
        // Wait for the length of the current animation (death) to complete
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Debug.Log("Zombie is dood!");  // Log the zombie's death
        gameObject.SetActive(false);   // Deactivate the zombie
    }

    // Handle taking damage from external sources
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Reduce the zombie's health by the damage amount
        Debug.Log("Zombie neemt " + damage + " schade, huidige gezondheid: " + currentHealth);  // Log the damage taken
    }

    // Helper function to find a random position within the NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;  // Generate a random direction within a sphere
        randDirection += origin;  // Add the origin to the random direction

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);  // Find a valid position on the NavMesh

        return navHit.position;  // Return the random valid position
    }
}