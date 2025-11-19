using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public sealed class TurnStateMachine
{
    //------------------------------------------------------------------------------------  
    // メンバ変数  
    //------------------------------------------------------------------------------------  
    #region Field  

    private IState _currentState;   // 現在のステート
    private IState _prevState;      // 前のステート
    private UIFactory _factory;     
    private TurnProgress _progress;

    #endregion

    //------------------------------------------------------------------------------------  
    // コンストラクタ  
    //------------------------------------------------------------------------------------  
    #region Constructor  

    public TurnStateMachine(UIFactory uIFactory,TurnProgress turn)
    {
        _factory = uIFactory;
        _progress = turn;
    }

    #endregion

    //------------------------------------------------------------------------------------  
    // Public関数  
    //------------------------------------------------------------------------------------  
    #region PublicFunction  

    /// <summary>  
    /// そのステート間でやり続ける処理  
    /// </summary>  
    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnUpdate();
#if false
           UnityEngine.Debug.Log(_currentState);  
#endif
        }
    }

    /// <summary>  
    ///  入力を受け渡す  
    /// </summary>  
    /// <param name="input">入力</param>  
    public void HandleInput(InputData input)
    {
        // メニューボタンが押されたら。
        if(input.MenuPressed)
        {
            // メニューのステートじゃなかったらメニューにする
            if(!(_currentState is MenuState))
            {
                _prevState = _currentState;
                StateChangeBus.Publish(new ChangeStateRequest(() => new MenuState(_factory, RegisterPrevState)));
                return;
            }
        }
        _currentState?.HandleInput(input);
    }

    /// <summary>  
    /// ステートを設定する  
    /// </summary>  
    /// <param name="newState">次のステート</param>  
    public void SetState(IState newState)
    {
        _currentState?.OnEnd();
        _currentState = newState;

        // プレイヤーを使うインターフェースを実装していたらプレイヤーをセットする  
        if (_currentState is IPlayerUsable playerusable)
        {
            playerusable.SetPlayer(GameManager.Instance.CurrentPlayer);
        }

        if(_currentState is ITurnProgressUsable progressUsable)
        {
            progressUsable.SetTurnProgress(_progress);
        }
        _currentState.OnBegin();
    }

    #endregion

    //----------------------------------------------------------
    // Private関数
    //----------------------------------------------------------
    #region PrivateFunction

    /// <summary>
    /// 前のステートセットし直す
    /// </summary>
    private void RegisterPrevState()
    {
        _currentState = _prevState;
    }

    #endregion
}
