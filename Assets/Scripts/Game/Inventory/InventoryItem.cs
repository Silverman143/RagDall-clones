using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    Consumable, Equipment, Weapon, Quest
}

public enum ItemName
{
    Board, Pine, 
}

[CreateAssetMenu(fileName ="New Item", menuName = "Item/Create new Item")]
public class InventoryItem : ScriptableObject
{
    public int id;
    public int level;
    public int strength;
    public int inventoryId;
    public int holderId;
    public ItemName name;
    public ItemType type;
    [Range(0, 20)]
    public int amount;
    public Sprite icon;
}
