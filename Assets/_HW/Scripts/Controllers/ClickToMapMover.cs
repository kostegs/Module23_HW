using UnityEngine;

public class ClickToMapMover : Controller
{
    private IMovable _movableObject;    
    private InputService _inputService;

    public ClickToMapMover(IMovable movableObject, InputService inputService)
    {
        _movableObject = movableObject;        
        _inputService = inputService;
    }

    protected override void UpdateLogic()
    {
        if (_inputService.GetClickedPointOnMap(out Vector3 target))
        {        
            _movableObject.ResumeMove();
            _movableObject.SetMoveDirection(target);
        }
    }
}