using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosiveRadius;
    [SerializeField] private int _explosiveDamage;
    [SerializeField] private LayerMask _explosiveObjectsMask;
    [SerializeField] private BombView _view;

    public float ExplosiveRadius => _explosiveRadius;

    private void Awake()
    {
        _view.Initialize(this);
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosiveRadius, _explosiveObjectsMask);

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(_explosiveDamage);
                _view.MakeExplosion();
                Destroy(this.gameObject);
            }                
        }            
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _explosiveRadius);
    }
}
