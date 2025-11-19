using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI弄るときに必須
using static GameManager;

public class CometControl
{
    //// TextをList化してInspecter上に表示する
    //[SerializeField] Text[] commandlist;

    //// ImageをList化してInspecter上に表示する
    ///*[SerializeField] Image[] commandlist;*/

    //// 選択されたもののList番号
    //public int lastTime;

    //// 選択されているもののList番号
    //private float _countTime;

    //// ルーレットの速さ
    //private float _speed;

    //// RouletteText の Objcet を Inspector で入れる
    //[SerializeField]
    //private GameObject _rouletteTextObject;

    //// PlayerBall の Objcet を Inspector で入れる
    //[SerializeField]
    //private GameObject _playerBallObject;
    //// マネージャーオブジェクトを格納するための変数
    //[SerializeField]
    //private GaugeControl _gaugeControlObj;
    //// GameManager型を格納するための変数
    //private GaugeControl _gaugeControl;

    //// 自分の玉のサイズ
    //private Vector3 _playerBallSize;

    //// ルーレットが回っているかどうかのflag
    //private bool _isMoveRouletto;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // GameManager型を取得
    //    _gaugeControl = _gaugeControlObj.GetComponent<GaugeControl>();

    //    // 初期化
    //    _speed = 0f;
    //    _countTime = 0f;
    //    _isMoveRouletto = false;

    //    // 自分の玉のサイズを代入する
    //    _playerBallSize = new Vector3(0.2f, 0.2f, 0.2f);

    //}

    //void Update()
    //{
    //}

    ///// <summary>
    ///// ルーレットのコルーチン
    ///// </summary>
    //IEnumerator Rouletto()
    //{
    //    //終わるまで待ってほしい処理
    //    //ここで(int)countTimeを他スクリプトに送信
    //    lastTime = (int)_countTime;

    //    //待ち
    //    yield return new WaitForSeconds(3);

    //    //以下、再開してから実行したい処理

    //    // ルーレットを非表示にする
    //    _rouletteTextObject.SetActive(false);

    //    // ルーレットが回っているかどうかのflagをfalseにする
    //    _isMoveRouletto = false;

    //    //彗星が落ちたかのフラグを false にする
    //    EventManager.EventMng.StartingCometRouletteFlag = false;
    //}

    ///// <summary>
    ///// ルーレットが動いている場合のコルーチン
    ///// 願いの値の代入をルーレットを止めてから代入したいためコルーチンを使用している
    ///// </summary>
    //IEnumerator MovingRouletto()
    //{
    //    //Debug.Log("ルーレットが動いているためコルーチンを開始させる");
    //    yield return new WaitUntil(() => _isMoveRouletto == false);
    //    //Debug.Log("ルーレットが止まったためコルーチンを終了させる");

    //    //以下、再開してから実行したい処理

    //    // 願いの抽選の結果の代入
    //    int _cometRandom = lastTime;

    //    // [Debug用]願いの結果を入れる変数 = 願い
    //    /*int _cometRandom = 2;*/

    //    // 現在のプレイヤーのターンの代入
    //    GameManager.PlayerTurn PlayerTurn = GameManager.GameMng.GetPlayerTurn();
    //    //Debug.Log(_cometRandom);

    //    // イベントマネージャーに彗星の願いの番号を渡し、願いのFlagをtrueにする
    //    if (PlayerTurn == GameManager.PlayerTurn.Player1)
    //    {
    //        EventManager.EventMng.Player1CometWishNumber = _cometRandom;
    //        EventManager.EventMng.Player1WishFlag = true;
    //    }
    //    else
    //    {
    //        EventManager.EventMng.Player2CometWishNumber = _cometRandom;
    //        EventManager.EventMng.Player2WishFlag = true;
    //    }

    //    // 抽選された彗星の願いが２の場合(動作確認済み)
    //    if (_cometRandom == 1)
    //    {
    //        // 次のターンも俺のターン！ 完成
    //        // プレイヤーのターンを変えるFlagをfalseにする
    //        GameManager.GameMng.ChangeTurnFlag = false;
    //        //Debug.Log("次のターンも俺のターン");

    //        // プレイヤーのWishFlagをfalseにする処理
    //        if (PlayerTurn == GameManager.PlayerTurn.Player1)
    //        {
    //            ResetPlayerWishFlag(GameManager.PlayerTurn.Player1);
    //        }
    //        else
    //        {
    //            ResetPlayerWishFlag(GameManager.PlayerTurn.Player2);
    //        }
    //    }
    //    //Debug.Log("Player1のCometWishNum " + EventManager.EventMng.Player1CometWishNumber);
    //    //Debug.Log("Player2のCometWishNum " + EventManager.EventMng.Player2CometWishNumber);
    //    // 彗星が落ちたかどうかを確認するflagをfalseにする
    //    EventManager.EventMng.CometisFallFlag = false;
    //}

