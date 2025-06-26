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

    public void Initialize(SoundService soundService)
    {
        _raycastResults = new List<RaycastResult>();
        _pointerEventData = new PointerEventData(EventSystem.current);
        _soundService = soundService;
    }

    void Update()
    {
        _pointerEventData.position = Input.mousePosition;

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
                _currentHoveredButton.OnMouseExit();

            if (_newHoveredButton != null)
                _newHoveredButton.OnMouseOver();

            _currentHoveredButton = _newHoveredButton;
        }

        if (Input.GetMouseButtonDown(0))
            if (_currentHoveredButton != null)
            {
                _currentHoveredButton.OnMouseClick();
                bool isTurnOn = _currentHoveredButton.IsTurnOn;

                if (_currentHoveredButton == _musicButton)
                    _soundService.TurnMusicOnOff(isTurnOn);
                else if (_currentHoveredButton == _effectsButton)
                    _soundService.TurnFxOnOff(isTurnOn);
            }
    }
}