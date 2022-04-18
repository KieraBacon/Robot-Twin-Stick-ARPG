using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlotAmountCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text AmountText;
    private ItemScriptable Item;

    private void Awake()
    {
        HideWidget();
    }

    public void ShowWidget()
    {
        gameObject.SetActive(true);
    }

    public void HideWidget()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(ItemScriptable item)
    {
        if (!item.stackable) return;
        Item = item;
        ShowWidget();
        Item.onAmountChanged += OnAmountChange;
        OnAmountChange();
    }

    private void OnAmountChange()
    {
        AmountText.text = Item.amount.ToString();
    }

    private void OnDisable()
    {
        if (Item) Item.onAmountChanged -= OnAmountChange;
    }
}