    ///// <summary>
    ///// ルーレットを開始させる処理
    ///// </summary>
    //public void StartRoulett()
    //{
    //    _speed = 10;      // スピードを設定
    //    _countTime = 0;   // 初期化
    //}

    //public void CometWish()
    //{
    //    // ルーレットのテキストObjectをアクティブにしてルーレットを表示させる
    //    _rouletteTextObject.SetActive(true);

    //    // ルーレットが回っているかどうかのflagをtrueにする
    //    _isMoveRouletto = true;

    //    // 演出中のフラグをtrueにする
    //    GameManager.GameMng.IsDirection = true;

    //    // ルーレットを起動して彗星の願いの抽選を始める
    //    StartRoulett();

    //    // コルーチン
    //    StartCoroutine(MovingRouletto());
    //}

    ///// <summary>
    ///// 彗星の願いで増加した値をリセットする
    ///// </summary>
    ///// <param name="CometWish">引数：float型の彗星の願い</param>
    //public void ResetCometWish(float CometWish)
    //{
    //    GameManager.PlayerTurn PlayerTurn = GameManager.GameMng.GetPlayerTurn();

    //    if (CometWish == 0)
    //    {
    //        if (PlayerTurn == GameManager.PlayerTurn.Player1)
    //        {
    //            GameManager.GameMng.Player1ScoreRate = 1;       //プレイヤー１のスコアの倍率を１にする
    //        }
    //        else
    //        {
    //            GameManager.GameMng.Player2ScoreRate = 1;       //プレイヤー２のスコアの倍率を１にする
    //        }
    //    }
    //    else if (CometWish == 2)
    //    {
    //        //Debug.Log(_playerBallSize);
    //        _playerBallObject.gameObject.transform.localScale = _playerBallSize;
    //        //Debug.Log("プレイヤーボールの大きさを元に戻す");
    //    }
    //    else if (CometWish == 3 || CometWish == 4)
    //    {
    //        if (PlayerTurn == GameManager.PlayerTurn.Player1)
    //        {
    //            _gaugeControl.Player1AddGuageSpeed = 1.0f;
    //            //Debug.Log("プレイヤー1のゲージの倍率を元に戻す");
    //            //Debug.Log("プレイヤー1のゲージの倍率は" + _gaugeControl.Player1AddGuageSpeed);

    //        }
    //        else
    //        {
    //            _gaugeControl.Player2AddGuageSpeed = 1.0f;
    //            //Debug.Log("プレイヤー2のゲージの倍率を元に戻す");
    //            //Debug.Log("プレイヤー2のゲージの倍率は" + _gaugeControl.Player2AddGuageSpeed);

    //        }
    //    }

    //    // 彗星が落ちたかのFlagをfalseにする
    //    if (PlayerTurn == GameManager.PlayerTurn.Player1)
    //    {
    //        EventManager.EventMng.Player1WishFlag = false;
    //        //Debug.Log("Player1の彗星が落ちたかのFlagをFalseにする");
    //        //Debug.Log("Player1WishFlagは" + EventManager.EventMng.Player1WishFlag);

    //    }
    //    else
    //    {
    //        EventManager.EventMng.Player2WishFlag = false;
    //        //Debug.Log("Player2の彗星が落ちたかのFlagをFalseにする");
    //        //Debug.Log("Player2WishFlagは" + EventManager.EventMng.Player2WishFlag);

    //    }
    //}

    ///// <summary>
    ///// プレイヤーの彗星の願いのフラグをリセットする
    ///// </summary>
    ///// <param name="PlayerTurn"></param>
    //private void ResetPlayerWishFlag(GameManager.PlayerTurn PlayerTurn)
    //{
    //    if (PlayerTurn == GameManager.PlayerTurn.Player1)
    //    {
    //        EventManager.EventMng.Player1WishFlag = false;
    //    }
    //    else
    //    {
    //        EventManager.EventMng.Player2WishFlag = false;
    //    }
    //}

    ///// <summary>
    ///// 次のターンの得点２倍彗星の願い１の処理(動作確認済み)
    ///// </summary>
    ///// <param name="playerTurn"></param>
    //public void CometWish0(GameManager.PlayerTurn playerTurn)
    //{
    //    // 次のターンの得点２倍
    //    if (playerTurn == GameManager.PlayerTurn.Player1)
    //    {
    //        GameManager.GameMng.Player1ScoreRate = 2;       //プレイヤー１のスコアの倍率を２にする
    //    }
    //    else
    //    {
    //        GameManager.GameMng.Player2ScoreRate = 2;       //プレイヤー２のスコアの倍率を２にする
    //    }
    //    //Debug.Log("次のターンの得点に２倍");
    //}

