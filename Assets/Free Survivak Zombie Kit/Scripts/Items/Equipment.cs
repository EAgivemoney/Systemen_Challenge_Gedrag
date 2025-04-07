using UnityEngine;

/* 
 * Represents an item that can be equipped to increase armor or damage.
 * This class inherits from Item and overrides the Use method to handle 
 * equipping the item.
 */
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipments")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;  // The slot to equip the item in
    public int armorModifier;         // The amount of armor this equipment provides
    public int damageModifier;        // The amount of damage this equipment adds

    // Called when the item is used from the inventory
    public override void Use()
    {
        // Ensure EquipmentManager is initialized before trying to equip
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.Equip(this); // Equip the item
            RemoveFromInventory();                  // Remove the item from the inventory after use
        }
        else
        {
            Debug.LogWarning("EquipmentManager instance is not available!");
        }
    }
}

/// <summary>
/// Enum for defining the different equipment slots available.
/// </summary>
public enum EquipmentSlot
{
    Head,    // Head armor
    Chest,   // Chest armor
    Legs,    // Leg armor
    Weapon,  // Weapon slot
    Shield,  // Shield slot
    Feet     // Foot armor
}