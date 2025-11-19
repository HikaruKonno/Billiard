using UnityEngine;
using static PlanetManager;
public sealed class IdleState : IState
{
    //--------------------------------------------------
    // メンバ変数
    //--------------------------------------------------
    #region Field
    private float _time = 0.0f;
    #endregion

    //--------------------------------------------------
    // コンストラクタ
    //--------------------------------------------------
    #region Constructor
    public IdleState()
    {

    }
    #endregion

    //--------------------------------------------------
    // インターフェース実装
    //--------------------------------------------------
    #region Interface
    // ステート最初の処理
    public void OnBegin()
    {
        MyDebug.CallRecord();
    }

    // この状態の間ずっと呼び出される処理
    public void OnUpdate()
    {
        // 全てボールが止まっていたらOnExitに移行
        if (Instance.IsStoppedAllPlanets())
        {
            // 次のステートを打つステートにする
            _time += Time.deltaTime;
            if(_time > 1.0f)
            {
                StateChangeBus.Publish(new ChangeStateRequest(() => new ShootState()));
                _time = 0.0f;
            }
        }
    }

    // ステートが終了するときの処理
    public void OnEnd()
    {

    }

    public void HandleInput(InputData input)
    {

    }
    #endregion 
}
