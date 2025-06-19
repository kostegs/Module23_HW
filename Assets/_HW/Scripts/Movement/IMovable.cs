using UnityEngine;

public interface IMovable
{
    Vector3 CurrentVelocity { get; }

    bool InMovingProcess { get; }

    void SetMoveDirection(Vector3 moveDirection);

    void StopMove();

    void ResumeMove();
}
