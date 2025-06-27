using UnityEngine;
using UnityEngine.AI;

public class AgentCharacter : MonoBehaviour, IMovable, IRotatable, IJumpable, IDamageable
{
    private NavMeshAgent _agent;

    private AgentMover _mover;
    private Rotator _rotator;
    private AgentJumper _jumper;

    private Health _health;
    private int _maxHealth;    

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _movementSpeedInjured;
    [SerializeField] private float _rotationSpeed;
    
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private AnimationCurve _jumpCurve;

    [SerializeField] private int _startHealthValue;    
    [SerializeField] private int _healthToChangeToinjured;

    [SerializeField] private AgentCharacterView _view;
    [SerializeField] private AgentCharacterSound _soundService;

    public Vector3 CurrentVelocity => _mover.CurrentVelocity;

    public Quaternion CurrentRotation => _rotator.CurrentRotation;

    public bool InMovingProcess => _mover.InMovingProcess;

    public bool InJumpingProcess => _jumper.IsJumpProcess;

    public void Initialize()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _mover = new AgentMover(_agent, _movementSpeed);
        _rotator = new Rotator(transform, _rotationSpeed);
        _jumper = new AgentJumper(_jumpSpeed, _agent, this, _jumpCurve);

        _health = new Health(_startHealthValue);
        _maxHealth = _startHealthValue;

        _view.Initialize(this, this, this, _healthToChangeToinjured);
        _soundService.Initialize(this, this);
    }
    
    private void Update()
    {
        _rotator.Update(Time.deltaTime);
    }

    public void SetMoveDirection(Vector3 moveDirection) => _mover.SetDestination(moveDirection);

    public void StopMove() => _mover.Stop();

    public void ResumeMove() => _mover.Resume();

    public void Jump()
    {
    }

    public void Jump(OffMeshLinkData offMeshLinkData) => _jumper.Jump(offMeshLinkData);  
        
    public bool IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData) => NavMeshUtils.IsOnNavMeshLink(_agent, out offMeshLinkData);

    public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);

    public void TakeDamage(int damage)
    {
        _health.Reduce(damage);

        if (_health.Value <= 0)
        {
            StopMove();
            _view.ProcessDeath();
            _soundService.ProcessDeath();
            return;
        }            

        _view.ProcessInjure();
        _soundService.ProcessInjure();

        if(_health.Value <= _healthToChangeToinjured)
        {
            _mover.ChangeSpeed(_movementSpeedInjured);
            _soundService.SetInjuredState();
        }                     
    }

    public int GetCurrentHealth() => _health.Value;

    public int GetMaxHealth() => _maxHealth;
}
