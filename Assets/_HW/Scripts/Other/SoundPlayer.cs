using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    public bool AudioOn => _audioSource.isPlaying;
    public float AudioLength => _audioClip.length;

    private bool _enabled;

    public void PlayOneShot()
    {
        if (_enabled == false)
            return;

        if (AudioOn == false)
            _audioSource.PlayOneShot(_audioClip);
    }

    public void SetEnableOn() => _enabled = true;

    public void SetEnableOff() => _enabled = false;

    public void SetPitch(float pitch) => _audioSource.pitch = pitch;
}