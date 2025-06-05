using UnityEngine;
using UnityEngine.AI;

public class AgentClickToMapMover : Controller
{
    private AgentCharacter _character;    
    private InputService _inputService;

    private NavMeshPath _pathToTarget = new NavMeshPath();

    public AgentClickToMapMover(AgentCharacter character, InputService inputService)
    {
        _character = character;        
        _inputService = inputService;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        Vector3 _target = _inputService.GetClickedPointOnMap();        

        if (_target != Vector3.zero && _character.TryGetPath(_target, _pathToTarget))
        {
            float distanceToTarget = NavMeshUtils.GetPathLength(_pathToTarget);

            _character.ResumeMove();
            _character.SetMoveDirection(_target);
        }
    }
}