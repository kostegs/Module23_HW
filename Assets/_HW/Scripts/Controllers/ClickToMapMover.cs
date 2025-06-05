using UnityEngine;
using UnityEngine.AI;

public class ClickToMapMover : Controller
{
    private IMovable _movableObject;    
    private InputService _inputService;

    private NavMeshPath _pathToTarget = new NavMeshPath();

    public ClickToMapMover(IMovable movableObject, InputService inputService)
    {
        _movableObject = movableObject;        
        _inputService = inputService;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_inputService.GetClickedPointOnMap(out Vector3 target))
        {        
            float distanceToTarget = NavMeshUtils.GetPathLength(_pathToTarget);

            _movableObject.ResumeMove();
            _movableObject.SetMoveDirection(target);
        }
    }
}