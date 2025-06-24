using TMPro;
using UnityEngine;

public class BombView : MonoBehaviour
{
    private const float TimeToLiveExplosionEffect = 3f;

    [SerializeField] private ParticleSystem _explosiionEffectPrefab;    
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private Transform textPosition;
    [SerializeField] private Canvas _countdownTextCanvas;

    [SerializeField] private ShimmingGlowingCircle _visibleRadius;    
    
    public void PrepareCountdown()
    {
        _countdownTextCanvas.gameObject.SetActive(true);
        _countdownText.transform.position = textPosition.position;
        _countdownText.transform.rotation = Camera.main.transform.rotation;
    }

    public void ChangeCountdownText(string text) => _countdownText.text = text;   

    public void ProcessExplosion()
    {
        ParticleSystem explosionEffect = Instantiate(_explosiionEffectPrefab, transform.position + transform.up, Quaternion.identity);
        explosionEffect.Play();
        Destroy(explosionEffect.gameObject, TimeToLiveExplosionEffect);        
    }
}