using System.Collections;
using UnityEngine;

public class AgentCharacterSoundService : MonoBehaviour
{
    [SerializeField] private SoundPlayer _movableSoundPlayer;
    [SerializeField] private SoundPlayer _jumpableSoundPlayer;
    [SerializeField] private SoundPlayer _deathSoundPlayer;
    [SerializeField] private SoundPlayer _injuredSoundPlayer;

    [SerializeField] private float _injuredPitch;

    private bool _isInitialized;

    private IMovable _movableService;
    private IJumpable _jumpableService;

    private bool _jumpSoundPlayed;

    public void Initialize(IMovable movableService, IJumpable jumpableService)
    {
        _movableService = movableService;
        _jumpableService = jumpableService;

        _movableSoundPlayer.SetEnableOn();
        _jumpableSoundPlayer.SetEnableOn();
        _injuredSoundPlayer.SetEnableOn();
        _deathSoundPlayer.SetEnableOn();

        _isInitialized = true;
    }        

    private void Update()
    {
        if (_isInitialized == false)
            return;

        CheckPlayJumpSound();
        CheckPlayMovingSound();        
    }

    public void ProcessDeath()
    {
        _movableSoundPlayer.SetEnableOff();
        _jumpableSoundPlayer.SetEnableOff();
        _deathSoundPlayer.PlayOneShot();
    }

    public void ProcessInjure() => _injuredSoundPlayer.PlayOneShot();

    public void SetInjuredState()
    {
        _movableSoundPlayer.SetPitch(_injuredPitch);
    }

    private void CheckPlayJumpSound()
    {
        if (_jumpableService.InJumpingProcess && _jumpSoundPlayed == false)
        {
            _movableSoundPlayer.SetEnableOff();
            _jumpableSoundPlayer.PlayOneShot();
            _jumpSoundPlayed = true;
        }
        else if (_jumpableService.InJumpingProcess == false && _jumpSoundPlayed)
        {
            _movableSoundPlayer.SetEnableOn();
            _jumpSoundPlayed = false;
        }
    }

    private void CheckPlayMovingSound()
    {
        if (_movableService.InMovingProcess)
            _movableSoundPlayer.PlayOneShot();
    }
}
