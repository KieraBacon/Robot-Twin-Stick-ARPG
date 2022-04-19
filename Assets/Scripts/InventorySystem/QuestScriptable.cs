using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Item", menuName = "Items/Quest Item", order = 1)]
public class QuestScriptable : ItemScriptable
{
    public override void UseItem(PlayerController playerController) {}
    public void RemoveItem()
    {
        SetAmount(amount - 1);
        
        if (amount <= 0)
        {
            DeleteItem(playerController);
            playerController.inventory.DeleteItem(this);
        }
    }
}
