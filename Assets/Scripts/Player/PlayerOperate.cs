using System;
using UnityEngine;

public enum OperateState : byte
{
    ROTATION = 0,   // 回転
    POWER,          // 威力を決める
    DERECTION,      // 方向を決める
    SHOOT,          // 打つ
    END             // 打ち終わった状態
}

[System.Serializable]
public class PlayerOperate
{
    // ビルダー
    #region Builder
    public class Builder
    {
        public BallShooter Shooter { get; private set; }
        public GameObject CueObj { get; private set; }
        public GameObject BallObj { get; private set; }

        public Builder() { }

        public Builder SetShoot(BallShooter shooter)
        {
            this.Shooter = shooter;
            return this;
        }

        public Builder SetCue(GameObject cueObj)
        {
            this.CueObj = cueObj;
            return this;
        }

        public Builder SetBall(GameObject ballObj)
        {
            this.BallObj = ballObj;
            return this;
        }

        public PlayerOperate Build()
        {
            return new PlayerOperate(this);
        }
    }
    #endregion

    //----------------------------------------------
    // メンバ変数
    //----------------------------------------------
    #region Field
    [SerializeField] private ShootData _shootdata;
    private GameObject _playerBallObj;
    private GameObject _cue;
    private BallShooter _ballShooter;
    private OperateState _state;
    #endregion

    //-------------------------------------------------
    // コンストラクタ
    //-------------------------------------------------
    public PlayerOperate(Builder builder)
    {
        _playerBallObj = builder.BallObj;
        _cue = builder.CueObj;
        _ballShooter = builder.Shooter;
    }

    //----------------------------------------------------
    // Public関数
    //----------------------------------------------------
    #region PublicFunction
    
    // PlayerBallオブジェクトをセット
    public void SetPlayerBallObj(GameObject obj)
    {
        _playerBallObj = obj;
    }
    // Cueオブジェクトをセット
    public void SetCueObj(GameObject obj)
    {
        _cue = obj;
    }
    // BallShooterを取得
    public BallShooter GetBallShooter()
    {
        return _ballShooter;
    }

    public void Reset()
    {
        _state = OperateState.ROTATION;
        _ballShooter.ResetValue();
    }

    /// <summary>
    /// プレイヤーの操作
    /// </summary>
    public void Action(InputData input)
    {
        if (_state == OperateState.ROTATION)
        {
            // どの方向に打つかを決める
            _ballShooter.MoveAngle(input,ref _state);
        }
        else if (_state == OperateState.POWER)
        {
            // 威力を決める
            _ballShooter.DecidePower(input, ref _state);
        }
        else if (_state == OperateState.DERECTION)
        {
            // 角度ゲージを決める
            _ballShooter.DecideDirection(input,ref _state);
        }
        else if (_state == OperateState.SHOOT)
        {
            // 球を打つ(正確には棒を動かす)
            _ballShooter.ShootCue();
            _state = OperateState.END;
        }
    }
    #endregion
}

