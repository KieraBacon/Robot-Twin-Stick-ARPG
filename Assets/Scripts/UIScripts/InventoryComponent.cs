using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    [SerializeField] private List<ItemScriptable> Items = new List<ItemScriptable>();

    private PlayerController Controller;
    
    private void Awake()
    {
        Controller = GetComponent<PlayerController>();
    }

    public List<ItemScriptable> GetItemList() => Items;

    public int GetItemCount() => Items.Count;

    public ItemScriptable FindItem(string itemName)
    {
        return Items.Find((invItem) => invItem.name == itemName);
    }

    public void AddItem(ItemScriptable item, int amount = 0)
    {
        int itemIndex = Items.FindIndex(listItem => listItem.name == item.name);
        if (itemIndex != -1)
        {
            ItemScriptable listItem = Items[itemIndex];
            if (listItem.stackable && listItem.amount < listItem.maxStackSize)
            {
                listItem.IncrementAmount(item.amount);
            }
        }
        else
        {
            if (item == null) return;

            ItemScriptable itemClone = Instantiate(item);
            itemClone.Initialize(Controller);
            itemClone.SetAmount(amount <= 1 ? item.amount : amount);
            Items.Add(itemClone);
        }
    }

    public void DeleteItem(ItemScriptable item)
    {
        Debug.Log($"{item.name} deleted!");
        int itemIndex = Items.FindIndex(listItem => listItem.name == item.name);
        if (itemIndex == -1) return;

        Items.Remove(item);
    }

    public List<ItemScriptable> GetItemsOfCategory(ItemScriptable.Category itemCategory)
    {
        if (Items == null || Items.Count <= 0) return null;

        return itemCategory == ItemScriptable.Category.None ? Items : 
            Items.FindAll(item => item.category == itemCategory);
    }
}
