using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;

public class DynamicInterface : UserInterface
{
    public GameObject inventoryPrefab;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEMS;

    public int NUMBER_OF_COLUMN;

    bool itemInfoActived = false;
    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit();});
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj);});
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });


            slotsOnInterface.Add(obj, inventory.Container.Items[i]);
        }
    }
    public void OnClick(GameObject obj)
    {
        itemInfoActived = !itemInfoActived;

        //if (!itemInfoActived)
        //{
        //    if (slotsOnInterface[obj].item.Id > -1)
        //    {
        //        foreach (var item in slotsOnInterface)
        //        {
        //            if (item.Value.item.Id == slotsOnInterface[obj].item.Id)
        //            {
        //                ItemInfoPanel.instance.itemInfoDisplay.SetActive(true);
        //                ItemInfoPanel.instance.SetItemDescriptionToText(item.Value.Items);
        //            }
        //        }
        //    }
        //}
        //else ItemInfoPanel.instance.itemInfoDisplay.SetActive(false);
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }
}