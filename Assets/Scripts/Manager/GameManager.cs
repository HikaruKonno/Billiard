using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public sealed class GameManager : MonoBehaviour
{
    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    //-----------------------------------------------------------------
    // インスペクターで設定
    //-----------------------------------------------------------------
    #region Inspector

    [SerializeField, Header("プレイヤーの設定用")]
    private PlayerSetting _playerSetting;
    [SerializeField, Header("地球のオブジェクト")]
    private GameObject _playerBallObject;
    [SerializeField, Header("キューのオブジェクト")]
    private GameObject _cueObject;
    [SerializeField, Header("カバーのオブジェクト")]
    private GameObject _cover;
    [SerializeField, Header("ターン数")]
    private int _gameTurn;
    [SerializeField, Header("スコアのUIクラス")]
    private ScoreUIViewer _scoreUIViewer;
    [SerializeField, Header("メニューのUI")]
    private UIFactory _uiFactory;
    [SerializeField, Header("現在のプレイヤーのフォーカスをするクラス")]
    private CurrentPlayerText _currentPlayerText;
    [SerializeField, Header("ターン数表示するためのUI")]
    private TurnUIText _turnUIText;
    #endregion

    private TurnProgress _turnProgress;
    private Player _player1;                                 // プレイヤー
    private Player _player2;
    private TurnStateMachine _turnStateMachine;              // ターンのステートマシン
    #endregion

    //------------------------------------------------------------------------------------
    // プロパティ
    //------------------------------------------------------------------------------------
    #region Property

    // 自分自身のインスタンス
    public static GameManager Instance {get ; private set;}
    // 現在のプレイヤー情報
    public Player CurrentPlayer { get; private set; }

    #endregion

    //------------------------------------------------------------------------------------
    // Unityライフサイクル
    //------------------------------------------------------------------------------------
    #region LifeCycle

    // 生成処理・重要度が高い初期化処理
    private void Awake()
    {
        // 安全確認
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        _playerBallObject = Instantiate(_playerBallObject);           // 地球オブジェクトを生成
        _cueObject = Instantiate(_cueObject);                         // キューオブジェクトを生成
        CreatePlayer(ref _player1,Player.Number.Player1);             // プレイヤーの生成
        CreatePlayer(ref _player2, Player.Number.Player2);
        CurrentPlayer = _player2;                                     // プレイヤーの設定
        _turnProgress = new TurnProgress(_gameTurn,_turnUIText,OnStateChange); // ターン経過の処理
        _turnStateMachine = new TurnStateMachine(_uiFactory,_turnProgress);         // ステートマシーン生成
        StateChangeBus.Subscribe<ChangeStateRequest>(OnStateChange);  // イベントバスの登録
    }

    // セット処理
    private void Start()
    {
        // 地球をPlanetManagerに登録する
        PlanetManager.Instance.RegisterPlanet(_playerBallObject.GetComponent<PlayerBall>());
        // スコアのロジックとビュワーをバインドさせる
        Player.ScoreUIModel = _scoreUIViewer.UpdateScore;
        // ステートの初期化
        StateChangeBus.Publish(new ChangeStateRequest(() => new StartState()));

        MyDebug.CallRecord();
    }

    private void Update()
    {
        // 入力処理
        var input = InputHandler.GetInput();
        _turnStateMachine.HandleInput(input);

        // 現在のステートでやり続ける処理
        _turnStateMachine.Update();
    }

    // オブジェクトが破棄されるときに呼び出される
    private void OnDestroy()
    {
        // EventBusからOnStateChangeメソッドの登録を解除する
        StateChangeBus.UnSubscribe<ChangeStateRequest>(OnStateChange);
    }

    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction

    /// <summary>
    /// プレイヤーを切り替える
    /// </summary>
    public void TogglePlayerImpl()
    {
        // 現在のプレイヤーを切り替え
        CurrentPlayer = (CurrentPlayer == _player1) ? _player2 : _player1;
        // プレイヤーのフォーカスの切り替え
        _currentPlayerText.SelectingPlayer(CurrentPlayer);
    }

    /// <summary>
    /// 相手プレイヤーを取得する
    /// </summary>
    /// <returns>相手プレイヤー</returns>
    public Player GetRivalPlayer()
    {
        return (CurrentPlayer == _player1) ? _player2: _player1; ;
    }

    /// <summary>
    /// 勝者を返す
    /// </summary>
    /// <returns>勝ったほうのPlayer or 引き分けだったらnull</returns>
    public Player GetWinnerPlayer()
    {
        if (_player1.Score > _player2.Score)
        {
            return _player1;
        }
        else if (_player1.Score < _player2.Score)
        {
            return _player2;
        }

        return null;
    }

    /// <summary>
    /// カバーのアクティブ状態を操作
    /// </summary>
    /// <param name="isActive">trueならアクティブ状態にする</param>
    public void ActiveCover(bool isActive)
    {
        // カバーをアクティブ状態を操作する
        _cover.SetActive(isActive);
    }

    #endregion

    //------------------------------------------------------------------
    // Private関数
    //------------------------------------------------------------------
    #region PrivateFunction

    // ステートを変える
    private void OnStateChange(ChangeStateRequest state)
    {
        var nextState = state.Factory();        // ← この時点で new が発生
        _turnStateMachine.SetState(nextState);  // ← OnEnd(), OnBegin() 自動呼び出し
    }

    // プレイヤーを作成する
    private void CreatePlayer(ref Player player,Player.Number number)
    {
        // ビルダーで各項目を設定
        player = new Player.Builder()
                     .SetBilliardCue(_cueObject.GetComponent<BilliardCue>())
                     .SetBilliardCueObj(_cueObject)
                     .SetPlayerBall(_playerBallObject.GetComponent<PlayerBall>())
                     .SetPlayerBallObj(_playerBallObject)
                     .SetShootData(_playerSetting.shootData)
                     .Build();

        player.PlayerNumber = number;
    }

    #endregion
}