using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//-------------------------------------------------------------
// メインメニューの見た目の機能のクラス
//-------------------------------------------------------------
public class MainMenuView : MonoBehaviour,IMenu
{
    //------------------------------------------------------------------------------------
    // メンバー変数
    //------------------------------------------------------------------------------------
    #region Field
    [SerializeField] private Button[] buttons;   // Inspector で並び順にセット
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    private int _currentIndex = 0;
    #endregion

    //------------------------------------------------------------------------------------
    // デリゲート
    //------------------------------------------------------------------------------------
    #region Delegate

    // Presenterが購読するためのイベント
    public event Action<int> OnItemSelected;

    #endregion

    //------------------------------------------------------------------------------------
    // インターフェース実装
    //------------------------------------------------------------------------------------
    #region Interface

    /// <summary>
    /// 現在選択中のボタンのインデックスを取得
    /// </summary>
    int IMenu.CurrentIndex => _currentIndex;

    /// <summary>
    /// 現在選択中インデックスをセットし、見た目を更新
    /// </summary>
    void IMenu.Highlight(int index)
    {
        if (buttons.Length == 0)
        {
            return;
        }

        // 範囲チェック＋ラップ
        if (index < 0)
        {
            index = buttons.Length - 1;
        }
        if (index >= buttons.Length)
        {
            index = 0;
        }

        // 前のボタンを戻す
        buttons[_currentIndex].GetComponent<Image>().color = normalColor;

        // 新しいボタンをハイライト
        _currentIndex = index;
        buttons[_currentIndex].GetComponent<Image>().color = selectedColor;
    }

    /// <summary>
    /// メニューを開く
    /// </summary>
    void IMenu.Open()
    {
        // このUIをアクティブにする
        gameObject.SetActive(true);
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    void IMenu.Close()
    {
        // このUIを非アクティブにする
        gameObject.SetActive(false);
    }

    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction

    /// <summary>
    /// 項目が決定されたことを通知するメソッド
    /// </summary>
    public void SelectItem()
    {
        OnItemSelected?.Invoke(_currentIndex);
    }

    #endregion
}
