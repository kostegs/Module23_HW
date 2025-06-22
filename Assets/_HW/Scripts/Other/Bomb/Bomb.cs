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
        _view.Initialize(this, _timeToExplode);
        _soundService.Initialize();
    }

    private void Update()
    {
        if (_destroyingCoroutine != null)
            return;

        if (GetDamageableInRadius() != null)
        {
            _view.MakeExplosionEffects();
            _destroyingCoroutine = StartCoroutine(ProcessDestroying());
        }                    
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

    private IEnumerator ProcessDestroying()
    {
        while (_view.EffectsFinished() == false)
            yield return null;

        IDamageable damageable = GetDamageableInRadius();

        if (damageable != null)
            damageable.TakeDamage(_explosiveDamage);

        _soundService.ProcessExplosion();
        _view.gameObject.SetActive(false);

        yield return new WaitForSeconds(_soundService.ExplosionAudioLength);

        Destroy(this.gameObject);
    }
}
