using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScriptable : ScriptableObject
{
    public enum Category
    {
        None,
        Weapon,
        Consumable,
        Equipment,
        Ammo,
    }

    public delegate void AmountChangeEventHandler();
    public event AmountChangeEventHandler onAmountChanged;

    public delegate void ItemDestroyedEventHandler();
    public event ItemDestroyedEventHandler onItemDestroyed;

    public delegate void ItemDroppedEventHandler();
    public event ItemDroppedEventHandler onItemDropped;

    public string name = "Item";
    public Category category = Category.None;
    public GameObject itemPrefab;
    public bool stackable;
    public int maxStackSize;
    public int amount = 1;

    public PlayerController playerController { get; private set; }

    public virtual void Initialize(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public abstract void UseItem(PlayerController playerController);
    public virtual void DeleteItem(PlayerController playerController)
    {
        onItemDestroyed?.Invoke();
    }

    public virtual void DropItem(PlayerController playerController)
    {
        onItemDropped?.Invoke();
    }

    public void IncrementAmount(int change)
    {
        amount += change;
        onAmountChanged?.Invoke();
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
        onAmountChanged?.Invoke();
    }
}
