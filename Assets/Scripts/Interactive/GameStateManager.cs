using UnityEngine;

/// <summary>
/// GameStateManager 存储当前状态，在游戏中根据需要调用不同状态的 EnterState() 和 UpdateState() 方法。
/// </summary>
public class GameStateManager : MonoBehaviour
{
    // 单例实例
    private InteractionState currentState;  // 当前游戏状态

    // 切换状态
    public void SwitchState(InteractionState newState)
    {
        if (currentState != null)
            currentState.ExitState();  // 退出当前状态

        currentState = newState;  // 设置新的状态
        currentState.EnterState();  // 进入新的状态

        //UpdateCurrentState();
    }

    // 每帧更新当前状态
    void Update()
    {
        if (currentState != null)
            currentState.UpdateState();  // 更新当前状态
    }
}
