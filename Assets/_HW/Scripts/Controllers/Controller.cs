public abstract class Controller
{
    private bool _IsEnabled;

    public virtual void Enable() => _IsEnabled = true;

    public virtual void Disable() => _IsEnabled = false;

    public void Update(float deltaTime)
    {
        if (_IsEnabled == false)
            return;

        UpdateLogic(deltaTime);
    }

    protected abstract void UpdateLogic(float deltaTime);
}
