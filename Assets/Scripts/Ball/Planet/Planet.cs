using UnityEngine;

public abstract class Planet : MonoBehaviour
{
    //-----------------------------------------------------------------------------------
    // enum
    //-----------------------------------------------------------------------------------
    #region 惑星のタイプ
    public enum PlanetType : int
    {
        None = -1,           // このスクリプト用

        Earth,                 // 地球（プレイヤー）
        Mercury,              // 水星
        Venus,                // 金星
        Mars,                  // 火星
        Jupiter,               // 木星
        Saturn,               // 土星
        Uranus,              // 天王星
        Neptune,            // 海王星
        Pluto,                 // 冥王星
        Moon,                // 月
        Sun,                  // 太陽
        NewPlanet,         // 新惑星
        Comet                // 彗星
    };
    #endregion

    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field      
    [SerializeField] private int _planetScore;              // 惑星のスコア
    [SerializeField] private float _rayLength;              // レイの長さ
    [SerializeField] private bool _isStop;                  // 球が止まったかどうかのフラグ
    [SerializeField] private bool _isFall;                  // 球が落ちたかどうかのフラグ 
    [SerializeField] private PlanetType _planetType;        // 惑星 
    [SerializeField] private LayerMask _layerMask;          // レイヤーマスク
    private BallMovement _ballMove;                         // 球の動きを担うクラス
    private SphereCollider _sc;                             // 当たり判定用
    private bool _abilityFlag;                              // 惑星の能力のフラグ
    #endregion

    //------------------------------------------------------------------------------------
    // デリゲート
    //------------------------------------------------------------------------------------
    #region Delegate
    // 衝突時の点数を表示するUI
    public delegate void UI_AddScoreEffect(Vector3 vec3, int score, string _addScoreType);
    public UI_AddScoreEffect UI_TextEffect;
    #endregion

    //------------------------------------------------------------------------------------
    // プロパティ(ゲッター、セッター)
    //------------------------------------------------------------------------------------
    #region Property
    // 球の挙動を担うクラス
    public BallMovement BallMove
    {
        get => _ballMove; set => _ballMove = value;
    }
    // 当たり判定
    public SphereCollider Sphere
    {
        get => _sc; set => _sc = value;
    }
    /// 惑星のタイプ
    public PlanetType GetPlanetType
    {
        get => _planetType;
    }
    // 地面のレイヤー
    public LayerMask GroundLayer
    {
        get => _layerMask;
    }
    // 設置判定のレイの長さ
    public float RayLength
    {
        get => _rayLength;
    }
    // 止まったフラグ
    public bool IsStop
    {
        get => _isStop; set => _isStop = value;
    }
    // 落ちたフラグ
    public bool IsFall
    {
        get => _isFall; set => _isFall = value;
    }
    // 惑星の能力のフラグ
    public bool AbilityFlag
    {
        get => _abilityFlag; set => _abilityFlag = value;
    }

    #endregion

    //------------------------------------------------------------------------------------
    // Unityライフサイクル
    //------------------------------------------------------------------------------------
    #region LifeCycle

    //------------------------------------------------------------------------------------
    // Protected
    //------------------------------------------------------------------------------------

    // トリガーの衝突判定
    protected virtual void OnTriggerEnter(Collider other)
    {
        // 穴に落ちたら処理
        if (other.gameObject.CompareTag("Pocket"))
        {
            // 点数を加算
            GameManager.Instance.CurrentPlayer.AddScore(_planetScore);

            //------------ リセット処理--------------
            // フラグのリセット
            _isFall = true;
            _isStop = true;

            // 物理挙動のリセット
            _ballMove.Rb.velocity = Vector3.zero;   // 速度を０に補正
            _ballMove.Rb.Sleep();                   // 停止状態にする
            _ballMove.Rb.useGravity = false;        // 重力の使用を切る

            // 回転状態をリセット
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // 当たり判定を無くす
            _sc.enabled = false;
        }
    }

    //------------------------------------------------------------------------------------
    // Private
    //------------------------------------------------------------------------------------

    // オブジェクトの生成・重要な初期化処理
    private void Awake()
    {
        _ballMove = new BallMovement.Builder()          // BallMovementを生成
            .SetPlanet(this)
            .SetLayer(_layerMask)
            .SetRayLength(_rayLength)
            .Build();
    }

    // 初期化
    private void Start()
    {
        _abilityFlag = true;
        _isFall = false;
        _isStop = true;
        _ballMove.Rb = GetComponent<Rigidbody>();
        _sc = GetComponent<SphereCollider>();

        MyDebug.Assert( _sc != null );
        MyDebug.Assert(_ballMove.Rb != null );
    }

    // 物理演算用のUpdate関数
    private void FixedUpdate()
    {
        // 貫通したときの救済処置
        AdjustFall();
        if (_isFall) { return; }
        // 止まったかを判断する
        _isStop = _ballMove.TryStop();
    }

    // 物理衝突判定
    private void OnCollisionEnter(Collision collision)
    {
        // 惑星の能力処理
        Ability(collision);

        // 当たったのがPlanet型を継承しているオブジェクトだったら処理する
        if (collision.gameObject.GetComponent<Planet>() is Planet)
        {
            // 点数を入れてもいい状態なら処理
            if (EventManager.Instance.IsBlackHole == false)
            {
                // スコアを追加
                GameManager.Instance.CurrentPlayer.AddScore(50);
                // テキストを表示
                UI_TextEffect?.Invoke(transform.position, 50, "Conflict");
            }

            // そのターンにまだイベントが起きていなければ処理
            if (EventManager.Instance.GetEvent() == EventManager.EventType.None)
            {
                // イベントが起きるか計算する
                EventManager.Instance.DecideEvent();
            }
        }
    }

    #endregion

    //-----------------------------------------------------------------------------------
    // Public 関数
    //-----------------------------------------------------------------------------------
    #region PublicFunction

    #endregion

    //------------------------------------------------------------------------------------
    // Protected関数
    //------------------------------------------------------------------------------------
    #region ProtectedFunction
    // 惑星の能力処理
    protected abstract void Ability(Collision collision);

    #endregion

    //------------------------------------------------------------------------------------
    // Private関数
    //------------------------------------------------------------------------------------
    #region PrivateFunction

    // 万が一貫通してしまったときに強制的に落ちた判定にする処理
    private void AdjustFall()
    {
        if (transform.position.y < -20f)
        {
            _isFall = true;
            _isStop = true;
            _ballMove.Rb.velocity = Vector3.zero;
            _ballMove.Rb.Sleep();
            _ballMove.Rb.useGravity = false;
            _sc.enabled = false;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    #endregion
}