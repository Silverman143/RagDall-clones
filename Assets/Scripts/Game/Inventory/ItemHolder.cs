using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHolder : MonoBehaviour, IDropHandler
{
    public int Id;
    private int _inventoryId;
    public bool _isActive = true;
    private RectTransform _rectransform;
    [SerializeField] private InventoryItemObject _itemObject;



    private void OnEnable()
    {
        if (!_itemObject)
        {
            _itemObject = GetComponentInChildren<InventoryItemObject>();
        }
        if (!_rectransform)
        {
            _rectransform = GetComponent<RectTransform>();
        }
    }

    public void SetInventoryId(int id)
    {
        _inventoryId = id;
    }

    public void SetItem(InventoryItem item)
    {
        _itemObject.gameObject.SetActive(true);
        if (item)
        {
            item.holderId = Id;
            item.inventoryId = _inventoryId;
        }
        _itemObject.SetItem(item);
        
    }

    public void UpdateInterface()
    {
        _itemObject.UpdateInterface();
    }

    public void OnDrop(PointerEventData eventData)
    {
        
        if (eventData.pointerDrag != null )
        {
            if (eventData.pointerDrag.TryGetComponent<InventoryItemObject>(out InventoryItemObject newItem))
            {
                //ItemDrop.OnItemDrop.Invoke(newItem, this);
                InventariesInteractionHandler.InventaryInteraction(this, newItem);
            }
        }
    }

    public bool IsEmpty()
    {
        if (_itemObject.IsEmpty())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public InventoryItem GetItem() => _itemObject.GetItem();
}
