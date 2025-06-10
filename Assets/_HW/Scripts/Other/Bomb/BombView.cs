using System.Collections;
using TMPro;
using UnityEngine;

public class BombView : MonoBehaviour
{
    private const float TimeToLiveExplosionEffect = 3f;

    [SerializeField] private ParticleSystem _explosiionEffectPrefab;
    [SerializeField] private BombVisibleRadius _visibleRadius;
    [SerializeField] private bool _drawVisibleRadius;
    [SerializeField] private Color[] _shimmerColors;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Transform textPosition;
    [SerializeField] private Canvas _countdownTextCanvas;
    
    private Bomb _parentBomb;   
    private Coroutine countdownCoroutine;
    private bool _effectsFinished;
    private float _countdownTimer;

    public void Initialize(Bomb bomb, float countdownTimer)
    {
        _parentBomb = bomb;

        _visibleRadius.gameObject.SetActive(_drawVisibleRadius);

        if (_drawVisibleRadius)
            _visibleRadius.Initialize(_shimmerColors);

        _countdownTimer = countdownTimer;
    }

    private void Update() => DrawVisibleRadius();
    public void MakeExplosionEffects()
    {
        if (countdownCoroutine == null)
            countdownCoroutine = StartCoroutine(StartCountdown());
    }

    public bool EffectsFinished() => _effectsFinished;

    private void DrawVisibleRadius()
    {
        if (_drawVisibleRadius)
            _visibleRadius.CheckVisibleRadius(_parentBomb.ExplosiveRadius);
    }

    IEnumerator StartCountdown()
    {
        _countdownTextCanvas.gameObject.SetActive(true);
        float timer = _countdownTimer;
        countdownText.transform.position = textPosition.position;
        countdownText.transform.rotation = Camera.main.transform.rotation;

        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();            
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        Explode();
    }

    private void Explode()
    {
        ParticleSystem explosionEffect = Instantiate(_explosiionEffectPrefab, transform.position + transform.up, Quaternion.identity);
        explosionEffect.Play();
        Destroy(explosionEffect.gameObject, TimeToLiveExplosionEffect);
        _effectsFinished = true;
    }
}