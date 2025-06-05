using UnityEngine;

public interface IRotatable
{
    Quaternion CurrentRotation { get; }

    void SetRotationDirection(Vector3 inputDirection);
}
