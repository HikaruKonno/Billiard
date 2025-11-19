using UnityEngine;

public sealed class StartState : IState, IPlayerUsable
{
    //--------------------------------------------------
    // メンバ変数
    //--------------------------------------------------
    #region Field
    private Player _player;
    #endregion

    //--------------------------------------------------
    // コンストラクタ
    //--------------------------------------------------
    #region Constructor
    public StartState()
    {
        // TODO
        GameManager.Instance.TogglePlayerImpl();          // プレイヤーを切り替える
    }
    #endregion

    //--------------------------------------------------
    // インターフェース実装
    //--------------------------------------------------
    #region Interface
    // ステート最初の処理
    void IState.OnBegin()
    {
        _player.IsFoul = false;                           // ファールの状態をリセット
        EventManager.Instance.ResetFlag();                // イベントのフラグをリセット
        GameManager.Instance.ActiveCover(false);          // カバーを外す
        PlanetManager.Instance.RevivalBall();             // 惑星の復活
        // デバッカーに処理の流れを登録
        MyDebug.CallRecord();
    }

    // この状態の間ずっと呼び出される処理
    void IState.OnUpdate()
    {
        // 全てボールが着地し、止まっていたら次のステートに移行する
        if (PlanetManager.Instance.IsStoppedAllPlanets())
        {
            // 次のステートをルーレットにする
            StateChangeBus.Publish(new ChangeStateRequest(() => new RouletteState()));

        }
    }

    // ステートが終了するときの処理
    void IState.OnEnd()
    {

    }

    void IState.HandleInput(InputData input)
    {

    }

    void IPlayerUsable.SetPlayer(Player player)
    {
        _player = player;
    }
    #endregion
}
