using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosiveRadius;
    [SerializeField] private int _explosiveDamage;
    [SerializeField] private LayerMask _explosiveObjectsMask;
    [SerializeField] private BombView _view;
    [SerializeField] private BombSoundService _soundService;
    [SerializeField] private float _timeToExplode;

    public float ExplosiveRadius => _explosiveRadius;    

    private Coroutine _destroyingCoroutine;

    private void Awake()
    {        
        _soundService.Initialize();
    }

    private void Update()
    {
        if (_destroyingCoroutine != null)
            return;

        IDamageable damageable = GetDamageableInRadius();

        if (damageable != null)        
            _destroyingCoroutine = StartCoroutine(ProcessDestroying(damageable));                            
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _explosiveRadius);
    }

    private IDamageable GetDamageableInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosiveRadius, _explosiveObjectsMask);

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                return damageable;
            }                
        }

        return null;
    }

    private IEnumerator ProcessDestroying(IDamageable damageable)
    {
        yield return StartCoroutine(CountdownBeforeExplode());
        
        _view.ProcessExplosion();        
        _soundService.ProcessExplosion();

        damageable.TakeDamage(_explosiveDamage);
        _view.gameObject.SetActive(false);

        yield return new WaitForSeconds(_soundService.ExplosionAudioLength);

        Destroy(this.gameObject);
    }

    private IEnumerator CountdownBeforeExplode()
    {
        float timer = _timeToExplode;
        _view.PrepareCountdown();
        
        while (timer > 0)
        {
            _view.ChangeCountdownText(Mathf.Ceil(timer).ToString());
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
    }
}
