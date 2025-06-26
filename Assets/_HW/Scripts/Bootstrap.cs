using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private const int MouseButton = 0;

    [SerializeField] private MapMoveToMarker _mapMoveToMarker;        
    [SerializeField] private Example _Example;
    [SerializeField] private AgentCharacter _character;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private SoundSystemUI _soundSystemUI;
    [SerializeField] private SoundService _soundService;

    private void Awake()
    {
        IInputProcessor inputProcessor = new MouseInput(MouseButton);

        InputService inputService = new InputService(inputProcessor);
        
        _mapMoveToMarker.Initialize(inputService);
        _Example.Initialize(inputService);
        _character.Initialize();
        _healthBar.Initialize(_character);
        _soundSystemUI.Initialize(_soundService);
    }
}