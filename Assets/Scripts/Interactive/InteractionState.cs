/// <summary>
/// 每种交互类型实现具体的状态类（如可见状态、互动状态和解锁状态）
/// </summary>
public abstract class InteractionState
{
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
}
