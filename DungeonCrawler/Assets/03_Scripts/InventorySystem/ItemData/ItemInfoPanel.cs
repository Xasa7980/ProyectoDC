using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInfoPanel : ItemDisplayInfo
{
    public GameObject itemInfoDisplay;

    [SerializeField] TextMeshProUGUI itemName, itemDescription;
    [SerializeField] Image itemDisplay;
    public static ItemInfoPanel instance;
    private void Awake()
    {
        instance = this;
    }
    public override void SetItemDescriptionToText(ItemObject itemObject)
    {
        itemDisplay.sprite = itemObject.uiDisplay;
        itemName.text = itemObject.itemName;
        itemDescription.text = itemObject.itemDescription;
    }
}
