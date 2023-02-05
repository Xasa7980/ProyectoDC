using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory; //Inventario al que iran los objetos
    public Attribute[] attributes;
    public int itemId;
    public static bool isEquipped;
    private void Start()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i] = new Attribute(this);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<GroundItem>(out GroundItem item))
        {
            switch (item.item.type)
            {
                case ItemType.Miscellaneous: if (inventory.AddItem(new Item(item.item), 1)) Destroy(other.gameObject); break;
                case ItemType.Drone: if (inventory.AddItem(new Item(item.item), 1)) Destroy(other.gameObject); break;
                case ItemType.Food: if (inventory.AddItem(new Item(item.item), 1)) Destroy(other.gameObject); break;
                case ItemType.Upgrade: if (inventory.AddItem(new Item(item.item), 1)) Destroy(other.gameObject); break;
            }
        }
    }
    private void OnEnable()
    {
        inventory.Load();
    }
    private void OnDisable()
    {
        inventory.Save();
    }
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, "was updated! Value is now ", attribute.value.ModifiedValue));
    }
    public void OnApplicationQuit() //Limpiamos el inventario
    {
        //inventory.Container.Clear();
    }
}
[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public Attributes type;
    public ModifiableInt value;
    public Attribute(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}
