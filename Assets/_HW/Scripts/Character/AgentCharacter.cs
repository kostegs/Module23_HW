using UnityEngine;
using UnityEngine.AI;

public class AgentCharacter : MonoBehaviour, IMovable, IRotatable
{
    private NavMeshAgent _agent;

    private AgentMover _mover;
    private Rotator _rotator;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;

    public Vector3 CurrentVelocity => _mover.CurrentVelocity;
    public Quaternion CurrentRotation => _rotator.CurrentRotation;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _mover = new AgentMover(_agent, _movementSpeed);
        _rotator = new Rotator(transform, _rotationSpeed);
    }

    private void Update()
    {
        _rotator.Update(Time.deltaTime);
    }

    public void SetMoveDirection(Vector3 moveDirection) => _mover.SetDestination(moveDirection);
    public void StopMove() => _mover.Stop();

    public void ResumeMove() => _mover.Resume();

    public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);
       
}
