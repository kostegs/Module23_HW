using UnityEngine;

public class BombView : MonoBehaviour
{
    private const float TimeToLiveExplosionEffect = 3f;

    [SerializeField] private ParticleSystem _explosiionEffectPrefab;
    [SerializeField] private BombVisibleRadius _visibleRadius;
    [SerializeField] private bool _drawVisibleRadius;
    [SerializeField] private Color[] _shimmerColors;

    private Bomb _parentBomb;   
    private SpriteRenderer _spriteRenderer;

    public void Initialize(Bomb bomb)
    {
        _parentBomb = bomb;

        _visibleRadius.gameObject.SetActive(_drawVisibleRadius);

        if (_drawVisibleRadius)
            _visibleRadius.Initialize(_shimmerColors);
    }

    private void Update()
    {
        DrawVisibleRadius();
    }

    private void DrawVisibleRadius()
    {
        if (_drawVisibleRadius)
            _visibleRadius.CheckVisibleRadius(_parentBomb.ExplosiveRadius);
    }

    public void MakeExplosion()
    {
        ParticleSystem explosionEffect = Instantiate(_explosiionEffectPrefab, transform.position + transform.up, Quaternion.identity);
        explosionEffect.Play();
        Destroy(explosionEffect.gameObject, TimeToLiveExplosionEffect);
    }   
}