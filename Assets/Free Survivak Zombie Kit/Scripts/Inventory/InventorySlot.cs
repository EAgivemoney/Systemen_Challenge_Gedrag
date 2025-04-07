using UnityEngine;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;              // The icon representing the item
    [SerializeField] private Button removeButton;      // Button to remove the item

    private Item item;                                // Current item in the slot

    // Add item to the slot
    public void AddItem(Item newItem)
    {
        item = newItem;

        if (item != null)
        {
            icon.sprite = item.icon;                  // Set icon sprite
            icon.enabled = true;                       // Show icon
            removeButton.interactable = true;         // Enable remove button
        }
        else
        {
            ClearSlot();                              // Clear slot if no item
        }
    }

    // Clear the slot
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;                           // Clear icon
        icon.enabled = false;                         // Hide icon
        removeButton.interactable = false;           // Disable remove button
    }

    // If the remove button is pressed, this function will be called.
    public void RemoveItemFromInventory()
    {
        if (item != null)
        {
            Inventory.instance.Remove(item);          // Remove item from inventory
            ClearSlot();                              // Clear slot after removal
        }
        else
        {
            Debug.LogWarning("Attempted to remove an item that is null."); // Error handling
        }
    }

    public void Drop()
    {
        if (item != null)
        {
            Debug.Log("Dropping item: " + item.name);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Transform dropPos = player.GetComponent<PlayerController>().dropHolder.transform;

            GameObject drop = Instantiate(item.objectPrefab, dropPos.position, Quaternion.identity);
            Inventory.instance.Remove(item);           // Remove item from inventory
            ClearSlot();                               // Clear slot after dropping
        }
        else
        {
            Debug.LogWarning("Attempted to drop an item that is null."); // Error handling
        }
    }

    // Use the item
    public void UseItem()
    {
        if (item != null)
        {
            item.Use();                               // Call the item's Use method
        }
        else
        {
            Debug.LogWarning("Attempted to use an item that is null."); // Error handling
        }
    }
}