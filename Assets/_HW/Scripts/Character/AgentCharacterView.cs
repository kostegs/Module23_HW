using System.Collections;
using UnityEngine;

public class AgentCharacterView : MonoBehaviour
{
    private readonly int ExplosionInjureKey = Animator.StringToHash("Explosion");
    private readonly int DeathKey = Animator.StringToHash("Death");
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsJumpingKey = Animator.StringToHash("IsJumping");

    private const int InjuredLayerIndex = 1;
    private const int ExplosionInjuredLayerIndex = 2;
    private const int InjuredLayerWeightOn = 2;
    private const int InjuredLayerWeightOff = 0;

    private const int TurnOffShimming = 0;

    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _debuffAnimation;
    
    [SerializeField] private ShimmingGlowingCircle _glowingCircle;
    [SerializeField] private bool _enableGlowingCircle;    
    [SerializeField] private Color _glowCircleColorInjured1;
    [SerializeField] private Color _glowCircleColorInjured2;
    [SerializeField] private float _injuredBlinkTime;
    [SerializeField] private float _timeToShowShimCircleGlow;

    private IMovable _movable;
    private IDamageable _damageable;
    private IJumpable _jumpable;

    private bool _isInitialized;
    private bool _switchedToInjureAnimations;
    private int _healthToSwitchToInjuredAnimations;

    private Coroutine _injuredShimmerCoroutine;

    public void Initialize(
        IMovable movable,
        IDamageable damageable,
        IJumpable jumpable,
        int healthToSwitchToInjuredAnimations)
    {
        _movable = movable;
        _damageable = damageable;
        _jumpable = jumpable;
        _healthToSwitchToInjuredAnimations = healthToSwitchToInjuredAnimations;    

        _isInitialized = true;        
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;

        if (_jumpable.InJumpingProcess)
        {
            _animator.SetBool(IsJumpingKey, true);
            return;
        }
        else
        {
            _animator.SetBool(IsJumpingKey, false);
        }            

        if (_movable.InMovingProcess)            
            _animator.SetBool(IsRunningKey, true);        
        else        
            _animator.SetBool(IsRunningKey, false);               
        
        if(_damageable.GetCurrentHealth() <= _healthToSwitchToInjuredAnimations
            && _switchedToInjureAnimations == false)
        {
            _animator.SetLayerWeight(InjuredLayerIndex, InjuredLayerWeightOn);            
            _switchedToInjureAnimations = true;
        }
    }

    public void ProcessInjure()
    {
        _animator.SetLayerWeight(ExplosionInjuredLayerIndex, InjuredLayerWeightOn);
        _debuffAnimation.Play();
        _animator.SetTrigger(ExplosionInjureKey);

        if (_enableGlowingCircle)
            GlowingCircleProcessInjure();        
    }

    private void GlowingCircleProcessInjure()
    {
        if (_injuredShimmerCoroutine != null)
            StopCoroutine(_injuredShimmerCoroutine);

        (Color color1, Color color2) = _glowingCircle.GetColors();

        _injuredShimmerCoroutine = StartCoroutine(GlowingCircleChangeColors(_glowCircleColorInjured1, _glowCircleColorInjured2, color1, _injuredBlinkTime, _timeToShowShimCircleGlow));
    }

    IEnumerator GlowingCircleChangeColors(Color color1, Color color2, Color colorToRestore, float blinkTime, float timeToShim)
    {
        _glowingCircle.SetColors(color1, color2);
        _glowingCircle.SetBlinkTime(blinkTime);

        yield return new WaitForSeconds(timeToShim);

        _glowingCircle.SetColors(colorToRestore, colorToRestore);
        _glowingCircle.SetBlinkTime(TurnOffShimming);

        _injuredShimmerCoroutine = null;

        yield return null;
    }

    public void ProcessDeath() => _animator.SetTrigger(DeathKey);

    public void StopInjuringAnimation() => _animator.SetLayerWeight(ExplosionInjuredLayerIndex, InjuredLayerWeightOff);   
}
