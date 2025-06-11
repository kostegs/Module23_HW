public abstract class Controller
{
    private bool _IsEnabled;

    public virtual void Enable() => _IsEnabled = true;

    public virtual void Disable() => _IsEnabled = false;

    public void Update()
    {
        if (_IsEnabled == false)
            return;

        UpdateLogic();
    }

    protected abstract void UpdateLogic();
}
