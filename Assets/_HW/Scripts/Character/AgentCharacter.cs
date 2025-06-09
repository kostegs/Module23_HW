using UnityEngine;
using UnityEngine.AI;

public class AgentCharacter : MonoBehaviour, IMovable, IRotatable, IDamageable
{
    private NavMeshAgent _agent;

    private AgentMover _mover;
    private Rotator _rotator;

    private Health _health;
    private int _maxHealth;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _startHealthValue;

    public Vector3 CurrentVelocity => _mover.CurrentVelocity;
    public Quaternion CurrentRotation => _rotator.CurrentRotation;

    public void Initialize()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _mover = new AgentMover(_agent, _movementSpeed);
        _rotator = new Rotator(transform, _rotationSpeed);

        _health = new Health(_startHealthValue);
        _maxHealth = _startHealthValue;
    }
    
    private void Update()
    {
        _rotator.Update(Time.deltaTime);
    }

    public void SetMoveDirection(Vector3 moveDirection) => _mover.SetDestination(moveDirection);

    public void StopMove() => _mover.Stop();

    public void ResumeMove() => _mover.Resume();

    public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);

    public void TakeDamage(int damage) => _health.Reduce(damage);

    public int GetCurrentHealth() => _health.Value;

    public int GetMaxHealth() => _maxHealth;
}
