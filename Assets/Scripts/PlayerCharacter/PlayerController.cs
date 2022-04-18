using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isFiring;
    public bool isReloading;
    public bool isInAir;
    public bool isRunning;
    public bool isAiming;
    public bool inInventory;

    [Header("Inventory")]
    public InventoryComponent inventory;
    public GameUIController uiController;
    public WeaponHolder weaponHolder;
    public HealthComponent healthComponent;

    private void Awake()
    {
        inventory = GetComponent<InventoryComponent>();
        uiController = FindObjectOfType<GameUIController>();
        weaponHolder = GetComponent<WeaponHolder>();
        healthComponent = GetComponent<HealthComponent>();
    }

    public void OnInventory(InputValue value)
    {
        inInventory = !inInventory;
        SetInventoryOpen(inInventory);
    }

    private void SetInventoryOpen(bool value)
    {
        if (value)
        {
            uiController.EnableInventoryMenu();
        }
        else
        {
            uiController.EnableGameMenu();
        }
    }
}
