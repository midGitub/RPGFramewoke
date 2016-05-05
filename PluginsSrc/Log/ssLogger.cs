using UnityEngine;

    public class ssLogger
    {
        public static bool realTimeLog = true;//控制屏幕打印
        public static RealTimeLog.LogLevel outputLeve = RealTimeLog.LogLevel.Info;//输出log的级别

        private static bool CheckEnable(RealTimeLog.LogLevel level)
        {
            if (outputLeve <= level)
            {
                return true;
            }
            return false;
        }

        public static void Log(object message, RealTimeLog.LogLevel logLevel = RealTimeLog.LogLevel.Info)
        {
            if (CheckEnable(logLevel))
            {
                Debug.Log(message);
            }
        }

        public static void Log(object message, Object context, RealTimeLog.LogLevel logLevel = RealTimeLog.LogLevel.Info)
        {
            if (CheckEnable(logLevel))
            {
                Debug.Log(message, context);
            }
        }

        public static void LogWarning(object message, RealTimeLog.LogLevel logLevel = RealTimeLog.LogLevel.Warning)
        {
            if (CheckEnable(logLevel))
            {
                Debug.LogWarning(message);
                if (realTimeLog) RealTimeLog.LogWarning(message.ToString());
            }
        }

        public static void LogWarning(object message, Object context, RealTimeLog.LogLevel logLevel = RealTimeLog.LogLevel.Warning)
        {
            if (CheckEnable(logLevel))
            {
                Debug.LogWarning(message, context);
                if (realTimeLog) RealTimeLog.LogWarning(message.ToString());
            }
        }

        public static void LogError(object message, RealTimeLog.LogLevel logLevel = RealTimeLog.LogLevel.Error)
        {
            if (CheckEnable(logLevel))
            {
                Debug.LogError(message);
                if (realTimeLog) RealTimeLog.LogError(message.ToString());
            }
        }

        public static void LogError(object message, Object context, RealTimeLog.LogLevel logLevel = RealTimeLog.LogLevel.Error)
        {
            if (CheckEnable(logLevel))
            {
                Debug.LogError(message, context);
                if (realTimeLog) RealTimeLog.LogError(message.ToString());
            }
        }
    }
