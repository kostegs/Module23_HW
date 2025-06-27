using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosiveRadius;
    [SerializeField] private int _explosiveDamage;
    [SerializeField] private LayerMask _explosiveObjectsMask;
    [SerializeField] private BombView _view;
    [SerializeField] private BombSound _soundService;
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

        if (GetDamageableInRadius(out List<IDamageable> damageableObjects))        
            _destroyingCoroutine = StartCoroutine(ProcessDestroying());                            
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _explosiveRadius);
    }

    private bool GetDamageableInRadius(out List<IDamageable> damageableObjects)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosiveRadius, _explosiveObjectsMask);
        damageableObjects = new List<IDamageable>();

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                damageableObjects.Add(damageable);                            
        }

        return damageableObjects.Count > 0;
    }

    private IEnumerator ProcessDestroying()
    {
        yield return StartCoroutine(CountdownBeforeExplode());
        
        _view.ProcessExplosion();        
        _soundService.ProcessExplosion();

        GetDamageableInRadius(out List<IDamageable> damageableObjects);

        foreach (IDamageable damageable in damageableObjects)
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
