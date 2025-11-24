using System;
using System.Collections.Generic;

/// <summary>
/// ターンステート切り替え専用のバス
/// </summary>
public static class StateChangeBus
{
    // 型（イベントの種類）ごとにコールバック（リスナー）を登録する辞書
    private static Dictionary<Type, Action<object>> _events = new Dictionary<Type, Action<object>>();

    /// <summary>
    /// 指定した型のイベントを登録（ステートを切り替える関数を登録）
    /// </summary>
    public static void Subscribe<T>(Action<T> callback) where T :ChangeStateRequest
    {
        // Keyが重複していなければイベントを登録
        if (!_events.ContainsKey(typeof(T)))
        {
            // T型のイベントに対して、objectからTにキャストしてから呼び出すようにラップ
            Action<object> wrapper = obj => callback((T)obj);
            // Keyが存在しなかったらリスナーを登録する
            _events.Add(typeof(T),wrapper);
        }
    }

    /// <summary>
    /// 指定した型のイベントを解除する
    /// </summary>
    public static void UnSubscribe<T>(Action<T> callback) where T:ChangeStateRequest
    {
        if(_events.ContainsKey(typeof(T)))
        {
            // T型のイベントに対して、objectからTにキャストしてから呼び出すようにラップ
            Action<object> wrapper = obj => callback((T)obj);
            // Keyが存在しなかったらリスナーを解除する
            _events.Remove(typeof(T),out wrapper);
        }
    }

    /// <summary>
    /// 指定した型のイベントを発行（ステートを切り替える）
    /// </summary>
    public static void Publish<T>(T message) where T :ChangeStateRequest
    {
        if (_events.ContainsKey(typeof(T)))
        {
            _events[typeof(T)]?.Invoke(message);
        }
        else
        {
            UnityEngine.Debug.LogError("イベントが登録されていません。");
        }
    }
}

