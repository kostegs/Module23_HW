using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ShimmingGlowingCircle : MonoBehaviour
{
    private const string ChangeColorCoefficiencyKey = "_ChangeColorCoefficiency";
    private const string Color1Key = "_GlowColor1";
    private const string Color2Key = "_GlowColor2";
    private const string RadiusKey = "_Radius";
    private const string OutBorderWidthKey = "_OutBorderWidth";
    private const string InBorderWidthKey = "_InBorderWidth";

    [SerializeField] private float _blinkTime;
    [SerializeField] private Color _glowColor1;
    [SerializeField] private Color _glowColor2;
    [SerializeField] private float _radius;
    [SerializeField] private float _inBorderWidth;
    [SerializeField] private float _outBorderWidth;

    private bool _isInitialized;

    private MaterialPropertyBlock _propertyBlock;
    private MeshRenderer _renderer;        

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();

        _propertyBlock = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_propertyBlock);

        ConfigureMaterial();

        _isInitialized = true;
    }    

    private void Update()
    {
        if (_isInitialized == false || _blinkTime == 0)
            return;

        float colorValue = Mathf.PingPong(Time.time, _blinkTime);
        _propertyBlock.SetFloat(ChangeColorCoefficiencyKey, colorValue / _blinkTime);
        _renderer.SetPropertyBlock(_propertyBlock);
    }

    public (Color, Color) GetColors() => (_glowColor1,  _glowColor2);

    public void SetColors(Color color1, Color color2)
    {
        _propertyBlock.SetColor(Color1Key, color1);
        _propertyBlock.SetColor(Color2Key, color2);
        _renderer.SetPropertyBlock(_propertyBlock);
    } 

    public void SetBlinkTime(float blinkTime) => _blinkTime = blinkTime;
        
    private void ConfigureMaterial()
    {
        SetColors(_glowColor1, _glowColor2);
        SetRadius(_radius);
        SetBordersWidth(_inBorderWidth, _outBorderWidth);        
    }   

    private void SetRadius(float radius)
    {
        _propertyBlock.SetFloat(RadiusKey, radius);
        _renderer.SetPropertyBlock(_propertyBlock);
    }

    private void SetBordersWidth(float inBorderWidth, float outBorderWidth)
    {
        _propertyBlock.SetFloat(InBorderWidthKey, inBorderWidth);
        _propertyBlock.SetFloat(OutBorderWidthKey, outBorderWidth);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}