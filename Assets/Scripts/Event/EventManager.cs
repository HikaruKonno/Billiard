using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlanetManager;

public class EventManager : MonoBehaviour
{
    //------------------------------------------------------------------------------------
    // Enum
    //------------------------------------------------------------------------------------
    #region Enum
    // 各イベントを番号に振り分ける
    public enum EventType
    {
        None,                     // なし
        BlackHoleExplosion,       // ブラックホール・超新星爆発（大号令）
        NewPlanetSpawn,           // 新惑星誕生
        completed                 // 作動済み
    };
    #endregion

    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    // Public
    // シングルトンクラスのため自分自身を持つ
    [System.NonSerialized]
    public static EventManager Instance;

    // Private
    [SerializeField]
    private GameObject _blackHoleObj;      //BlackHole オブジェクト
    [SerializeField] 
    private BlackHole _blackHole;          // ブラックホールの機能を持った変数
    [SerializeField]
    private int _blackHoleExplosionChance; // 超新星爆発（大号令）が起きる確率
    [SerializeField] 
    private int _newPlanetChance;          // 新惑星誕生が起きる確率
    private EventType _event;              // イベントを管理するための変数
    #endregion

    //------------------------------------------------------------------------------------
    // プロパティ
    //------------------------------------------------------------------------------------
    #region Property
    // ブラックホール状態かのフラグ
    public bool IsBlackHole { get ; set ; }
    #endregion

    //------------------------------------------------------------------------------------
    // Unityのライフサイクル
    //------------------------------------------------------------------------------------
    #region LifeCycle
    // シングルトンを使用する上の安全確保
    private void Awake()
    {
        // シングルトンの呪文
        if (Instance == null)
        {
            // 自身をインスタンスとする
            Instance = this;
        }
        else
        {
            // インスタンスが複数存在しないように、既に存在していたら自身を消去する
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // イベントを None に初期化
        _event = EventType.None;
        IsBlackHole = false;
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction
    /// <summary>
    /// フラグをリセットする関数
    /// </summary>
    public void ResetFlag()
    {
        _event = EventType.None;
    }

    /// <summary>
    /// どの Event がおきるか計算し Event の値を返す関数
    /// </summary>
    public void DecideEvent()
    {
        // 既にイベントが発生していたら終了
        if(_event != EventType.None) { return; }
        // ランダムの値を取得するための変数
        int RandomValue;
      
        // ランダムの値を取得 (１～１００）
        RandomValue = Random.Range(1, 101);

        // RandomValue の値で返り値を変更する
        // 超新星爆発 (BlackHoleExplosionChanceの値分のパーセントで処理される)
        if (RandomValue <= _blackHoleExplosionChance)
        {
            _event = EventType.BlackHoleExplosion;
            IsBlackHole = true;
            // ブラックホールを発生
            StartCoroutine(BlackHoleEvent(3.0f, PlanetManager.Instance.GetPlanets()));
        }
        // 新惑星誕生・ブラックホール (NewPlanetChanceの値分のパーセントで処理される)
        else if (-RandomValue <= -(100 - _newPlanetChance))
        {
            _event = EventType.NewPlanetSpawn;
        }
    }

    /// <summary>
    /// イベントを取得する関数
    /// </summary>
    /// <returns></returns>
    public EventType GetEvent()
    {
        return _event;
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Private関数
    //------------------------------------------------------------------------------------
    #region PrivateFunction
    /// <summary>
    /// ブラックホールのイベント
    /// </summary>
    /// <param name="doTime">ブラックホールを処理する時間</param>
    /// <param name="planets">現在の惑星</param>
    /// <returns></returns>
    private IEnumerator BlackHoleEvent(float doTime,List<Planet> planets)
    {
        float timer = 0.0f;
        // ブラックホールを出現
        _blackHoleObj.SetActive(true);
        // doTime秒処理する
        while (timer < doTime)
        {
            // ここに毎フレーム実行したい処理を記述
            // 経過時間を加算
            timer += Time.deltaTime;
            _blackHole.SuckedBlackHole(planets, _blackHoleObj);
            // 1フレーム待機
            yield return null;  
        }
        // ブラックホールを消す
        _blackHoleObj.SetActive(false);
        // 大号令
        _blackHole.Explosion(planets);
        IsBlackHole = false;
    }
#endregion
}
