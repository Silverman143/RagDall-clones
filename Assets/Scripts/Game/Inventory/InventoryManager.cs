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

        ItemDrop.OnItemDrop.AddListener(InteractItems);
    }

    private void OnDisable()
    {
        ItemDrop.OnItemDrop.RemoveListener(InteractItems);
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
        Items.Add(item);
        AssignHolder(item);
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

    private void InteractItems(InventoryItemObject itemObj, ItemHolder holder)
    {
        holder.GetComponentInChildren<InventoryItemObject>().SetParent(itemObj.transform.parent);
        itemObj.SetParent(holder.transform);
    }
}
