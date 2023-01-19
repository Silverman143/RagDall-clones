using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Item/Create new Item")]
public class InventoryItem : ScriptableObject
{
    public int id;
    public string name;
    [Range(0, 20)]
    public int amount;
    public Sprite icon;
}
