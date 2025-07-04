using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UITurnOnOffButton : MonoBehaviour, ITurnOnTurnOffButton
{
    private const float LocalScaleCoefficiency = 1.2f;

    [SerializeField] private Sprite _imageTurnOn;
    [SerializeField] private Sprite _imageTurnOff;

    private bool _isTurnOn;
    private bool _isButtonOver;
    private Image _image;
    private Vector3 _cachedScale;

    public bool IsTurnOn => _isTurnOn;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = _imageTurnOn;
        _isTurnOn = true;
    }    

    public void OnButtonClick()
    {
        if (_isTurnOn)
            _image.sprite = _imageTurnOff;
        else
            _image.sprite = _imageTurnOn;

        _isTurnOn = !_isTurnOn;
    }    

    public void OnButtonOver()
    {
        if (_isButtonOver == false)
        {
            _isButtonOver = true;
            _cachedScale = transform.localScale;
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(LocalScaleCoefficiency, LocalScaleCoefficiency, LocalScaleCoefficiency));
        }       
    }

    public void OnButtonExit()
    {
        _isButtonOver = false;
        transform.localScale = _cachedScale;
    }
}
