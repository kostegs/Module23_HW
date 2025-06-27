using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundSystemUI : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private UITurnOnOffButton _musicButton;
    [SerializeField] private UITurnOnOffButton _effectsButton;

    private UITurnOnOffButton _currentHoveredButton;
    private UITurnOnOffButton _newHoveredButton;

    private List<RaycastResult> _raycastResults;
    private PointerEventData _pointerEventData;
    private SoundService _soundService;
    private InputService _inputService;

    public void Initialize(SoundService soundService, InputService inputService)
    {
        _raycastResults = new List<RaycastResult>();
        _pointerEventData = new PointerEventData(EventSystem.current);
        _soundService = soundService;
        _inputService = inputService;
    }

    void Update()
    {
        _pointerEventData.position = _inputService.GetCurrentCursorPosition();

        _raycastResults.Clear();
        _raycaster.Raycast(_pointerEventData, _raycastResults);

        _newHoveredButton = null;

        foreach (RaycastResult r in _raycastResults)            
            if (r.gameObject.TryGetComponent<UITurnOnOffButton>(out UITurnOnOffButton clickableButton))
            {
                _newHoveredButton = clickableButton;
                break;
            }

        if (_currentHoveredButton != _newHoveredButton)
        {
            if (_currentHoveredButton != null)
                _currentHoveredButton.OnButtonExit();

            if (_newHoveredButton != null)
                _newHoveredButton.OnButtonOver();

            _currentHoveredButton = _newHoveredButton;
        }

        if (_inputService.IsButtonDown())
            if (_currentHoveredButton != null)
            {
                _currentHoveredButton.OnButtonClick();
                
                bool isTurnOn = _currentHoveredButton.IsTurnOn;

                if (_currentHoveredButton == _musicButton)
                    _soundService.TurnMusicOnOff(isTurnOn);
                else if (_currentHoveredButton == _effectsButton)
                    _soundService.TurnFxOnOff(isTurnOn);
            }
    }
}