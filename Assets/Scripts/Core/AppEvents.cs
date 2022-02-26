using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEvents
{
    public delegate void MouseCursorVisibilityChangedEventHandler(bool enabled);
    public static event MouseCursorVisibilityChangedEventHandler onMouseCursorVisbilityChanged;

    public static void SetMouseCursorVisible(bool visible)
    {
        onMouseCursorVisbilityChanged?.Invoke(visible);
    }
}
