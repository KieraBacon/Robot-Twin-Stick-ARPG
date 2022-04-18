using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Consumable", menuName = "Items/Health Consumable", order = 1)]
public class HealthConsumableScriptable : ConsumableScriptable
{
    public override void UseItem(PlayerController playerController)
    {
        if (playerController.healthComponent.CurrentHealth >= playerController.healthComponent.MaxHealth) return;

        playerController.healthComponent.RestoreHealth(effect);
        base.UseItem(playerController);
    }
}
