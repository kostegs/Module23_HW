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

    public void TurnFxOnOff(bool turnOn) => TurnSoundOnOff(EffectsKey, turnOn, _cachedEffectsVolume);

    public void TurnMusicOnOff(bool turnOn) => TurnSoundOnOff(MusicKey, turnOn, _cachedMusicVolume);    

    private void TurnSoundOnOff(string key, bool turnOn, float cachedVolume)
    {
        if (turnOn)
        {
            _audioMixer.SetFloat(key, cachedVolume);
        }
        else
        {
            _audioMixer.GetFloat(key, out cachedVolume);
            _audioMixer.SetFloat(key, OffVolumeValue);
        }
    }
}
