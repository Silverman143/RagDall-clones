using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

// Events.

public class ItemDragStart : UnityEvent<InventoryItemObject>
{
    public static ItemDragStart OnItemsDragStart = new ItemDragStart();
}

public class ItemDragEnd : UnityEvent
{
    public static ItemDragEnd OnItemsDragEnd = new ItemDragEnd();
}

public class ItemDrop: UnityEvent<InventoryItemObject, ItemHolder>
{
    public static ItemDrop OnItemDrop = new ItemDrop();
}


// Main.
public class InventoryManager : MonoBehaviour
{
    public int Id;
    public List<InventoryItem> Items;
    public List<ItemHolder> _holders;

    private ItemsLoader _loader;


    public virtual void Start()
    {
        _holders = GetComponentsInChildren<ItemHolder>().ToList<ItemHolder>();
        //Items = DataBaseHandler.GetItems();

        //foreach (InventoryItem item in Items)
        //{
        //    Debug.Log(item.icon);
        //}

        //SetItems(Items);
    }

    private void OnEnable()
    {
        _holders = GetComponentsInChildren<ItemHolder>().ToList<ItemHolder>();
        _loader = FindObjectOfType<ItemsLoader>();
        Items = _loader.GetInventoryItems(Id);

        foreach (InventoryItem item in Items)
        {
            AssignHolder(item);
        }
        foreach (ItemHolder holder in _holders)
        {
            holder.SetInventoryId(Id);
        }


    }

    public virtual void SetItems(List<InventoryItem> items)
    {
        Items = items;

        foreach(InventoryItem item in Items)
        {
            AssignHolder(item);
        }
    }

    public virtual void AddItem(InventoryItem item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (item.name == Items[i].name && Items[i].amount < InventariesInteractionHandler.MaxItemAmount)
            {
                int amount = Items[i].amount + item.amount;
                if (amount < InventariesInteractionHandler.MaxItemAmount)
                {
                    Items[i].amount = amount;

                }
            }
        }
    }

    public virtual void RemoveItem(InventoryItem item)
    {
        Items.Remove(item);
    }

    public virtual void AssignHolder(InventoryItem item)
    {
        foreach(ItemHolder holder in _holders)
        {
            if (holder.Id == item.holderId)
            {
                holder.SetItem(item);
            }
        }
    }
}
