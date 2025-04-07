using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // The enemy's current health point total
    public int currentHealth = 100; // Ensure it starts with some health
    public int damageAmount = 10; // Amount of damage to deal
    public float detectionRange = 5f; // Range to detect the player
    public float wanderRadius = 10f; // Radius within which to wander

    public enum States
    {
        Idle,
        Wandering,
        Seeking,
        Hunting,
        Dying
    }

    public States state;
    private NavMeshAgent agent;
    private Transform player; // Reference to the player's transform

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has the "Player" tag
    }

    private void Start()
    {
        state = States.Wandering;
        Wander();
    }

    private void Update()
    {
        switch (state)
        {
            case States.Wandering:
                if (Vector3.Distance(agent.destination, transform.position) < 1f)
                {
                    Wander();
                }
                CheckForPlayer();
                break;

            case States.Hunting:
                HuntPlayer();
                break;

            case States.Dying:
                HandleDeath();
                break;
        }
    }

    // Function to make the enemy wander
    private void Wander()
    {
        Vector3 newTarget = transform.position + new Vector3(Random.Range(-wanderRadius, wanderRadius), 0, Random.Range(-wanderRadius, wanderRadius));
        agent.SetDestination(newTarget);
        state = States.Wandering;
    }

    // Check if the player is within detection range
    private void CheckForPlayer()
    {
        if (Vector3.Distance(player.position, transform.position) <= detectionRange)
        {
            state = States.Hunting;
        }
    }

    // Function to hunt the player
    private void HuntPlayer()
    {
        agent.SetDestination(player.position);

        // Check if the enemy is close enough to the player to deal damage
        if (Vector3.Distance(player.position, transform.position) < 1.5f)
        {
            Damage(damageAmount);
            // Optional: Add logic for cooldown or delay between attacks
        }

        // If the player moves out of detection range, go back to wandering
        if (Vector3.Distance(player.position, transform.position) > detectionRange)
        {
            state = States.Wandering;
            Wander();
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            state = States.Dying; // Change to dying state
        }
    }

    private void HandleDeath()
    {
        Debug.Log($"{gameObject.name} is dying!");
        // Disable or destroy the enemy object, trigger death animation, etc.
        gameObject.SetActive(false); // Deactivating for simplicity
    }
}