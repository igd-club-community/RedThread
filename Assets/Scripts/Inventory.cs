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
    delegate void ItemFunction();
    Dictionary<Item, ItemFunction> ItemFunctions;

    private LevelController levelController;
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        ItemFunctions = new Dictionary<Item, ItemFunction>()
        {
            {
                Item.Test,
                delegate ()
                {
                    levelController.generateNeedCoffeEvent();
                }
            }
        };
    }

    public void OnItemSlotClicked(ItemSlot itemSlot)
    {
        if (ItemFunctions.ContainsKey(itemSlot.CurrentItem))
            ItemFunctions[itemSlot.CurrentItem]();
    }
}
