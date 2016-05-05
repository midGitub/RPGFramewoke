using System;
using UnityEngine;
using System.Collections.Generic;

    public class RealTimeLog : MonoBehaviour
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error,
            None,
        }
        struct LogItem
        {
            public float time;
            public string log;
            public LogLevel level;
        }

        const int MaxNum = 30;
        const float StayTime = 20;
        public static LogLevel Level = LogLevel.Error;
        GUIStyle InfoStyle;
        GUIStyle WarningStyle;
        GUIStyle ErrorStyle;
        static public int FontSize = 10;
        int Height = 10;

        // 单件子类实例
        private static RealTimeLog _instance;

        public static RealTimeLog GetInstance()
        {
            if (_instance == null)
            {
                string typeName = typeof(RealTimeLog).Name;
                GameObject go = GameObject.Find(typeName);
                if (go == null)
                {
                    go = new GameObject(typeof(RealTimeLog).Name);
                    go.AddComponent<RealTimeLog>();
                }
                _instance = go.GetComponent<RealTimeLog>();
                if (null == _instance)
                {
                    go.AddComponent<RealTimeLog>();
                }
            }
            return _instance;
        }

        void Start()
        {
            FontSize = (Screen.height - 100) / MaxNum;
            Height = FontSize + 5;
            InfoStyle = new GUIStyle();
            InfoStyle.fontSize = FontSize;
            InfoStyle.normal.textColor = Color.green;

            WarningStyle = new GUIStyle();
            WarningStyle.fontSize = FontSize;
            WarningStyle.normal.textColor = Color.yellow;

            ErrorStyle = new GUIStyle();
            ErrorStyle.fontSize = FontSize;
            ErrorStyle.normal.textColor = Color.red;
        }


        public static void Log(string log)
        {
            RealTimeLog.GetInstance().LogImpl(log, LogLevel.Info);
        }

        public static void LogWarning(string log)
        {
            RealTimeLog.GetInstance().LogImpl(log, LogLevel.Warning);
        }

        public static void LogError(string log)
        {
            RealTimeLog.GetInstance().LogImpl(log, LogLevel.Error);
        }

        private List<LogItem> AllLogs = new List<LogItem>();

        private void LogImpl(string log, LogLevel level)
        {
            if (level < Level)
            {
                return;
            }
            if (AllLogs.Count > MaxNum)
            {
                AllLogs.RemoveAt(0);
            }
            LogItem item;
            item.time = Time.realtimeSinceStartup;
            item.log = log;
            item.level = level;
            AllLogs.Add(item);
        }

        void Update()
        {
            if (AllLogs.Count > 0)
            {
                if (Time.realtimeSinceStartup - AllLogs[0].time > StayTime)
                {
                    AllLogs.RemoveAt(0);
                }
            }
        }

        void OnGUI()
        {

            for (int i = 0; i < AllLogs.Count; i++)
            {
                LogItem item = AllLogs[i];
                GUIStyle style = null;
                if (item.level == LogLevel.Warning)
                {
                    style = WarningStyle;
                }
                else if (item.level == LogLevel.Error)
                {
                    style = ErrorStyle;
                }

                string slog = string.Format("{0} {1}", item.time.ToString("0.00"), item.log);
                GUI.Label(new Rect(20, Screen.height - 80 - i * Height, Screen.width - 200, Height), slog, style);
            }
        }
    }
