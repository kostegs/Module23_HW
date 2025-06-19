using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentJumper
{
    private float _speed;
    private NavMeshAgent _agent;
    private MonoBehaviour _coroutineStarter;
    private AnimationCurve _yOffsetCurve;

    private Coroutine _jumpProcess;

    public bool IsJumpProcess => _jumpProcess != null;

    public AgentJumper(
        float speed, 
        NavMeshAgent agent,
        MonoBehaviour coroutineStarter,
        AnimationCurve yOffsetCurve)
    {
        _speed = speed;
        _agent = agent;
        _coroutineStarter = coroutineStarter;
        _yOffsetCurve = yOffsetCurve;
    }

    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        if (IsJumpProcess)
            return;

        _jumpProcess = _coroutineStarter.StartCoroutine(JumpProcess(offMeshLinkData));
    }

    private IEnumerator JumpProcess(OffMeshLinkData offMeshLinkData)
    {
        Vector3 startPos = offMeshLinkData.startPos;
        Vector3 endPos = offMeshLinkData.endPos;

        float duration = Vector3.Distance(startPos, endPos) / _speed;

        float progress = 0;

        while (progress < duration)
        {
            float yOffset = _yOffsetCurve.Evaluate(progress / duration);

            _agent.transform.position = Vector3.Lerp(startPos, endPos, progress / duration) + Vector3.up * yOffset;
            progress += Time.deltaTime;

            yield return null;
        }

        _agent.CompleteOffMeshLink();
        _jumpProcess = null;
    }
}
