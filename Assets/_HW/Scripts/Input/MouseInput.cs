using UnityEngine;

public class MouseInput : IInputProcessor
{
    private int _mouseButton;

    public MouseInput(int mouseButton)
        => _mouseButton = mouseButton;    

    public Vector3 GetCurrentCursorPosition() => Input.mousePosition;

    public bool IsButtonDown() => Input.GetMouseButtonDown(_mouseButton);    
}
