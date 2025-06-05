using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private const int MouseButton = 0;

    [SerializeField] private InputFlagService _inputFlagService;        
    [SerializeField] private LayerMask _walkableMask;
    [SerializeField] private InputExample _inputExample;

    private void Awake()
    {
        IInputProcessor inputProcessor = new MouseInput(MouseButton);

        InputService inputService = new InputService(inputProcessor, _walkableMask);
        
        _inputFlagService.Initialize(inputService);
        _inputExample.Initialize(inputService);
    }
}