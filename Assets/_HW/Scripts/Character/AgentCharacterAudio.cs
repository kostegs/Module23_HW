using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AgentCharacterAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;    
    [SerializeField] private AudioClip _footSound;

    private bool _audioOn;
    private bool _isInitialized;

    private IMovable _movable;

    public void Initialize(IMovable movable)
    {
        _movable = movable;

        _isInitialized = true;
    }
        

    void Update()
    {
        if (_isInitialized == false)
            return;        

        if (_movable.InMovingProcess && _audioOn == false)
        {            
            _audioSource.Play();
            _audioOn = true;
        }            
        else if (_movable.InMovingProcess == false && _audioOn == true)
        {
            StartCoroutine(StopSound());
            _audioOn = false;            
        }            
    }

    private IEnumerator StopSound()
    {
        Debug.Log("Корутина запущена");
        yield return new WaitForSeconds(_audioSource.clip.length);
        _audioSource.Stop();
    }
}
