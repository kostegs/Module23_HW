using UnityEngine;

public interface IInputProcessor
{
    public bool IsButtonDown();

    public Vector3 GetCurrentCursorPosition();
}
