using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject inventoryPrefab;
    public GameObject objSelected;
    [SerializeField] InventoryObject otherInventory;
    [SerializeField] int X_START;
    [SerializeField] int Y_START;
    [SerializeField] int X_SPACE_BETWEEN_ITEM;
    [SerializeField] int Y_SPACE_BETWEEN_ITEMS;

    [SerializeField] int NUMBER_OF_COLUMN;

    [SerializeField] GameObject itemInfoPanel;
    [SerializeField] TextMeshProUGUI itemName, itemDescription;
    [SerializeField] Image itemDisplay;
    bool itemInfoActived = false;

    [SerializeField] bool hasTransition;
    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            if (hasTransition)
            {
                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(); });
                AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
                AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
                AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(); });
            }
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });


            slotsOnInterface.Add(obj, inventory.Container.Items[i]);
        }
    }
    public void OnClick(GameObject obj)
    {
        
        itemInfoActived = true;
        ItemInfoPanel info = new ItemInfoPanel(itemName, itemDescription,itemDisplay);
        objSelected = obj;
        if (itemInfoActived)
        {
            if (slotsOnInterface[obj].item.Id > -1)
            {
                foreach (var item in slotsOnInterface)
                {
                    if (item.Value.item.Id == slotsOnInterface[obj].item.Id)
                    {
                        itemInfoPanel.SetActive(true);
                        info.SetItemDescriptionToText(item.Value.Items);
                    }
                }
            }
        }
    }
    public void PurchaseItem()
    {
        if (slotsOnInterface[objSelected].item.Id > -1)
        {
            foreach (var item in slotsOnInterface)
            {
                if (item.Value.item.Id == slotsOnInterface[objSelected].item.Id)
                {
                    otherInventory.AddItem(item.Value.item, item.Value.amount);
                    slotsOnInterface[objSelected].RemoveItem();
                    return;
                }
            }
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }
}