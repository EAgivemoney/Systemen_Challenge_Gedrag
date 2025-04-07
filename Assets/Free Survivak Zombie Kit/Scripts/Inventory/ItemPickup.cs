using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Item to put in the inventory if picked up

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            Debug.Log("Player found item: " + item.name);
            ShowPickupPrompt();

            if (Input.GetKeyDown(KeyCode.P))
            {
                // Open inventory to prevent null object bugs
                var playerController = playerCollider.GetComponent<PlayerController>();
                if (playerController != null && playerController.inventory != null)
                {
                    playerController.inventory.SetActive(true); // Show inventory
                    PickUp();
                }
                else
                {
                    Debug.LogWarning("PlayerController or Inventory not found on player.");
                }
            }
        }
    }

    private void ShowPickupPrompt()
    {
        // Implement UI feedback here (e.g., show message to the player)
        Debug.Log("Press 'P' to pick up " + item.name);
    }

    // Pick up the item
    private void PickUp()
    {
        Debug.Log("Picked up " + item.name);
        Inventory.instance.Add(item); // Add to inventory

        // Optional: Close the inventory after picking up the item
        // Inventory.instance.Close();

        Destroy(gameObject); // Destroy item from scene
    }

    private void OnTriggerExit(Collider playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            // Optional: Hide pickup prompt
            Debug.Log("Player left the item pickup area.");
        }
    }
}