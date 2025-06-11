public class RotatableController : Controller
{
    private IMovable _movable;
    private IRotatable _rotatable;

    public RotatableController(IMovable movable, IRotatable rotatable)
    {
        _movable = movable;
        _rotatable = rotatable;
    }

    protected override void UpdateLogic()
    {
        _rotatable.SetRotationDirection(_movable.CurrentVelocity);
    }
}
