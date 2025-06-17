using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BombVisibleRadiusShader : MonoBehaviour
{
    private const string ChangeColorCoefficiencyKey = "_ChangeColorCoefficiency";

    [SerializeField] private float _timeToChangeColor;    

    private bool _isInitialized;

    private MaterialPropertyBlock _propertyBlock;
    private MeshRenderer _renderer;

    public void Initialize()
    {
        _renderer = GetComponent<MeshRenderer>();
        
        _propertyBlock = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_propertyBlock);        

        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;

        float colorValue = Mathf.PingPong(Time.time, _timeToChangeColor);
        _propertyBlock.SetFloat(ChangeColorCoefficiencyKey, colorValue / _timeToChangeColor);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
