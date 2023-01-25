using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    private InventoryObject previousInventory;
    bool createSlot = false;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    public void OnEnable()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }
        if (!createSlot)
        {
            createSlot = true;
            CreateSlots();
        }
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(); });
    }

    public abstract void CreateSlots();

    public void UpdateInventoryLinks()
    {
        int i = 0;
        foreach (var key in slotsOnInterface.Keys.ToList())
        {
            slotsOnInterface[key] = inventory.Container.Items[i];
            i++;
        }
    }


    public void Update()
    {
        if (previousInventory != inventory)
        {
            UpdateInventoryLinks();
        }
        previousInventory = inventory;
        slotsOnInterface.UpdateSlotDisplay();

    }
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (!trigger) { Debug.LogWarning("No EventTrigger component found!"); return; }
        var eventTrigger = new EventTrigger.Entry
        {
            eventID = type
        };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public virtual void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public virtual void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }
    public virtual void OnExitInterface()
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public virtual void OnExit()
    {
        MouseData.slotHoveredOver = null;
    }

    public virtual void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    private GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].Items.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }
    public virtual void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            Debug.Log("1");
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            Debug.Log("2");
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public virtual void OnDrag()
    {
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            if (_slot.Value.item.Id <= -1)
            {
                _slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = _slot.Value.Items.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
        }
    }
}