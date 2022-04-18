using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon", order = 2)]
public class WeaponScriptable : EquippableScriptable
{
    public WeaponStats weaponStats;

    public override void UseItem(PlayerController playerController)
    {
        if (Equipped)
        {
            playerController.weaponHolder.UnequipWeapon();
            // Unequip from inventory here
            // Remove from controller here
        }
        else
        {
            // Invote OnWeaponEquipped from player here for inventory
            // Equip weapon from weapon holder on payerController

            playerController.weaponHolder.EquipWeapon(this);
        }

        base.UseItem(playerController);
    }
}
