using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootHandler : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> _possibleLoot;
    [SerializeField] private List<InventoryItem> _loot;

    public List<InventoryItem> GetLoot() => _loot;
    public bool IsEmpty()
    {
        if(_loot.Count > 0)
        {
            return false;
        }
        return true;
    }
    private void Awake()
    {
        DetermineLoot();
    }
    //private void OnEnable()
    //{
        
    //}

    private void DetermineLoot()
    {
        foreach(InventoryItem item in _possibleLoot)
        {
            //if(Random.RandomRange(0, 1) == 1)
            //{
            //    InventoryItem newItem = ScriptableObject.Instantiate<InventoryItem>(item);
            //    newItem.amount = Random.RandomRange(0, 2);
            //    _loot.Add(newItem);
            //}

            InventoryItem newItem = ScriptableObject.Instantiate<InventoryItem>(item);
            newItem.amount = Random.RandomRange(1, 2);
            _loot.Add(newItem);
        }
    }
}
