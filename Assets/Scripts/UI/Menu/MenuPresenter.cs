using System;
using System.Collections;
using System.Collections.Generic;

public class MenuPresenter
{
    //----------------------------------------------------------------------------------
    // メンバー変数
    //----------------------------------------------------------------------------------
    #region Field

    private readonly IMenu _view;
    private readonly List<MenuModel> _menuModel;

    #endregion

    //----------------------------------------------------------------------------------
    // コンストラクタ
    //----------------------------------------------------------------------------------
    #region Constructor

    public MenuPresenter(IMenu view, List<MenuModel> menuModel)
    {
        _view = view;
        _menuModel = menuModel;

        // ビューに
        // View が発行する汎用イベントだけを購読
        _view.OnItemSelected += OnItemSelected;
    }

    #endregion

    //----------------------------------------------------------------------------------
    // Public関数
    //----------------------------------------------------------------------------------
    #region PublicFunction
    public void OnSelect(int index)
    {
        _view.Highlight(index);
    }

    /// <summary>
    /// 現在選択されている項目の処理の呼び出し
    /// </summary>
    /// <param name="index">選択されている番号</param>
    public void OnItemSelected(int index)
    {
        if (index >= 0 && index < _menuModel.Count)
        {
            _menuModel[index].OnSelect?.Invoke();
        }
    }

    /// <summary>
    /// ビューとモデルの連携を解除する
    /// </summary>
    public void Dispose()
    {
        _view.OnItemSelected -= OnItemSelected;

    }

    #endregion
}
