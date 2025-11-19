using System;
using UnityEngine;

public class Player
{
    //---------------------------------------------------------
    // ビルダー
    //---------------------------------------------------------
    #region Builder
    public class Builder
    {
        public Builder() { }
        public GameObject playerBallObj { get; private set; }
        public GameObject billiardCueObj { get; private set; }
        public PlayerBall playerBall { get; private set; }
        public BilliardCue billiardCue { get; private set; }
        public ShootData _shootData { get; private set; }

        public Player Build()
        {
            return new Player(this);
        }
        public Builder SetPlayerBallObj(GameObject obj)
        {
            playerBallObj = obj;
            return this;
        }
        public Builder SetBilliardCueObj(GameObject obj)
        {
            billiardCueObj = obj;
            return this;
        }
        public Builder SetPlayerBall(PlayerBall ball)
        {
            playerBall = ball;
            return this;
        }
        public Builder SetBilliardCue(BilliardCue cue)
        {
            billiardCue = cue;
            return this;
        }
        public Builder SetShootData(ShootData data)
        {
            _shootData = data;
            return this;
        }
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Enum
    //------------------------------------------------------------------------------------
    public enum Number
    {
        Player1,
        Player2
    }

    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    private GameObject _playerBallObj;     // 地球のオブジェクト
    private GameObject _billiardCueObj;    // キューのオブジェクト
    private PlayerOperate _playerOperate;  // プレイヤーの操作
    private PlayerBall _playerBall;        // プレイヤーボールクラス
    private BilliardCue _billiardCue;      // ビリヤードの棒 
    private ShootData _shootData;          // 打つデータ
    private int _score;                    // 点数
    private bool _isShoot;                 // プレイヤーが球を打ったか
    private bool _isFoul;                  // ファールかどうか
    #endregion

    //------------------------------------------------------------------------------------
    // プロパティ
    //------------------------------------------------------------------------------------
    #region Property
    // プレイヤーの番号
    public Number PlayerNumber
    {
        get;
        set;
    }
    // 球を打ったか
    public bool IsShoot
    {
        get => _isShoot;
        set => _isShoot = value;
    }
    // ファールの状態か
    public bool IsFoul
    {
        get => _isFoul; 
        set => _isFoul = value;
    }
    // スコアを取得
    public int Score
    {
        get => _score;
    }
    #endregion

    //------------------------------------------------------------------------------------
    // デリゲート
    //------------------------------------------------------------------------------------
    #region Delegate
    // スコアが更新された時のUI処理
    public static Action<Number, int> ScoreUIModel;
    #endregion

    //------------------------------------------------------------------------------------
    // コンストラクタ
    //------------------------------------------------------------------------------------
    #region Constructor

    // 初期化
    public Player(Builder build)
    {
        // 設定
        _playerBallObj = build.playerBallObj;
        _billiardCueObj = build.billiardCueObj;
        _playerBall = build.playerBall;
        _billiardCue = build.billiardCue;
        _shootData = build._shootData;

        // プレイヤーの操作クラスの生成
        _playerOperate = new PlayerOperate.Builder()
                             .SetCue(_billiardCueObj)
                             .SetBall(_playerBallObj)
                             .SetShoot(new BallShooter(_playerBallObj,_billiardCueObj,_shootData))
                             .Build();
        _isShoot = false;
        _score = 0;
    }

    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction

    public GameObject GetPlayerBallObj()
    {
        return _playerBallObj;
    }

    public PlayerBall GetPlayerBall()
    {
        return _playerBall;
    }

    public PlayerOperate GetPlayerOperate()
    {
        return _playerOperate;
    }

    /// <summary>
    /// 入力内容によって操作をする
    /// </summary>
    /// <param name="input">入力内容</param>
    public void OperateInput(InputData input)
    {
        _playerOperate.Action(input);
    }

    /// <summary>
    /// キューを復活させる
    /// </summary>
    public void RivaivalQue()
    {
        Vector3 _instansPos = _playerBallObj.transform.position;      // スポーンさせる座標用の変数

         // プレイヤーの座標からXを＋１０した場所にする
        _instansPos = new Vector3(_instansPos.x + 10.0f, _instansPos.y, _instansPos.z);   
        _billiardCueObj.SetActive(true);                              // キューをアクティブ状態にする
        _billiardCueObj.transform.position = _instansPos;             // キューの座標を変更

        // キューを地球に向けるようにする
        Vector3 _targetPosition = _playerBall.transform.position;
        Vector3 QueDirection = (_targetPosition - _billiardCueObj.transform.position);
        _billiardCueObj.transform.rotation = Quaternion.LookRotation(QueDirection, Vector3.up);

        // キューの物理演算を取得
        _billiardCueObj.TryGetComponent<Rigidbody>(out var rb);
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.Sleep();                                                   // とにかく復活したとき動かないように止める！！！
    }

    /// <summary>
    /// 値のリセット
    /// </summary>
    public void ResetValue()
    {
        _isShoot = false;
    }

    /// <summary>
    /// スコア加算の関数
    /// </summary>
    /// <param name="score">追加するスコアの点数</param>
    public void AddScore(int score)
    {
        _score += score;    //スコアの加算
        ScoreUIModel?.Invoke(PlayerNumber, _score); // UIの更新
    }

    #endregion
}


