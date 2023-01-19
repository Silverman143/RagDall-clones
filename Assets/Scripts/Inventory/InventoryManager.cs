using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventoryItem> Items;


    public void Awake()
    {
        Instance = this;
    }

    public void AddItem(InventoryItem item)
    {
        Items.Add(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        Items.Remove(item);
    }
}
