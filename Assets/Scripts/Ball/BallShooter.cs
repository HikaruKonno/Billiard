using System;
using UnityEngine;

/// <summary>
/// 球を打つときの具体的な処理クラス
/// </summary>
public sealed class BallShooter
{
    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    private GameObject _playerBallObj;      // プレイヤーの球
    private GameObject _cue;                // ビリヤードの棒
    private readonly float _addPower;       // 力を追加する値
    private readonly float _maxPower;       // 打つ力の最大値
    private readonly float _minPower;       // 打つ力の最小値
    private readonly float _rotationSpeed;  // 現在の回転スピード
    private float _currentRotationSpeed;    // 回転スピード
    private float _currentPower;            // 現在の打つ威力
    private float _directionGaugeValue;     // 方向ゲージの矢印の角度
    private float _directionGaugeTimer;     // 方向を決める際のタイマー
    #endregion

    //-----------------------------------------------------------
    // コンストラクタ
    //-----------------------------------------------------------
    public BallShooter(GameObject playerBall,GameObject cue,ShootData data)
    {
        _playerBallObj = playerBall;
        _cue = cue;
        _addPower = data.AddPower;
        _maxPower = data.MaxPower;
        _minPower = data.MinPower;
        _rotationSpeed = data.RotationSpeed;
    }

    //------------------------------------------------------------------------------------
    // プロパティ
    //------------------------------------------------------------------------------------
    #region Property
    // PlayerBallに渡すスピード
    public Vector3 Velocity { get; private set; }
    #endregion

    //------------------------------------------------------------------------------------
    // デリゲート
    //------------------------------------------------------------------------------------
    #region Delegate
    public static Action<float> PowerUI;
    public static Action<float> DirectionUI;
    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction

    /// <summary>
    /// 値のリセット
    /// </summary>
    public void ResetValue()
    {
        _currentPower = 0.0f;
        PowerUI(0.0f);
        _currentRotationSpeed = 0.0f;
        _directionGaugeValue = 90.0f;     // 矢印の方向を左下向きから始める
        DirectionUI(90.0f);
        _directionGaugeTimer = 0.0f;
    }

    /// <summary>
    /// 打つときの角度操作
    /// </summary>
    public void MoveAngle(InputData input,ref OperateState state)
    {
        // 決定ボタンが押されたら次のステートに移行
        if (input.DecidePressed)
        {
            ++state;
            return;
        }

        // 右押している時の処理
        if (input.RightPressed)
        {
            _currentRotationSpeed = -(_rotationSpeed * Time.deltaTime);
        }
        // 左・上のArrowキーを押している時処理
        else if (input.LeftPressed)
        {
            _currentRotationSpeed = _rotationSpeed * Time.deltaTime;
        }
        // 何のボタンも押されてなかったら移動量を0にする
        else if (!input.AnyPressed)
        {
            _currentRotationSpeed = 0.0f;
        }

        // 常に PlayerBall（地球）にY軸を向ける
        PointYTarget(_playerBallObj, _cue);
        // 球を回転
        RotationAngle(_playerBallObj, _cue);
    }

    /// <summary>
    /// 打つ力を決める
    /// </summary>
    public void DecidePower(InputData input, ref OperateState state)
    {
        // 決定ボタンが押されたら次のステートに移行
        if (input.DecidePressed)
        {
            ++state;
            return;
        }

        // 打つ力を加算
        _currentPower += _addPower;
        // 上限を超えたらリセット
        if (_maxPower < _currentPower)
        {
            _currentPower = 0.0f;
        }

        // 力の割合
        float ratio = _currentPower / _maxPower;
        // 現在の威力をUIに反映
        PowerUI?.Invoke(ratio);
    }

    /// <summary>
    /// 打つ方向を決める
    /// </summary>
    public void DecideDirection(InputData input, ref OperateState state)
    {
        // 決定ボタンが押されたら次のステートに移行
        if(input.DecidePressed)
        {
            ++state;
            return;
        }
        // 経過時間を計測する
        _directionGaugeTimer += Time.deltaTime;

        float amplitude = 90f; // 振幅
        float period = 2f; // 一往復する周期（秒）
        float phase = 0.5f; // 位相

        // 位相を考慮した時間補正
        float t = 4 * amplitude * (_directionGaugeTimer / period + phase);

        // 指定された振幅と周期のPingPong
        _directionGaugeValue = Mathf.PingPong(t, 2 * amplitude) - amplitude;

        // 現在の矢印の角度をUIに反映する
        if (DirectionUI != null) { DirectionUI(_directionGaugeValue); }
    }

    /// <summary>
    /// 球を打つときの処理(棒を動かす)
    /// </summary>
    public void ShootCue()
    {
        MyDebug.Assert(_cue.TryGetComponent<Rigidbody>(out var rb));

        // 値が0以下の場合最小値を代入
        if(_currentPower <= 0f)
        {
            _currentPower = _minPower;
        }

        // 打つ力を代入
        Vector3 shotPower = _cue.transform.forward * _currentPower;
        // 移動量をセット
        Velocity = shotPower;
        // キューの移動
        rb.velocity = shotPower;
    }

    /// <summary>
    /// ボールを打つ処理
    /// </summary>
    /// <param name="rb">打つ球のRigidbody</param>
    public void ShootBall(Rigidbody rb)
    {
        MyDebug.Assert(rb != null);

        Vector3 shotPower = _cue.transform.forward * _currentPower;
        rb.velocity = shotPower;
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Private関数
    //------------------------------------------------------------------------------------
    #region PrivateFunction

    /// <summary>
    /// 打つ玉の周りを回転する
    /// </summary>
    private void RotationAngle(GameObject playerBall, GameObject cue)
    {
        // playerballがqueを中心に回転する
        cue.transform.RotateAround
            (
            // queの座標（回転軸）
            playerBall.transform.position,
            // 回転軸
            Vector3.up,
             // 回転速度
             _currentRotationSpeed
            );
    }

    /// <summary>
    /// Que のY軸を常に Playerball に向ける関数
    /// </summary>
    private void PointYTarget(GameObject _playerBall, GameObject _cue)
    {
        // PlayerObj の座標を取得する
        Vector3 _targetPosition = _playerBall.transform.position;
        Vector3 QueDirection = (_targetPosition - _cue.transform.position);
        _cue.transform.rotation = Quaternion.LookRotation(QueDirection, Vector3.up);
    }
    #endregion
}
