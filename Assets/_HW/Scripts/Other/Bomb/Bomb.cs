using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosiveRadius;
    [SerializeField] private int _explosiveDamage;
    [SerializeField] private LayerMask _explosiveObjectsMask;
    [SerializeField] private BombView _view;
    [SerializeField] private float _timeToExplode;

    public float ExplosiveRadius => _explosiveRadius;
    private bool IsDestroying;

    private void Awake()
    {
        _view.Initialize(this, _timeToExplode);
    }

    private void Update()
    {
        if (IsDestroying)
        {
            if (_view.EffectsFinished())
            {
                IDamageable damageable = GetDamageableInRadius();
                
                if(damageable != null)
                    damageable.TakeDamage(_explosiveDamage);                                   

                Destroy(this.gameObject);
            }

            return;
        }

        if (GetDamageableInRadius() != null)
        {
            IsDestroying = true;
            _view.MakeExplosionEffects();
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
}
