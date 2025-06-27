using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCharacterView : MonoBehaviour
{
    private readonly int ExplosionInjureKey = Animator.StringToHash("Explosion");
    private readonly int DeathKey = Animator.StringToHash("Death");
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsJumpingKey = Animator.StringToHash("IsJumping");
    private const string DissolveEdgeKey = "_Edge";

    private const int InjuredLayerIndex = 1;
    private const int ExplosionInjuredLayerIndex = 2;
    private const int InjuredLayerWeightOn = 2;
    private const int InjuredLayerWeightOff = 0;

    private const int TurnOffShimming = 0;

    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _debuffAnimation;
    [SerializeField] private CinemachineVirtualCamera _camera;
    
    [SerializeField] private ShimmingGlowingCircle _glowingCircle;
    [SerializeField] private bool _enableGlowingCircle;    
    [SerializeField] private Color _glowCircleColorInjured1;
    [SerializeField] private Color _glowCircleColorInjured2;
    [SerializeField] private float _injuredBlinkTime;
    [SerializeField] private float _timeToShowShimCircleGlow;

    [SerializeField] private float _timeToDissolveAfterDeath;

    private IMovable _movable;
    private IDamageable _damageable;
    private IJumpable _jumpable;

    private bool _isInitialized;
    private bool _switchedToInjureAnimations;
    private int _healthToSwitchToInjuredAnimations;
    private Dictionary<SkinnedMeshRenderer, MaterialPropertyBlock> _materials;

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
        
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _materials = new();

        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            MaterialPropertyBlock propertyBlock = new();
            renderer.GetPropertyBlock(propertyBlock);
            _materials.Add(renderer, propertyBlock);
        }

        _isInitialized = true;        
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;

        _animator.SetBool(IsJumpingKey, _jumpable.InJumpingProcess);
        _animator.SetBool(IsRunningKey, _movable.InMovingProcess && !_jumpable.InJumpingProcess);                
        
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

    // Animation event - don't kill it
    public void StopInjuringAnimation() => _animator.SetLayerWeight(ExplosionInjuredLayerIndex, InjuredLayerWeightOff);

    // Animation event - don't kill it
    public void ProcessDissolve() => StartCoroutine(DissolveCoroutine());

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
    
    private IEnumerator DissolveCoroutine()
    {
        // TODO: move it to a right place
        _camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0f, 3f, -2.5f);

        yield return StartCoroutine(GlowingCircleChangeColors(_glowCircleColorInjured1, _glowCircleColorInjured1, _glowCircleColorInjured1, 0, 0));

        float elapsedTime = 0f;

        while (elapsedTime <= _timeToDissolveAfterDeath)
        {
            foreach (KeyValuePair<SkinnedMeshRenderer, MaterialPropertyBlock> KeyValue in _materials)            
                SetFloatToRenderer(KeyValue.Key, KeyValue.Value, DissolveEdgeKey, elapsedTime / _timeToDissolveAfterDeath);            
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _glowingCircle.gameObject.SetActive(false);
    }

    private void SetFloatToRenderer(SkinnedMeshRenderer renderer, MaterialPropertyBlock propertyBlock, string key, float value)
    {
        propertyBlock.SetFloat(key, value);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
