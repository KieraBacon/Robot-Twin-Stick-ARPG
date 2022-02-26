using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private bool _cursorActive = false;
    public bool cursorActive => _cursorActive;

    protected virtual void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        AppEvents.onMouseCursorVisbilityChanged += OnMouseCursorVisibilityChanged;
    }

    private void OnDisable()
    {
        AppEvents.onMouseCursorVisbilityChanged -= OnMouseCursorVisibilityChanged;
    }

    private void OnMouseCursorVisibilityChanged(bool enabled)
    {
        if (enabled)
        {
            _cursorActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            _cursorActive = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
