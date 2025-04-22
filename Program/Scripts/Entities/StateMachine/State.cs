public abstract class State<T> where T : StateMachine<T>
{
    public bool canEnterSelf = false;
    public abstract void EnterState(T ctx);
    public abstract void UpdateState(T ctx);
    public abstract void ExitState(T ctx);
}