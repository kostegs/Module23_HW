using UnityEngine;

public class BombSound : MonoBehaviour
{
    [SerializeField] private SoundPlayer _explosionSoundPlayer;

    public float ExplosionAudioLength => _explosionSoundPlayer.AudioLength;

    public void Initialize() => _explosionSoundPlayer.SetEnableOn();

    public void ProcessExplosion() => _explosionSoundPlayer.PlayOneShot();
}
