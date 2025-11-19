using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    //--------------------------------------------------
    // Public
    //--------------------------------------------------
    // シングルトンのため自分自身を持つ
    [System.NonSerialized]
    public static PlanetManager Instance;
    // 惑星同士の衝突で点数を入れるか管理するためのFlag
    public bool AddScoreFlag;

    //---------------------------------------------------
    // Private
    //---------------------------------------------------
    [SerializeField, Header("惑星")]
    private List<Planet> _planetList;
    [SerializeField, Header("カバー")]
    private GameObject _cover;
    [SerializeField, Header("スコアが加算されたときのUIクラス")]
    private AddScoreTextManager _addScoreTextManager;
    [SerializeField, Tooltip("惑星を生成する範囲A")]
    private Transform _randomRivivalPosA;
    [SerializeField, Tooltip("惑星を生成する範囲B")]
    private Transform _randomRivivalPosB;
    #endregion

    //------------------------------------------------------------------------------------
    // Unity のライフサイクル
    //------------------------------------------------------------------------------------
    #region LifeCycle

    //シングルトンを使用する上の安全確保
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

    // 最初の１フレームに呼び出される
    void Start()
    {
        // 各球が止まったかのフラグをリセットする
        ResetPlanetStopFlag();
        // 球が当たったときのUI処理を全惑星に代入
        for (int i = 0; i < _planetList.Count; i++)
        {
            _planetList[i].UI_TextEffect = _addScoreTextManager.AddScoreEffect;
        }

    }
    #endregion

    //------------------------------------------------------------------------------------
    // Public 関数
    //------------------------------------------------------------------------------------
    #region PublicFunction

    /// <summary>
    /// プラネットマネージャーに惑星の登録をする
    /// </summary>
    /// <param name="planet">登録する惑星</param>
    public void RegisterPlanet(Planet planet)
    {
        _planetList.Add(planet);
    }

    /// <summary>
    /// 全ての Planet の止まったフラグ を false にリセットする関数
    /// </summary>
    public void ResetPlanetStopFlag()
    {
        // 惑星の止まったフラグをfalseにする
        for (int i = 0; i < _planetList.Count; ++i)
        {
            _planetList[i].IsStop = false;
        }

        // 惑星同士の衝突で点数を入れれる状態にする
        AddScoreFlag = true;
    }

    /// <summary>
    /// 落ちた惑星の復活
    /// </summary>
    public void RevivalBall()
    {
        //落ちた惑星が全部復活するまで繰り返す
        foreach (var planet in _planetList)
        {
            if (planet.IsFall)
            {
                // 地球は復活させないので処理を飛ばす
                if (planet.GetType() == typeof(PlayerBall)) { continue; }

                // _randomRivivalPosAと_randomRivivalPosBの座標の範囲内でランダムな数値を作成
                float x = Random.Range(_randomRivivalPosA.position.x, _randomRivivalPosB.position.x);
                float y = Random.Range(_randomRivivalPosA.position.y, _randomRivivalPosB.position.y);
                float z = Random.Range(_randomRivivalPosA.position.z, _randomRivivalPosB.position.z);

                // 落ちた惑星をランダムで取った値の座標に移動
                planet.transform.position = new Vector3(x, y + 100.0f, z);
                // 停止状態を解除する
                planet.BallMove.Rb.WakeUp();
                // スポーンした瞬間に止まらないように下方向に力を加える
                planet.BallMove.Rb.AddForce(x, -100.0f, z);

                planet.Sphere.enabled = true;         // 当たり判定を復活
                planet.BallMove.Rb.useGravity = true; // 重力を使う

                // フラグのリセット
                planet.IsStop = false;
                planet.IsFall = false;
            }
        }
    }

    /// <summary>
    /// 惑星の構造体配列を参照してボールが止まったか確認する
    /// </summary>
    /// <returns>全て止まってたらtrue</returns>
    public bool IsStoppedAllPlanets()
    {
        // 返り値宣言
        bool isStop = true;
        // 全ての惑星が止まっているか判断する
        for (int i = 0; i < _planetList.Count; ++i)
        {
            // 止まってなかったら判定を終了
            if (!_planetList[i].IsStop)
            {
                isStop = false;
                break;
            }
        }
        return isStop;
    }

    /// <summary>
    /// 現在の惑星を取得する
    /// </summary>
    /// <returns>惑星のリスト</returns>
    public List<Planet> GetPlanets()
    {
        return _planetList;
    }

    //------------------------------------------------------------------------
    // Private 関数
    //------------------------------------------------------------------------
    
    // デバッグ用
    // 各惑星のレイの長さを可視化する
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var planet in _planetList)
        {
            Gizmos.DrawRay(planet.transform.position, Vector3.down * planet.RayLength);
        }
    }

    #endregion
}