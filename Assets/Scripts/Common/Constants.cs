using UnityEngine;
using System.Collections;

public class Constants{
    //版本文件
    public static readonly string VERSION_FILE = "version.txt";
    //补丁包
    public static readonly string PATCH_FILE = "patch.zip";
    public static string AssetDirname = "StreamingAssets";      //素材目录 
    public static string AppName = "mmoframework";           //应用程序名称

    public const int LIMIT_MEMORY_SIZE = 1024;
    public const int PROCESSOR_COUNT= 2;
    public static bool IS_FIRST_INIT_GAME = true;
}
