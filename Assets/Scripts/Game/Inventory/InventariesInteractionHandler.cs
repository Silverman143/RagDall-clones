using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventariesInteractionHandler : MonoBehaviour
{
    //public delegate void InventaryInteraction(ItemHolder holder, InventoryItemObject itemObject);
    //public static event InventaryInteraction OnItemsInteract;
    [SerializeField] public static int MaxItemAmount = 20;
    [SerializeField] private static CharacterInventory _characterInventory;

    private void Awake()
    {
        _characterInventory = FindObjectOfType<CharacterInventory>();
    }

    public static void InventaryInteraction(ItemHolder holder, InventoryItemObject itemObj)
    {
        InventoryItem objItem = itemObj.GetItem();
        InventoryItem holderItem = holder.GetItem();

        if (objItem?.name == holderItem?.name && objItem?.type == ItemType.Consumable)
        {
            int amount = objItem.amount+holderItem.amount;
            if (amount > MaxItemAmount)
            {
                objItem.amount = 20;
                holderItem.amount = amount - 20;
                DataBaseHandler.UploadItemData(objItem);
                DataBaseHandler.UploadItemData(holderItem);

            }
            else
            {
                objItem.amount = amount;

                DataBaseHandler.UploadItemData(objItem);
                DataBaseHandler.RemoveItem(holderItem);

                holderItem = null;
            }

            itemObj.GetHolder().SetItem(holderItem);
            holder.SetItem(objItem);
        }
        else
        {
            if (objItem) DataBaseHandler.UploadItemData(objItem);
            if (holderItem) DataBaseHandler.UploadItemData(holderItem);

            itemObj.GetHolder().SetItem(holderItem);
            holder.SetItem(objItem);

            
        }
        
    }

    public static void AddToCharacterInventory(InventoryItem item)
    {

    } 
}
