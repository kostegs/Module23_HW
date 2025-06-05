using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private const int MouseButton = 0;

    [SerializeField] private MapMoveToMarker _mapMoveToMarker;        
    [SerializeField] private Example _Example;

    private void Awake()
    {
        IInputProcessor inputProcessor = new MouseInput(MouseButton);

        InputService inputService = new InputService(inputProcessor);
        
        _mapMoveToMarker.Initialize(inputService);
        _Example.Initialize(inputService);
    }
}