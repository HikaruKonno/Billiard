using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// 自作のデバッグクラス
// ほしい機能が増えたら随時追加していく。
public static class MyDebug
{
    private static Dictionary<string, int> frameCounter = new Dictionary<string, int>();
    private static Dictionary<string, float> timeCounter = new Dictionary<string, float>();

    // UEのようにアサーションが起こったらゲームを止めたいため、自分で作った.
    /// <summary>
    /// アサーションが起きたらゲームを一時停止させる
    /// </summary>
    /// <param name="judgement">間違っていたら止める</param>
    public static void Assert(bool judgement)
    {
        if (!judgement)
        {
            // 呼び出し元の情報を取得
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(1); // 1は1つ上の呼び出し元を指す

            string filePath = frame.GetFileName();
            string fileName = Path.GetFileName(filePath);
            string methodName = frame.GetMethod().Name;
            int lineNumber = frame.GetFileLineNumber();

            // ログの内容出力
            UnityEngine.Debug.LogError($"ファイル名:[ {fileName} ] \n" +
                                       $"{lineNumber} 行目\n" +
                                       $"呼び出された関数 {methodName} \n" +
                                       $"アサーションが起こりました。");

#if UNITY_EDITOR
            // アプリを一時停止する
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }
    }

    /// <summary>
    /// 指定した時間後にtrueを返す
    /// Updateで呼ばれていること前提
    /// </summary>
    /// <param name="frame">フレーム</param>
    /// <returns>指定した時間経過したらtrue</returns>
    public static bool Count(
                                    int frame,
                                    [CallerFilePath] string file = "",
                                    [CallerLineNumber] int line = 0,
                                    [CallerMemberName] string member = ""
                                 )
    {
        string key = $"{file}:{line}:{member}";

        // keyに登録されていなければ登録し、初期化
        if (!frameCounter.ContainsKey(key))
        {
            frameCounter[key] = 0;
        }

        frameCounter[key]++;
        if( frameCounter[key] >= frame )
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 指定した時間後にtrueを返す
    /// Updateで呼ばれていること前提
    /// </summary>
    /// <param name="time">時間(秒)</param>
    /// <returns>指定した時間経過したらtrue</returns>
    public static bool DebugCount(
                                    float time,
                                    [CallerFilePath] string file = "",
                                    [CallerLineNumber] int line = 0,
                                    [CallerMemberName] string member = ""
                                 )
    {
        string key = $"{file}:{line}:{member}";

        // keyに登録されていなければ登録し、初期化
        if (!timeCounter.ContainsKey(key))
        {
            timeCounter[key] = 0f;
        }

        timeCounter[key] += Time.deltaTime;
        if (timeCounter[key] >= time)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 呼び出し履歴を記録する
    /// </summary>
    /// <param name="filePath">呼び出し元のファイルパス</param>
    /// <param name="methodName">呼び出し元の関数名</param>
    public static void CallRecord
        (
            [CallerFilePath] string filePath = ""
           ,[CallerMemberName] string methodName = ""
        )
    {
        FlowLogger.Log(filePath,methodName);
    }
}

