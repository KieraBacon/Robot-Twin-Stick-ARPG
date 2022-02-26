using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public delegate void OnWeaponEequippedEvent(WeaponComponent weaponComponent);
    public static event OnWeaponEequippedEvent OnWeaponEquipped;

    public static void InvokeOnWeaponEquipped(WeaponComponent weaponComponent)
    {
        OnWeaponEquipped?.Invoke(weaponComponent);
    }
}
