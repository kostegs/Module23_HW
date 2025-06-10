using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AgentCharacterView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private IMovable _movable;
    private bool _isInitialized;

    public void Initialize(IMovable movable)
    {
        _movable = movable;
        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;

        if (_movable.CurrentVelocity.magnitude > 0.05f)
        {
            Debug.Log("set run true");
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            Debug.Log("set run false");
            _animator.SetBool("IsRunning", false);
        }
            
    }
}
