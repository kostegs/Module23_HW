using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _filledImage;
    [SerializeField] private TextMeshProUGUI _text;

    private IDamageable _damageable;
    
    private float _cachedHealth;

    public void Initialize(IDamageable damageable)
    {
        _damageable = damageable;
    }
    
    void Update()
    {
        float currentHealth = _damageable.GetCurrentHealth();

        if(_cachedHealth != currentHealth)
        {
            _cachedHealth = currentHealth;
            currentHealth = (float)_damageable.GetCurrentHealth() / _damageable.GetMaxHealth();
            _filledImage.fillAmount = currentHealth;
            _text.text = _cachedHealth.ToString("0.####") + "%";
        }
    }
}
