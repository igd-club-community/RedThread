using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    /* Item types */
    NoItem,
    Test
};

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> itemSlots;

    delegate void ItemFunction();
    Dictionary<Item, ItemFunction> ItemFunctions;
    void Start()
    {
        ItemFunctions = new Dictionary<Item, ItemFunction>()
        {
            {
                Item.Test,
                delegate ()
                {
                    Debug.Log("Test");
                }
            }
        };
    }

    public void OnItemSlotClicked(ItemSlot itemSlot)
    {
        if (ItemFunctions.ContainsKey(itemSlot.CurrentItem))
            ItemFunctions[itemSlot.CurrentItem]();
    }

    public void PutItem(Item item)
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.CurrentItem == Item.NoItem)
            {
                itemSlot.CurrentItem = item;
                break;
            }
        }
    }

    public void PutItem(ItemSlot itemContainer)
    {
        PutItem(itemContainer.CurrentItem);
    }

    public void RemoveItem(Item item)
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.CurrentItem == item)
            {
                itemSlot.CurrentItem = Item.NoItem;
                break;
            }
        }
    }
}
