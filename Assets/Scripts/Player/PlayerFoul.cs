using UnityEngine;

public class PlayerFoul
{
    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    private PlayerBall _playerBall;         // プレイヤー
    private GameObject _playerBallObj;      // 地球のオブジェクト
    private Color _colorRed;                // 置けないときの色
    private float _moveSpeed;               // 移動スピード
    private float _radius;                  // 半径
    private float _leftLimit;               // 左の移動制限
    private float _rightLimit;              // 右の移動制限 
    private float _frontLimit;              // 前の移動制限
    private float _rearLimit;               // 後ろの移動制限
    private bool _canSettingFlag;           // 置けるかのフラグ
    #endregion

    //-----------------------------------------------------------------------------------
    // コンストラクタ
    //-----------------------------------------------------------------------------------
    #region Constructor
    public PlayerFoul(GameObject obj, PlayerBall playerball)
    {
        _playerBallObj = obj;
        _playerBall = playerball;
        Init();
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction

    /// <summary>
    /// ファールの処理
    /// </summary>
    public void FoulProcess(out bool _isSet, InputData input)
    {
        // ファール処理
        _playerBallSetting(input);

        // ある条件に合う場所で指定のボタンが押されたらその位置に地球をアクティブ
        // ※下に他のボールがある || 穴に落ちてしまう場所は設置は条件に合わない(_canSettingFlag で制御)
        if (_canSettingFlag)
        {
            // TODO
            if (input.DecidePressed)
            {
                // PlayerBall を物理判定をアクティブ
                _playerBall.SetActiveRigidbody(false);
                _playerBall.IsFall = false;
                _playerBall.BallMove.Rb.useGravity = true;
                _playerBall.Sphere.enabled = true;
                _isSet = true;
                return;
            }
        }
        _isSet = false;
    }

    /// <summary>
    /// PlayerBallを配置する関数
    /// </summary>
    public void PlacePlayerBall()
    {
        // PlayerBall の物理判定を非アクティブにする
        _playerBall.SetActiveRigidbody(true);
        // ビリヤード台の真ん中に調整
        _playerBall.transform.position = new Vector3(0.0f, 15.0f, 0.0f);
        _playerBall.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Private関数
    //------------------------------------------------------------------------------------
    #region PrivateFunction
    /// <summary>
    /// ファールの処理で PlayerBall を動かすための関数
    /// </summary>
    private void _playerBallSetting(InputData input)
    {
        // SphereCast で検知した情報をもとに設置可能かどうかを判断する
        _canSettingFlag = _canSetting_SphereCast();

        // PlayerBall の位置を移動させる処理
        PlayerBallMovement(input);

        // PlayerBall が設置可能かどうかを色を変えて伝える
        ColorChange(_canSettingFlag);
    }

    /// <summary>
    /// SphereCast の情報から地球が置けるかどうかを判断する関数
    /// 戻り値：_canSetting
    /// </summary>
    /// <returns>bool</returns>
    private bool _canSetting_SphereCast()
    {
        // 地球がおける状態か管理する変数
        bool _canSetting = true;

        //  SphereCast で当たったものを保管する変数
        RaycastHit hit;

        // SphereCast を行い
        if (Physics.SphereCast(_playerBallObj.gameObject.transform.position, _radius,
            Vector3.down, out hit, 30.0f, LayerMask.GetMask("_ballLayer") + LayerMask.GetMask("_poketLayer") + LayerMask.GetMask("_tableWallLayer")))
        {
            _canSetting = false;
        }
        return _canSetting;
    }

    /// <summary>
    /// PlayerBallの移動
    /// </summary>
    private void PlayerBallMovement(InputData input)
    {
        //現在のポジションを保持する
        Vector3 currentPos = _playerBallObj.transform.position;

        // PlayerBall を移動させる
        //左キーが押されたら
        if (input.LeftPressed)
        {
            //左に移動させる
            currentPos.x -= _moveSpeed;
        }
        //右キーが押されたら
        if (input.RightPressed)
        {
            //右に移動させる
            currentPos.x += _moveSpeed;
        }
        //上キーが押されたら
        if (input.UpPressed)
        {
            //上に移動させる
            currentPos.z += _moveSpeed;
        }
        //下キーが押されたら
        if (input.DownPressed)
        {
            //下に移動させる
            currentPos.z -= _moveSpeed;
        }

        // X,Yの値それぞれが最小～最大の範囲内に収める。
        // 範囲を超えていたら範囲内の値を代入する
        currentPos.x = Mathf.Clamp(currentPos.x, _leftLimit, _rightLimit);
        currentPos.z = Mathf.Clamp(currentPos.z, _frontLimit, _rearLimit);

        // positionをcurrentPosにする
        _playerBallObj.transform.position = currentPos;
    }

    /// <summary>
    /// ファール処理の際、設置可能でない場合色を通常より赤く表示する関数
    /// </summary>
    private void ColorChange(bool _canSettingFlag)
    {
        // PlayerBall の子オブジェクトの Collider を取得する
        GameObject _child = _playerBallObj.transform.gameObject;

        // 設置不可能の場合赤色に表示する
        if (!_canSettingFlag)
        {
            _child.GetComponent<Renderer>().material.color = _colorRed;
        }
        // 通常の色に戻す
        else
        {
            _child.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    // 初期化
    private void Init()
    {
        _colorRed = Color.red;
        _moveSpeed = 1f;
        _radius = 5f;
        _leftLimit = -60f;
        _rightLimit = 60f;
        _frontLimit = -30f;
        _rearLimit =  30f;
    }
    #endregion
}
