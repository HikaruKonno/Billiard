using UnityEngine;
using static PlanetManager;

public sealed class RouletteState : IState
{
    //--------------------------------------------------
    // メンバ変数
    //--------------------------------------------------
    #region Field
    private Player _rival;        // 相手のプレイヤー
    #endregion

    //--------------------------------------------------
    // コンストラクタ
    //--------------------------------------------------
    #region Constructor
    public RouletteState()
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
        _rival = GameManager.Instance.GetRivalPlayer();
    }

    // この状態の間ずっと呼び出される処理
    void IState.OnUpdate()
    {
        // 全てボールが止まっていたらOnEndに移行
        if (Instance.IsStoppedAllPlanets())
        {
            // 相手がファールをしていたらファールのステートに移行
            // そうでなかったらアイドルのステートに移行
            if(_rival.IsFoul)
            {
                StateChangeBus.Publish(new ChangeStateRequest(() => new FoulState()));
            }
            else
            {
                StateChangeBus.Publish(new ChangeStateRequest(() => new IdleState()));
            }
            
        }
    }

    // ステートが終了するときの処理
    void IState.OnEnd()
    {

    }

    
    void IState.HandleInput(InputData input)
    {

    }

    #endregion
}
