using System;

public class MenuModel
{
    public string Name { get; private set; }
    public Action OnSelect { get; private set; } // 選択されたときに実行される処理

    public MenuModel(string name, Action onSelect)
    {
        Name = name;
        OnSelect = onSelect;
    }
}

