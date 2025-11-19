
public sealed class FoulState : IState
{
    //--------------------------------------------------
    // メンバ変数
    //--------------------------------------------------
    #region Field
    private Player _player;
    private PlayerFoul _playerFoul;
    private bool _isSet;
    #endregion

    //--------------------------------------------------
    // コンストラクタ
    //--------------------------------------------------
    #region Constructor
    public FoulState()
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
        _player = GameManager.Instance.GetRivalPlayer();
        _playerFoul = new PlayerFoul(_player.GetPlayerBallObj(), _player.GetPlayerBall());
        _playerFoul.PlacePlayerBall();      // PlayerBallを出現
        _isSet = false;                     // フラグの初期化
        MyDebug.CallRecord();
    }

    // この状態の間ずっと呼び出される処理
    void IState.OnUpdate()
    {
        // PlayerBallが設置されたらステートを移行
        if (_isSet)
        {
            StateChangeBus.Publish(new ChangeStateRequest(() => new IdleState()));
        }
    }

    // ステートが終了するときの処理
    void IState.OnEnd()
    {

    }

    void IState.HandleInput(InputData input)
    {
        // PlayerBallのファール処理
        _playerFoul.FoulProcess(out _isSet,input);
    }
    #endregion
}
