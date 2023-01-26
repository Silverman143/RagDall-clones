using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHolder : MonoBehaviour, IDropHandler
{
    public int Id;
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

    public void SetItem(InventoryItem item)
    {
        _itemObject.gameObject.SetActive(true);
        _itemObject.SetItem(item);
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null )
        {
            if (eventData.pointerDrag.TryGetComponent<InventoryItemObject>(out InventoryItemObject newItem))
            {
                ItemDrop.OnItemDrop.Invoke(newItem, this);
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