    ///// <summary>
    ///// 自分の球が大きくなる彗星の願い３の処理
    ///// </summary>
    ///// <param name="playerTurn"></param>
    //public void CometWish2(GameManager.PlayerTurn playerTurn)
    //{
    //    // 次のターンに自分の打つ玉が大きくなる
    //    if (playerTurn == GameManager.PlayerTurn.Player1)
    //    {
    //        _playerBallObject.gameObject.transform.localScale = _playerBallSize * 1.5f;
    //        //Debug.Log("次のターンに自分の撃つ玉が大きくなる(Player1)");
    //    }
    //    else
    //    {
    //        _playerBallObject.gameObject.transform.localScale = _playerBallSize * 1.5f;
    //        //Debug.Log("次のターンに自分の撃つ玉が大きくなる(Player2)");
    //    }
    //}

    ///// <summary>
    ///// 相手のゲージを早くする彗星の願い４の処理(動作確認済み)
    ///// </summary>
    ///// <param name="playerTurn"></param>
    //public void CometWish3(GameManager.PlayerTurn playerTurn)
    //{
    //    // 相手のゲージを早くする
    //    if (playerTurn == GameManager.PlayerTurn.Player1)
    //    {
    //        _gaugeControl.Player2AddGuageSpeed += 2.0f;
    //    }
    //    else
    //    {
    //        _gaugeControl.Player1AddGuageSpeed += 2.0f;
    //    }
    //    //Debug.Log("相手のゲージを早くする");
    //}

    ///// <summary>
    ///// 自分のゲージを遅くする彗星の願い５の処理(動作確認済み)
    ///// </summary>
    ///// <param name="playerTurn"></param>
    //public void CometWish4(GameManager.PlayerTurn playerTurn)
    //{
    //    // 自分の強さゲージの速さを遅くする
    //    if (playerTurn == GameManager.PlayerTurn.Player1)
    //    {
    //        _gaugeControl.Player1AddGuageSpeed -= 0.2f;
    //    }
    //    else
    //    {
    //        _gaugeControl.Player2AddGuageSpeed -= 0.2f;
    //    }
    //    //Debug.Log("自分の強さゲージの速さを遅くする");
    //}

    ///// <summary>
    ///// ルーレットの関数
    ///// </summary>
    //public void MoveRouletto()
    //{
    //    // ルーレットのスピードの設定
    //    _countTime += Time.deltaTime * _speed;

    //    // ルーレットのループの処理
    //    if (_countTime > commandlist.Length)
    //    {
    //        _countTime = 0f;    // 最初に戻す
    //    }

    //    // ルーレットが選択しているTextのカラーの設定(選択されているTextは赤くする)
    //    if (lastTime != (int)_countTime)
    //    {
    //        foreach (var command in commandlist)
    //        {
    //            command.color = new Color(1, 1, 1);   //white
    //        }
    //        lastTime = (int)_countTime;
    //        commandlist[(int)_countTime].color = new Color(1, 0, 0);   // red
    //    }

    //    // Xを押されたらルーレットを止める
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        // ルーレットの速さを0にして止める
    //        _speed = 0;

    //        // コルーチン
    //        StartCoroutine(Rouletto());
    //    }
    //}

    ///// <summary>
    ///// ルーレットが動いているかを返す関数
    ///// </summary>
    ///// <returns>引数：ルーレットが動いているか</returns>
    //public bool GetMovingRouletto()
    //{
    //    return _isMoveRouletto;
    //}

    ///// <summary>
    /////  彗星の願いの処理(願い1,3,4,5は次のターンに作用するため)
    ///// </summary>
    //public void CometWishUpdate()
    //{
    //    // Player1の場合の処理
    //    if (EventManager.EventMng.Player1WishFlag == true)
    //    {
    //        if (EventManager.EventMng.Player1CometWishNumber == 0)
    //        {
    //            CometWish0(PlayerTurn.Player1);
    //        }
    //        else if (EventManager.EventMng.Player1CometWishNumber == 2)
    //        {
    //            CometWish2(PlayerTurn.Player1);
    //        }
    //        else if (EventManager.EventMng.Player1CometWishNumber == 3)
    //        {
    //            CometWish3(PlayerTurn.Player1);
    //        }
    //        else if (EventManager.EventMng.Player1CometWishNumber == 4)
    //        {
    //            CometWish4(PlayerTurn.Player1);
    //        }
    //        GameManager.GameMng._cometWishFlag = true;
    //    }

    //    // Player2の場合の処理
    //    else if (EventManager.EventMng.Player2WishFlag == true)
    //    {
    //        if (EventManager.EventMng.Player2CometWishNumber == 0)
    //        {
    //            CometWish0(PlayerTurn.Player2);
    //        }
    //        else if (EventManager.EventMng.Player2CometWishNumber == 2)
    //        {
    //            CometWish2(PlayerTurn.Player2);
    //        }
    //        else if (EventManager.EventMng.Player2CometWishNumber == 3)
    //        {
    //            CometWish3(PlayerTurn.Player2);
    //        }
    //        else if (EventManager.EventMng.Player2CometWishNumber == 4)
    //        {
    //            CometWish4(PlayerTurn.Player2);
    //        }
    //        GameManager.GameMng._cometWishFlag = true;
    //    }
    //}
}