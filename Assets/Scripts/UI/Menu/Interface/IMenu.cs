using System;
using System.Collections.Generic;

public interface IMenu
{
    /// <summary>
    /// 選択項目が確定したときに呼ばれる
    /// </summary>
    event Action<int> OnItemSelected;

    /// <summary>
    /// 選択ハイライトを動かす
    /// </summary>
    void Highlight(int index);

    /// <summary>
    /// 現在選択中のインデックス
    /// </summary>
    int CurrentIndex { get; }

    /// <summary>
    /// メニューを開く
    /// </summary>
    void Open();

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    void Close();
}

