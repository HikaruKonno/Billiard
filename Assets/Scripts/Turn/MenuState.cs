using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// メニューのステート
public class MenuState : IState
{
    //-----------------------------------------------
    // メンバ変数
    //-----------------------------------------------
    #region Field
    private const float WAIT_TIME = 0.2f;           // 次の入力を受け付けるまでの待ち時間
    private readonly IViewFactory _viewFactory;     // ビューのファクトリー

    private IMenu _views;                           // メニューのビュー
    private MenuPresenter _presenter;               // メニューのプレゼンター
    private List<MenuModel> _menuModels;            // メニューのモデル
    private Action _returnPrevState;                // メニューに入る前のステートに戻る処理
    private float _timer = 0.0f;                    // タイマー

    #endregion

    //-----------------------------------------------
    // コンストラクタ
    //-----------------------------------------------
    #region Constructor

    public MenuState(IViewFactory viewFactory,Action prevState)
    {
        _viewFactory = viewFactory;
        _returnPrevState = prevState;
    }

    #endregion

    //-----------------------------------------------
    // インターフェース実装
    //-----------------------------------------------
    #region Interface

    void IState.OnBegin() 
    {
        UnityEngine.Time.timeScale = 0.0f;

        // 1. Model（メニュー項目）を定義
        _menuModels = new List<MenuModel>
        {
            new MenuModel("ゲームに戻る", CloseMenu),
                new MenuModel("リスタート", () => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1.0f;
            }),
            new MenuModel("タイトルに戻る", () => {
                SceneManager.LoadScene("Title");
                Time.timeScale = 1.0f;
            })
        };

        // 2.Viewを生成
        _views = _viewFactory.CreateView<MainMenuView>();
        // 3.Presenterを生成し、ViewとModelを渡す
        _presenter = new MenuPresenter(_views, _menuModels);
        // 4.ハイライトの初期設定
        _views.Highlight(0);
        // 5.メニューを開く
        _views.Open();
    }

    void IState.OnUpdate() 
    {
        // タイマーを加算
        _timer += UnityEngine.Time.unscaledDeltaTime;
    }

    void IState.OnEnd() 
    {
        // _views (MainMenuView)がnullでないか、またその参照先オブジェクトが破棄されていないか確認
        if (_views != null && (_views as UnityEngine.Component) != null)
        {
            // メニューを閉じる
            _views.Close();
            // 時間を動かす
            UnityEngine.Time.timeScale = 1.0f;
        }
    }

    void IState.HandleInput(InputData input) 
    {
        // メニューを閉じる
        if(input.MenuPressed)
        {
            ((IState)this).OnEnd();
            // 前のステートに戻す
            _returnPrevState.Invoke();
            return;
        }

        // 上に選択肢を移動
        if (input.UpPressed && _timer > WAIT_TIME)
        {
            _presenter.OnSelect(_views.CurrentIndex - 1);
            _timer = 0f;
        }
        // 下に選択肢を移動
        else if (input.DownPressed && _timer > WAIT_TIME)
        {
            _presenter.OnSelect(_views.CurrentIndex + 1);
            _timer = 0f;
        }

        // 決定ボタン
        if (input.DecidePressed)
        {
            _presenter.OnItemSelected(_views.CurrentIndex);
        }
    }

    #endregion

    //------------------------------------------------
    // Public関数
    //------------------------------------------------
    #region PublicFunction

    #endregion

    //-----------------------------------------------
    // Private関数
    //-----------------------------------------------
    #region PrivateFunction

    private void CloseMenu()
    {
        ((IState)this).OnEnd();
        _returnPrevState.Invoke();
    }

    #endregion
}

