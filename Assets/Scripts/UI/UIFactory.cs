using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFactory : MonoBehaviour,IViewFactory
{
    [SerializeField] private MainMenuView mainMenuView;

    public T CreateView<T>() where T : IMenu
    {
        if (typeof(T) == typeof(MainMenuView))
        {
            return (T)(object)mainMenuView;
        }
        throw new ArgumentException($"No prefab registered for {typeof(T)}");
    }
}
