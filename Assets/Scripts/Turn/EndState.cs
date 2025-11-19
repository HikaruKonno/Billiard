using static PlanetManager;

public class EndState : IState,IPlayerUsable,ITurnProgressUsable
{
    //--------------------------------------------------
    // メンバ変数
    //--------------------------------------------------
    #region Field
    private Player _player;
    private TurnProgress _turnProgress;
    #endregion

    //--------------------------------------------------
    // コンストラクタ
    //--------------------------------------------------
    #region Constructor
    public EndState()
    {

    }
    #endregion

    //--------------------------------------------------
    // インターフェース実装
    //--------------------------------------------------
    #region Interface
    // ステート最初の処理
    void IState.OnBegin()
    {
        MyDebug.CallRecord();
    }

    // この状態の間ずっと呼び出される処理
    void IState.OnUpdate()
    {
        // 全てボールが止まっていたらOnExitに移行
        if (Instance.IsStoppedAllPlanets())
        {
            // 次のステートをスタートにする
            StateChangeBus.Publish(new ChangeStateRequest(() => new StartState()));
        }
    }

    // ステートが終了するときの処理
    void IState.OnEnd()
    {
        _player.ResetValue();
        _turnProgress.Progress();    // ターンの経過
    }

    void IState.HandleInput(InputData input)
    {

    }

    void IPlayerUsable.SetPlayer(Player player)
    {
        _player = player;
    }

    void ITurnProgressUsable.SetTurnProgress(TurnProgress turn)
    {
        _turnProgress = turn;
    }
    #endregion
}
