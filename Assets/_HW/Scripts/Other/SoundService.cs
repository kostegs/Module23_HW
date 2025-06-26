using UnityEngine;
using UnityEngine.Audio;

public class SoundService : MonoBehaviour
{
    private const float OffVolumeValue = -80f;    

    private const string MusicKey = "MusicVolume";
    private const string EffectsKey = "EffectsVolume";

    [SerializeField] private AudioMixer _audioMixer;

    private float _cachedMusicVolume;
    private float _cachedEffectsVolume;

    public void TurnMusicOnOff(bool turnOn)
    {
        if (turnOn)
        {
            _audioMixer.SetFloat(MusicKey, _cachedMusicVolume);
        }
        else
        {
            _audioMixer.GetFloat(MusicKey, out _cachedMusicVolume);
            _audioMixer.SetFloat(MusicKey, OffVolumeValue);
        }
    }
}
