using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    // Delegate for item change notifications
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    [SerializeField] private int space = 10; // Amount of item spaces
    [SerializeField] private int gold; // Amount of gold

    // Current list of items in the inventory
    public List<Item> items = new List<Item>();

    // Cached references to UI components
    private Text goldText;

    private void Start()
    {
        // Cache the gold UI text component
        goldText = GameObject.Find("Canvas").transform.Find("Inventory").Find("Gold").Find("Value").GetComponent<Text>();
        UpdateGoldUI(); // Initialize the gold UI
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = gold.ToString();
        }
    }

    // Add a new item if enough room
    public void Add(Item item)
    {
        if (item.showInInventory)
        {
            if (items.Count >= space)
            {
                Debug.LogWarning("Not enough room in inventory for: " + item.name);
                // Optional: Provide UI feedback for the player
                return;
            }

            items.Add(item);
            onItemChangedCallback?.Invoke(); // Simplified callback invocation
        }
    }

    // Remove an item
    public void Remove(Item item)
    {
        if (items.Remove(item))
        {
            onItemChangedCallback?.Invoke(); // Simplified callback invocation
        }
    }

    // Method to add gold
    public void AddGold(int amount)
    {
        if (amount < 0)
            return; // Prevent adding negative gold

        gold += amount;
        UpdateGoldUI();
    }

    // Method to remove gold
    public void RemoveGold(int amount)
    {
        if (amount < 0)
            return; // Prevent removing negative gold

        gold = Mathf.Max(gold - amount, 0); // Prevent negative gold
        UpdateGoldUI();
    }
}