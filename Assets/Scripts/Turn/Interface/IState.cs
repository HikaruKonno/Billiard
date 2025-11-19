// ターンの状態のインターフェイス
using System;

public interface IState 
{
    /// <summary>
    /// ステートの最初の処理
    /// </summary>
    public void OnBegin();

    /// <summary>
    /// そのステート間でやり続ける処理
    /// </summary>
    public void OnUpdate();

    /// <summary>
    /// ステートの最後の処理
    /// </summary>
    public void OnEnd();

    /// <summary>
    /// そのステートの入力処理
    /// </summary>
    /// <param name="input">入力</param>
    public void HandleInput(InputData input);
}
