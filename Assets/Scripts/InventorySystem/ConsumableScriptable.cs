using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Consumable", order = 1)]
public class ConsumableScriptable : ItemScriptable
{
    public int effect = 0;

    public override void UseItem(PlayerController playerController)
    {
        // Check to see if the player is at max health, then return
        // Health player with potion

        SetAmount(amount - 1);

        if (amount <= 0)
        {
            DeleteItem(playerController);
            playerController.inventory.DeleteItem(this);
        }
    }
}
