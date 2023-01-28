using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHolderMono : MonoBehaviour, IDropHandler
{
    public int Id;
    public int _inventoryId;
    public bool IsActive = true;
    public RectTransform _rectransform;
    [SerializeField] public InventoryItemObject _itemObject;



    public virtual void OnEnable()
    {
        if (!_itemObject)
        {
            _itemObject = GetComponentInChildren<InventoryItemObject>();
        }
        if (!_rectransform)
        {
            _rectransform = GetComponent<RectTransform>();
        }

        UpdateInterface();
    }

    public virtual void SetInventoryId(int id)
    {
        _inventoryId = id;
    }

    public virtual void SetItem(InventoryItem item)
    {
        _itemObject.gameObject.SetActive(true);
        if (item)
        {
            item.holderId = Id;
            item.inventoryId = _inventoryId;
        }
        _itemObject.SetItem(item);
        
    }

    public virtual void UpdateInterface()
    {
        _itemObject.UpdateInterface();
    }

    public virtual void OnDrop(PointerEventData eventData)
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

    public virtual bool IsEmpty()
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

    public virtual InventoryItem GetItem() => _itemObject.GetItem();
}
