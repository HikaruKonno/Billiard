using UnityEngine;
using static PlanetManager;


public sealed class ShootState : IState, IPlayerUsable
{
    //--------------------------------------------------------
    // メンバ変数
    //--------------------------------------------------------
    #region Field
    private Player _player;
    private ShootData _shootData;
    #endregion

    //--------------------------------------------------------
    // コンストラクタ
    //--------------------------------------------------------
    #region Constructor
    public ShootState()
    {

    }
    #endregion

    //------------------------------------------------------
    // インターフェース実装
    //------------------------------------------------------
    #region Interface
    // ステート最初の処理
    void IState. OnBegin()
    {
        // ビリヤードの棒を復活させる
        _player.RivaivalQue();
        // 打つ処理のリセット
        _player.GetPlayerOperate().Reset();
        GameManager.Instance.ActiveCover(true);         // カバーをつける
        MyDebug.CallRecord();
    }

    // この状態の間ずっと呼び出される処理
    void IState.OnUpdate()
    {
        // プレイヤーが打ったあと全てのボールが止まっていたらステートの移行
        if (_player.IsShoot && Instance.IsStoppedAllPlanets())
        {
            // ステートをエンドに移行する
            StateChangeBus.Publish(new ChangeStateRequest(() => new EndState()));
        }
    }

    // ステートが終了するときの処理
    void IState.OnEnd()
    {

    }

    // 入力処理
    void IState.HandleInput(InputData input)
    {
        _player.OperateInput(input);
    }

    // プレイヤーをセット
    void IPlayerUsable.SetPlayer(Player player)
    {
        _player = player;
    }
    #endregion
}
