using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemsLoader : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> _itemsBases;
    [SerializeField] private List<InventoryItem> _allItems;
    [SerializeField] private List<InventoryManager> _allInventories;
    [SerializeField] private CharacterInventory _characterInventry;

    private void Start()
    {
        //_itemsBases = Resources.LoadAll("InventoryItems"):
        _allItems = DataBaseHandler.GetItems();
        _characterInventry = FindObjectOfType<CharacterInventory>();
        _allInventories = FindObjectsOfType<InventoryManager>().ToList<InventoryManager>();
        _allInventories.Remove(_characterInventry);
        SetIcons();
        //UploadCharacterInventory();
    }

    private void SetIcons()
    {
        foreach(InventoryItem item in _allItems)
        {
            foreach(InventoryItem itemBase in _itemsBases)
            {
                if(itemBase.name == item.name)
                {
                    item.icon = itemBase.icon;
                }
            }
        }
    }

    //private void UploadCharacterInventory()
    //{
    //    foreach (InventoryItem item in _allItems)
    //    {
    //        if (item.inventoryId == _characterInventry.Id)
    //        {
    //            _characterInventry.AddItem(item);
    //        }
    //    }
    //}

    public List<InventoryItem> GetInventoryItems(int id)
    {
        List<InventoryItem> items = new List<InventoryItem>();
        foreach(InventoryItem item in _allItems)
        {
            if (item.inventoryId == id)
            {
                items.Add(item);
            }
        }
        return items;
    }
}
