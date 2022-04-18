using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquippableScriptable : ItemScriptable
{
    public bool Equipped
    {
        get => equipped;
        set
        {
            if (equipped == value) return;

            equipped = value;
            onEquippedStatusChanged?.Invoke();
        }
    }
    private bool equipped = false;
    public delegate void EquipStatusChangeEventHandler();
    public event EquipStatusChangeEventHandler onEquippedStatusChanged;

    public override void UseItem(PlayerController playerController)
    {
        Equipped = !Equipped;
    }
}
