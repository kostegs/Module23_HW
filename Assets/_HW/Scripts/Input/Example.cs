using UnityEngine;
using UnityEngine.AI;

public class Example : MonoBehaviour
{
    public const float MinDistanceToTarget = 0.05f;

    [SerializeField] private AgentCharacter _agentCharacter;

    private Controller _agentController;

    public void Initialize(InputService inputService)
    {
        NavMeshQueryFilter queryFilter = new NavMeshQueryFilter();
        queryFilter.agentTypeID = 0;
        queryFilter.areaMask = NavMesh.AllAreas;

        _agentController = new CompositeController(
            new ClickToMapMover(_agentCharacter, inputService),
            new RotatableController(_agentCharacter, _agentCharacter));

        _agentController.Enable();
    }

    private void Update()
    {
        if (_agentCharacter.GetCurrentHealth() == 0)
            return;

        _agentController.Update(Time.deltaTime);        
    }
}