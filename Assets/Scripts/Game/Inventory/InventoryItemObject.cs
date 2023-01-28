using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItemObject : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private RectTransform _recTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private InventoryItem _item;
    [SerializeField] private TextMeshProUGUI _counterTMP;

    private Canvas _canvas;

    public bool _isEmpty = true;


    private void OnEnable()
    {
        if (!_recTransform)
        {
            _recTransform = GetComponent<RectTransform>();
        }
        if (!_canvasGroup)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        if (!_canvas)
        {
            _canvas = FindObjectOfType<Canvas>();
        }
        //if (!_counterTMP)
        //{
        //    _counterTMP = GetComponentInChildren<TextMeshProUGUI>();
        //}

        if (_item == null)
        {
            _icon.color = new Color32(255, 255, 225, 0);
        }

        ItemDragStart.OnItemsDragStart.AddListener(SetNotInteractable);
        ItemDragEnd.OnItemsDragEnd.AddListener(SetInteractable);
    }

    private void OnDisable()
    {
        ItemDragStart.OnItemsDragStart.RemoveListener(SetNotInteractable);
        ItemDragEnd.OnItemsDragEnd.RemoveListener(SetInteractable);
    }

    public InventoryItem GetItem()
    {
        return _item;
    }

    public ItemHolderMono GetHolder() => transform.parent.GetComponent<ItemHolderMono>();

    //public void SetParent(Transform parent)
    //{

    //    transform.parent = parent;
    //    _recTransform.anchoredPosition = Vector2.zero;
    //    if (_item)
    //    {
    //        int itemId = _item.id;
    //        int holderId = parent.GetComponent<ItemHolderMono>().Id;
    //        DataBaseHandler.ChangeItemHolderId(itemId, holderId);
    //    }
    //}

    public void RemoveItem()
    {
        _icon.sprite = null;
        _item = null;
        _recTransform.anchoredPosition = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void SetNotInteractable(InventoryItemObject obj)
    {
        if(obj != this)
        {
            _canvasGroup.blocksRaycasts = false;
        }
    }

    private void SetInteractable()
    {
        _canvasGroup.blocksRaycasts = true;
    }

    public void SetPosition(Vector2 pos)
    {
        _recTransform.anchoredPosition = pos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        ItemDragStart.OnItemsDragStart.Invoke(this);
        transform.parent.transform.SetAsLastSibling();
        Debug.Log("Drug begin");
    }

    public void OnDrag(PointerEventData eventData)
    {
        _recTransform.anchoredPosition += eventData.delta/_canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drug end");

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //ItemDragStart.OnItemsDragStart.Invoke(this);
        //transform.parent.transform.SetAsLastSibling();
    }

    public void SetItem(InventoryItem item)
    {

        if (item)
        {
            _item = item;
            //_icon = GetComponentInChildren<Image>();
            _icon.sprite = _item.icon;
            _icon.color = new Color32(255, 255, 225, 225);
            _counterTMP.text = _item.amount.ToString();
            DataBaseHandler.UploadItemData(_item);
            _isEmpty = false;
        }
        else
        {
            _item = null;
            _icon.color = new Color32(255, 255, 225, 0);
            _counterTMP.text = "";
            _isEmpty = true;
        }
        

    }

    public void UpdateInterface()
    {
        _icon.sprite = _item?.icon;
        if (_item)
        {
            _icon.color = new Color32(255, 255, 225, 225);
            _counterTMP.text = _item?.amount.ToString();
        }
        else
        {
            _icon.color = new Color32(255, 255, 225, 0);
        }
    }

    public bool IsEmpty() => _isEmpty;

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer UP");
        _canvasGroup.blocksRaycasts = true;
        _recTransform.anchoredPosition = Vector2.zero;
        ItemDragEnd.OnItemsDragEnd.Invoke();
    }
}
