using UnityEngine;

/* This object manages the inventory UI. */

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;  // The entire UI
    [SerializeField] private Transform itemsParent;     // The parent object of all the items

    private Inventory inventory;                        // Our current inventory

    void Start()
    {
        inventory = Inventory.instance;

        // Subscribe to the callback for inventory changes
        inventory.onItemChangedCallback += UpdateUI;
    }

    // Check to see if we should open/close the inventory
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            ToggleInventory();
        }
    }

    // Toggle the visibility of the inventory UI
    private void ToggleInventory()
    {
        bool isActive = !inventoryUI.activeSelf;
        inventoryUI.SetActive(isActive);

        if (isActive)
        {
            UpdateUI(); // Refresh UI when opened
        }
    }

    // Update the inventory UI by:
    //      - Adding items
    //      - Clearing empty slots
    // This is called using a delegate on the Inventory.
    public void UpdateUI()
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}