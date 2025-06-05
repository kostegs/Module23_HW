using UnityEngine;

public interface IMovable
{
    Vector3 CurrentVelocity { get; }

    void SetMoveDirection(Vector3 inputDirection);
}
