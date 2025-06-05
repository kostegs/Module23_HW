using UnityEngine;

public class InputService
{
    private LayerMask _layerMask;
    private IInputProcessor _inputProcessor;

    public IInputProcessor InputProcessor => _inputProcessor;

    public InputService(IInputProcessor inputProcessor, LayerMask layerMask)
    {
        _inputProcessor = inputProcessor;
        _layerMask = layerMask;
    }

    public Vector3 GetCurrentCursorPosition() => _inputProcessor.GetCurrentCursorPosition();

    public bool IsButtonDown() => _inputProcessor.IsButtonDown();

    public Vector3 GetClickedPointOnMap()
    {
        if (IsButtonDown())
        {
            Vector3 currentCursorPosition = GetCurrentCursorPosition();
            Ray cameraRay = Camera.main.ScreenPointToRay(currentCursorPosition);

            if (Physics.Raycast(cameraRay.origin, cameraRay.direction, out RaycastHit hitInfo, Mathf.Infinity, _layerMask))
                return hitInfo.point;
        }

        return Vector3.zero;
    }
}
