using UnityEngine;

public class AgentCharacterView : MonoBehaviour
{
    private readonly int ExplosionInjureKey = Animator.StringToHash("Explosion");
    private readonly int DeathKey = Animator.StringToHash("Death");
    private readonly string IsRunningKey = "IsRunning";
    private const int InjuredLayerIndex = 2;
    private const int InjuredLayerWeightOn = 2;
    private const int InjuredLayerWeightOff = 0;

    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _debuffAnimation;

    private IMovable _movable;
    private IDamageable _damageable;
    private bool _isInitialized;
    private bool _switchedToInjureAnimations;
    private int _healthToSwitchToInjuredAnimations;

    public void Initialize(IMovable movable, IDamageable damageable, int healthToSwitchToInjuredAnimations)
    {
        _movable = movable;
        _damageable = damageable;
        _healthToSwitchToInjuredAnimations = healthToSwitchToInjuredAnimations;

        _isInitialized = true;        
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;

        if (_movable.CurrentVelocity.magnitude > 0.05f)            
            _animator.SetBool(IsRunningKey, true);        
        else        
            _animator.SetBool(IsRunningKey, false);               
        
        if(_damageable.GetCurrentHealth() <= _healthToSwitchToInjuredAnimations
            && _switchedToInjureAnimations == false)
        {
            _animator.SetLayerWeight(1, 2);            
            _switchedToInjureAnimations = true;
        }
    }

    public void ProcessInjure()
    {
        _animator.SetLayerWeight(InjuredLayerIndex, InjuredLayerWeightOn);
        _debuffAnimation.Play();
        _animator.SetTrigger(ExplosionInjureKey);             
    }

    public void ProcessDeath() => _animator.SetTrigger(DeathKey);

    public void StopInjuringAnimation() => _animator.SetLayerWeight(InjuredLayerIndex, InjuredLayerWeightOff);   
}
