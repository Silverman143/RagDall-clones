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



// Main.
public class InventoryManager : MonoBehaviour
{
    public int Id;
    public List<InventoryItem> Items;
    public List<ItemHolderInventary> _holders;

    private ItemsLoader _loader;


    public virtual void Start()
    {
        //_holders = GetComponentsInChildren<ItemHolderInventary>().ToList<ItemHolderInventary>();
        //Items = DataBaseHandler.GetItems();

        //foreach (InventoryItem item in Items)
        //{
        //    Debug.Log(item.icon);
        //}

        //SetItems(Items);

        _loader = FindObjectOfType<ItemsLoader>();
        Items = _loader.GetInventoryItems(Id);

        foreach (InventoryItem item in Items)
        {
            AssignHolder(item);
        }
        foreach (ItemHolderInventary holder in _holders)
        {
            holder.SetInventoryId(Id);
        }
    }

    public virtual void OnEnable()
    {
        //_holders = GetComponentsInChildren<ItemHolderInventary>().ToList<ItemHolderInventary>();
        _loader = FindObjectOfType<ItemsLoader>();
        LoadData();
    }

    private void LoadData()
    {
        Items = new List<InventoryItem>();
        Items = _loader.GetInventoryItems(Id);

        foreach (InventoryItem item in Items)
        {
            AssignHolder(item);
        }
        foreach (ItemHolderInventary holder in _holders)
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
        bool complete = false;

        if (item.type == ItemType.Consumable)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (item.name == Items[i].name && Items[i].amount < InventariesInteractionHandler.MaxItemAmount)
                {
                    int amount = Items[i].amount + item.amount;
                    if (amount < InventariesInteractionHandler.MaxItemAmount)
                    {
                        Items[i].amount = amount;
                        item = null;
                        DataBaseHandler.UploadItemData(Items[i]);
                        complete = true;
                        break;
                    }
                    else
                    {
                        Items[i].amount = InventariesInteractionHandler.MaxItemAmount;
                        item.amount = amount - InventariesInteractionHandler.MaxItemAmount;
                        DataBaseHandler.UploadItemData(Items[i]);
                    }


                }
            }
        }
        
        if (item)
        {
            item.inventoryId = Id;

            for(int i =0; i< _holders.Count; i++)
            {
                if (_holders[i].IsEmpty() && _holders[i].IsActive)
                {
                    int id = DataBaseHandler.AddItem(item);
                    item.id = id;
                    _holders[i].SetItem(item);
                    Items.Add(item);
                    complete = true;
                    break;
                }
            }
        }

        if (!complete == true) Debug.Log("No place for object in items");
    }

    public virtual void RemoveItem(InventoryItem item)
    {
        Items.Remove(item);
    }

    public virtual void AssignHolder(InventoryItem item)
    {
        foreach(ItemHolderInventary holder in _holders)
        {
            if (holder.Id == item.holderId)
            {
                holder.SetItem(item);
            }
        }
    }
}
