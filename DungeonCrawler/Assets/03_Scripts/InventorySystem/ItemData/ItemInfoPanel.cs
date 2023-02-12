using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInfoPanel : ItemDisplayInfo
{
    public GameObject itemInfoDisplay;

    public TextMeshProUGUI itemName, itemDescription;
    public Image itemDisplay;

    public ItemInfoPanel(TextMeshProUGUI _itemName, TextMeshProUGUI _itemDescription, Image _itemDisplay) 
    {
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemDisplay = _itemDisplay;
    }
    public override void SetItemDescriptionToText(ItemObject itemObject)
    {
        itemDisplay.sprite = itemObject.uiDisplay;
        itemName.text = itemObject.itemName;
        itemDescription.text = itemObject.itemDescription;
    }
}
