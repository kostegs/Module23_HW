using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private readonly int ExplosionInjureKey = Animator.StringToHash("Explosion");
    private readonly int DeathKey = Animator.StringToHash("Death");
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsJumpingKey = Animator.StringToHash("IsJumping");

    private const int InjuredLayerIndex = 1;
    private const int ExplosionInjuredLayerIndex = 2;
    private const int InjuredLayerWeightOn = 2;
    private const int InjuredLayerWeightOff = 0;

    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _debuffAnimation;

    private IMovable _movable;
    private IDamageable _damageable;
    private IJumpable _jumpable;

    private bool _isInitialized;
    private bool _switchedToInjureAnimations;
    private int _healthToSwitchToInjuredAnimations;    

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
    }

    public void ProcessDeath() => _animator.SetTrigger(DeathKey);

    public void StopInjuringAnimation() => _animator.SetLayerWeight(ExplosionInjuredLayerIndex, InjuredLayerWeightOff);   
}
