using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon", order = 2)]
public class WeaponScriptable : EquippableScriptable
{
    public WeaponStats weaponStats;

    public override void UseItem(PlayerController playerController)
    {
        // Check to see if the player has this weapon equipped
        bool thisEquipped = Equipped;

        // Check to see if the player has any weapon equipped, and if so, unequip it
        if (playerController.weaponHolder.equippedWeapon)
        {
            playerController.weaponHolder.UnequipWeapon();
        }

        // If the player had not had this weapon equipped, equip it
        if (!thisEquipped)
        {
            playerController.weaponHolder.EquipWeapon(this);
        }
    }
}
