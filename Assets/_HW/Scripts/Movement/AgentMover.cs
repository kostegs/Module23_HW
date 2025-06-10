using UnityEngine;
using UnityEngine.AI;

public class AgentMover
{
    private const float MoveAcceleration = 999f;
    private NavMeshAgent _agent;
    private NavMeshPath _navMeshPath;

    public Vector3 CurrentVelocity => _agent.desiredVelocity;

    public AgentMover(NavMeshAgent agent, float movementSpeed)
    {
        _agent = agent;
        _agent.speed = movementSpeed;
        _agent.acceleration = MoveAcceleration;

        _navMeshPath = new NavMeshPath();
    }

    public void SetDestination(Vector3 position)
    {
        if (TryGetPath(position, _navMeshPath))        
            _agent.SetDestination(position);            
    }

    public void Stop() => _agent.isStopped = true;

    public void Resume() => _agent.isStopped = false;

    private bool TryGetPath(Vector3 targetPosition, NavMeshPath pathToTarget)
       => NavMeshUtils.TryGetPath(_agent, targetPosition, pathToTarget);
}
