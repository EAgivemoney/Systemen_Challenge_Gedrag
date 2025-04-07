using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Basic Item Info")]
    [Tooltip("The name of the item.")]
    public string itemName = "New Item"; // Name of the item

    [Tooltip("The icon representing the item.")]
    public Sprite icon = null; // Item icon

    [Tooltip("Should this item be displayed in the inventory?")]
    public bool showInInventory = true; // Show in inventory?

    [Tooltip("Prefab reference for the item in the world.")]
    public GameObject objectPrefab; // Object reference

    [TextArea(1, 5)]
    [Tooltip("A brief description of the item.")]
    public string description; // Description of the item

    // Called when the item is pressed in the inventory
    public virtual void Use()
    {
        // Use the item
        // You could add checks here to validate if the item can be used
        Debug.Log($"{itemName} has been used.");
    }

    // Call this method to remove the item from inventory
    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}