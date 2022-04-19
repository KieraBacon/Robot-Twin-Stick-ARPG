using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField]
    private QuestScriptable questItem;
    [SerializeField]
    private Door doorToOpen;
    [SerializeField]
    private bool returnToMenu;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController)
        {
            QuestScriptable found = playerController.inventory.FindItem(questItem.name) as QuestScriptable;
            if (found)
            {
                //playerController.inventory.DeleteItem(found);
                found.RemoveItem();

                if (doorToOpen)
                {
                    doorToOpen.Open();
                }
                if (returnToMenu)
                {
                    AppEvents.SetMouseCursorVisible(true);
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }
}
