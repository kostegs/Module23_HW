using UnityEngine;
using UnityEngine.AI;

public class AgentCharacterClickToMapMover : Controller
{
    private AgentCharacter _character;
    private InputService _inputService;
    private Vector3 _targetToMove;    

    public AgentCharacterClickToMapMover(AgentCharacter character, InputService inputService)
    {
        _character = character;
        _inputService = inputService;
    }

    protected override void UpdateLogic()
    {
        if (_inputService.GetClickedPointOnMap(out Vector3 target))
        {
            _targetToMove = target;

            if (_character.InJumpingProcess == false)
                _character.SetMoveDirection(_targetToMove);
        }               
        
        if (_character.IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData))
        {
            if (_character.InJumpingProcess == false)                
                _character.Jump(offMeshLinkData);            

            return;
        }

        _character.ResumeMove();        
    }
}
