using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance for the EquipmentManager
    public static EquipmentManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EquipmentManager>();
            }
            return _instance;
        }
    }
    private static EquipmentManager _instance;

    void Awake()
    {
        // Ensure this instance is the only one
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }

    #endregion

    private Equipment[] currentEquipment; // Array to hold currently equipped items
    private GameObject[] currentObjects;   // Array to hold currently equipped GameObjects

    // Callback for when an item is equipped
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public event OnEquipmentChanged onEquipmentChanged;

    private Inventory inventory; // Reference to the inventory

    void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentObjects = new GameObject[numSlots];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

    // Get the equipment in a specific slot
    public Equipment GetEquipment(EquipmentSlot slot)
    {
        return currentEquipment[(int)slot];
    }

    // Equip a new item
    public void Equip(Equipment newItem)
    {
        Equipment oldItem = null;
        int slotIndex = (int)newItem.equipSlot;

        // If there was already an item in the slot, put it back in the inventory
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory?.Add(oldItem); // Use null conditional to avoid exceptions
        }

        currentEquipment[slotIndex] = newItem; // Equip the new item
        Debug.Log(newItem.name + " equipped!");

        // Trigger the equipment changed event
        onEquipmentChanged?.Invoke(newItem, oldItem);

        // Equip game object checking the type
        EquipItemGameObject(newItem, oldItem);
    }

    // Equip item game object based on its type
    private void EquipItemGameObject(Equipment newItem, Equipment oldItem)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (newItem.equipSlot == EquipmentSlot.Weapon)
        {
            playerController.armsHolder.SetActive(false);
            playerController.weaponHolder.transform.Find(newItem.name)?.gameObject.SetActive(true);
        }
    }

    // Unequip an item at a specific slot index
    void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory?.Add(oldItem); // Use null conditional to avoid exceptions
            currentEquipment[slotIndex] = null;

            if (currentObjects[slotIndex] != null)
                Destroy(currentObjects[slotIndex].gameObject);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.weaponHolder.transform.Find(oldItem.name)?.gameObject.SetActive(false);
            playerController.armsHolder.SetActive(true);
        }
    }

    // Unequip all items
    void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }
}