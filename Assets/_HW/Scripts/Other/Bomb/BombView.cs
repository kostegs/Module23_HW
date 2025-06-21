using System.Collections;
using TMPro;
using UnityEngine;

public class BombView : MonoBehaviour
{
    private const float TimeToLiveExplosionEffect = 3f;

    [SerializeField] private ParticleSystem _explosiionEffectPrefab;    
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Transform textPosition;
    [SerializeField] private Canvas _countdownTextCanvas;

    [SerializeField] private ShimmingGlowingCircle _visibleRadius;    

    private Coroutine countdownCoroutine;
    private bool _effectsFinished;
    private float _countdownTimer;

    public void Initialize(Bomb bomb, float countdownTimer)
    {
        _countdownTimer = countdownTimer;
    }
    
    public void MakeExplosionEffects()
    {
        if (countdownCoroutine == null)
            countdownCoroutine = StartCoroutine(StartCountdown());
    }

    public bool EffectsFinished() => _effectsFinished;

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