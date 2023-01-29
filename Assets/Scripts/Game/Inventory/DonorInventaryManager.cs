using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonorInventaryManager : InventoryManager
{
    public bool _reusable;

    public override void Start()
    {
    }

    public override void OnEnable()
    {
        if (_reusable)
        {
            // Load from database
        }

    }

    public void AddItems(List<InventoryItem> items)
    {
        Items = items;
    }
}
