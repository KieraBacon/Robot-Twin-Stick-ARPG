using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : GameHUDWidget
{
    private PlayerController playerController;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void Close()
    {
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>();

        playerController.OnPause(null);
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
