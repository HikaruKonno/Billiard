using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class FlowLogger
{
    public static event Action<LogEntry> OnLogAdded;
    private static List<LogEntry> _logs = new List<LogEntry>();

    public static void Log(
                            [CallerFilePath] string filePath = "",
                            [CallerMemberName] string methodName = ""
                          )
    {
        // パスからファイル名を抜き取る
        var fileName = Path.GetFileName(filePath);
        var logEntry = new LogEntry
        {
            Frame = Time.frameCount,
            Time = Time.time,
        };
        
        logEntry.Info = new LogEntry.Infomation { FileName = fileName,MethodName = methodName};

        _logs.Add(logEntry);
        OnLogAdded?.Invoke(logEntry);
    }

    public static IReadOnlyList<LogEntry> GetLogs() => _logs;

    public static void Clear() => _logs.Clear();

    public class LogEntry
    {
        public struct Infomation
        {
            public string FileName;
            public string MethodName;
        };
        public int Frame;
        public float Time;
        public Infomation Info;
    }
}
