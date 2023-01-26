using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDB : MonoBehaviour
{
    public List<InventoryItem> _items;

    public void TestDB()
    {
       _items =  DataBaseHandler.GetItems();
    }
}
