using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
using System;

public class BuildProject : Editor
{
    #region Properties
    static string[] SCENES = FindEnableEditorScenes();
    static bool ExportingAndroidProject = false;
    static string interProjectName = "Galaxy";
    static string appName = "Galaxy";
    static string bundleName = "com.wangying.test";

    //android 相关
    static string bundleVersion = "1";
    static int bundleVersionCode = 1;
    static string keystorePass = "wangying123";
    static string keyaliasPass = "wangying123";
    #endregion

    #region Build
    /// <summary>
    /// 生成目标工程
    /// </summary>
    /// <param name="scenes"></param>
    /// <param name="targetDir"></param>
    /// <param name="buildTarget"></param>
    /// <param name="buildOptions"></param>
    static void GenericBuild(string[] scenes, string targetDir, BuildTarget buildTarget, BuildOptions buildOptions)
    {
        AssetDatabase.Refresh();
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
        string result = BuildPipeline.BuildPlayer(scenes, targetDir, buildTarget, buildOptions);
        if (result.Length != 0)
        {
            GLog.LogError("Build failed:" + result);
        }
    }

    [PostProcessBuild(1000)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)//old is iPhone
        {
            string iconsDir = Application.dataPath + "/Icons/Ios";
            string targetDir = pathToBuiltProject + "/Unity-iPhone/Images.xcassets/AppIcon.appiconset/";

            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }
            Directory.CreateDirectory(targetDir);

            DirectoryInfo folder = new DirectoryInfo(iconsDir);
            foreach (FileInfo file in folder.GetFiles())
            {
                if (file.Extension.Equals(".json") || file.Extension.Equals(".png"))
                {
                    string filename = targetDir + file.Name;
                    file.CopyTo(filename, true);
                }
            }
        }

        if (target == BuildTarget.Android && ExportingAndroidProject)
        {
            string sourceDir = Application.dataPath.Replace("/Assets", "/Platform/Plugins/Android/") + ProjectName;
            string destDir = Path.Combine(pathToBuiltProject, PlayerSettings.productName);

            File.Copy(Path.Combine(sourceDir, "AndroidManifest.xml"), Path.Combine(destDir, "AndroidManifest.xml"), true);
            File.Copy(Path.Combine(sourceDir, "build.xml"), Path.Combine(destDir, "build.xml"), true);
            File.Copy(Path.Combine(sourceDir, "ant.properties"), Path.Combine(destDir, "ant.properties"), true);
            File.Copy(Path.Combine(sourceDir, "project.properties"), Path.Combine(destDir, "project.properties"), true);
            CopyDirectory(Path.Combine(sourceDir, "src"), Path.Combine(destDir, "src"));

            sourceDir = Application.dataPath.Replace("/Assets", "/Platform/Plugins/Android/Common");
            CopyDirectory(Path.Combine(sourceDir, "src"), Path.Combine(destDir, "src"));

            //StreamWriter saveFile = File.CreateText(Path.Combine(destDir, "version.properties"));
            //saveFile.Write(string.Format("version={0}\nversioncode={1}\npackageid={2}\n",
            //Updater.version.GetVersion(), Updater.version.GetVersionCode(), PlayerSettings.bundleIdentifier));
            //saveFile.Close();

            DirectoryInfo dirInfo = new DirectoryInfo(destDir);
            dirInfo.MoveTo(Path.Combine(pathToBuiltProject, AppName));
        }
    }

    static void BuildAndroid(int version, string appName, string projectName, string defineSymbols, string bundleId, string id)
    {
        ExportingAndroidProject = true;
        //BuildTargetGroup targetGroup = BuildTargetGroup.Android;
        BuildTarget buildTarget = BuildTarget.Android;

        //string applicationPath = Application.dataPath.Replace("/Assets","");
        string targetDir = Application.dataPath.Replace("/Assets", "") + "/Build/";
        string targetName = targetDir + appName + ".apk";
        //copy android plugins
        string pluginsDir = Application.dataPath + "/Plugins/Android";
        if (Directory.Exists(pluginsDir))
            Directory.Delete(pluginsDir, true);
        string sourceDir = Application.dataPath.Replace("/Assets", "/Platform/Plugins/Android/") + projectName;
        CopyDirectory(sourceDir, pluginsDir);
        //copy common plugins
        sourceDir = Application.dataPath.Replace("/Assets", "/Platform/Plugins/Android/Common");
        if(File.Exists(sourceDir))
            CopyDirectory(sourceDir, pluginsDir);
        if (Directory.Exists(targetDir))
        {
            if (File.Exists(targetName))
            {
                File.Delete(targetName);
            }
        }
        else
        {
            Directory.CreateDirectory(targetDir);
        }

        /*if (version <= 0)
            version = Updater.version.GetVersionCode();
        else
            Updater.version.SetVersionCode(version);
        WriteVersion(version, id, VendorId);*/

        PlayerSettings.bundleIdentifier = bundleId;
        PlayerSettings.bundleVersion = bundleVersion; //Updater.version.GetVersion();
        PlayerSettings.Android.bundleVersionCode = bundleVersionCode; //Updater.version.GetVersionCode();
        PlayerSettings.keystorePass = keystorePass;
        PlayerSettings.keyaliasPass = keyaliasPass;
        //这里是添加自定义symbols的
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defineSymbols);
        //ExportLuas();
        GenericBuild(SCENES, targetName, buildTarget, BuildOptions.None);
        GLog.Log("build android package success!");
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, string.Empty);
    }

    static void BuildIOS(int version, string[] mods, string defineSymbols, string bundleId, string id)
    {
        BuildTarget buildTarget = BuildTarget.iOS;
        string targetDir = Application.dataPath.Replace("/Assets", "") + "/Build/";
        string targetName = targetDir + AppName;

        if (Directory.Exists(targetDir))
        {
            if (Directory.Exists(targetName))
            {
                Directory.Delete(targetName, true);
            }
        }
        else
        {
            Directory.CreateDirectory(targetDir);
        }

        /*if (version <= 0)
            version = Updater.version.GetVersionCode();
        else
            Updater.version.SetVersionCode(version);
        WriteVersion(version, id, VendorId);*/

        //XCodePostProcess.platformMods = mods;

        PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTargetGroup.iOS);
        //PlayerSettings.SetPropertyInt("Architecture", (int)iPhoneArchitecture.Universal, BuildTargetGroup.iOS);

        PlayerSettings.bundleIdentifier = bundleId;
        PlayerSettings.bundleVersion = bundleVersionCode.ToString(); //Updater.version.GetVersionCode().ToString();
        //PlayerSettings.shortBundleVersion = Updater.version.GetVersion();
        PlayerSettings.iPhoneBundleIdentifier = bundleVersion;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defineSymbols);
        //ExportLuas();
        GenericBuild(SCENES, targetName, buildTarget, BuildOptions.SymlinkLibraries);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, String.Empty);
    }
    #endregion

    #region IOS
    static void PublishIOS()
    {
        BuildIOS(VersionCode, new string[] {"",""}/*PlatformMods*/, DefineSymbols, BundleId, Identifier);
    }

    static void PublishIosForce() {
#if UNITY_IPHONE
		PublishIOS ();
#else
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
        PublishIOS();
#endif
    }
    #endregion

    #region Android 
#if UNITY_ANDROID
    [MenuItem("[Build Package]/Build Android")]
#endif
    static void BuildPackageForAndroid()
    {
        GLog.Log("package android start...");
        BuildAndroid(0, appName, interProjectName, string.Empty, bundleName, "version");
    }

    [MenuItem("sss/ss")]
    static void Test() {
        GLog.Log("test.......");
    }

    static void PublishAndroidForce()
    {
#if UNITY_ANDROID
        PublishAndroidProject();
#endif
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        PublishAndroidProject();
    }

    static void PublishAndroidProject() {
        ExportAndroidProject(VersionCode, AppName, ProjectName, DefineSymbols, BundleId, Identifier);
    }

    static void ExportAndroidProject(int version, string appName, string projectName, string defineSymbols, string bundleId, string id)
    {
        ExportingAndroidProject = true;
        string targetDir = Application.dataPath.Replace("/Assets", "") + "/Build/";
        string targetName = targetDir + appName;
        //copy android plugins
        string pluginsDir = Application.dataPath + "/Plugins/Android";
        if (Directory.Exists(pluginsDir))
            Directory.Delete(pluginsDir, true);
        Directory.CreateDirectory(pluginsDir);
        string sourceDir = Application.dataPath.Replace("/Assets", "/Platform/Plugins/Android/") + projectName;
        CopyDirectory(Path.Combine(sourceDir, "libs"), Path.Combine(pluginsDir, "libs"));
        CopyDirectory(Path.Combine(sourceDir, "assets"), Path.Combine(pluginsDir, "assets"));
        //copy common plugins
        sourceDir = Application.dataPath.Replace("/Assets", "/Platform/Plugins/Android/Common");
        CopyDirectory(Path.Combine(sourceDir, "libs"), Path.Combine(pluginsDir, "libs"));
        CopyDirectory(Path.Combine(sourceDir, "assets"), Path.Combine(pluginsDir, "assets"));

        if (!Directory.Exists(targetDir))
            Directory.CreateDirectory(targetDir);
        if (Directory.Exists(targetName))
            Directory.Delete(targetName, true);
        //if (version <= 0)
        //    version = Updater.version.GetVersionCode();
        //else
        //    Updater.version.SetVersionCode(version);

        //WriteVersion(version, id, VendorId);
        PlayerSettings.bundleIdentifier = bundleId;
        PlayerSettings.bundleVersion = bundleVersion;//Updater.version.GetVersion();
        PlayerSettings.Android.bundleVersionCode = bundleVersionCode; //Updater.version.GetVersionCode();
        PlayerSettings.keystorePass = keystorePass;
        PlayerSettings.keyaliasPass = keyaliasPass;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defineSymbols);
        //ExportLuas();
        GenericBuild(SCENES, targetDir, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, string.Empty);
    }
    #endregion

    #region Tools

    /// <summary>
    /// 获取当前工程所有场景文件
    /// </summary>
    private static string[] FindEnableEditorScenes()
    {
        List<string> scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            scenes.Add(scene.path);
        }
        return scenes.ToArray();
    }

    /// <summary>
    /// 将源文件夹复制到目标路径
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="destinationPath"></param>
    static void CopyDirectory(string sourcePath, string destinationPath)
    {
        if (!Directory.Exists(sourcePath)) return;

        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            if (fsi is System.IO.FileInfo)
            {
                if (!fsi.Extension.Equals(".meta"))
                    File.Copy(fsi.FullName, destName, true);
            }
            else
            {
                Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }


    /// <summary>
    /// 得到项目的名字project:后面
    /// </summary>
    public static string ProjectName
    {
        get
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("project"))
                {
                    return arg.Split(":"[0])[1];
                }
            }
            return interProjectName;//默认项目名
        }
        set { interProjectName = value; }
    }

    /// <summary>
    /// 返回version:后面的数字，默认返回0
    /// </summary>
	public static int VersionCode
    {
        get
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("version"))
                {
                    return int.Parse(arg.Split(":"[0])[1]);
                }
            }
            return 0;
        }
    }

    /// <summary>
    /// 返回mods:后面的字符串数组
    /// </summary>
    /*public static string[] PlatformMods
    {
        get
        {
            string jsonData = "[]";
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("mods"))
                {
                    jsonData = arg.Split(":"[0])[1];
                    break;
                }
            }
            return LitJson.JsonMapper.ToObject<string[]>(jsonData);
        }
    }*/

    /// <summary>
    /// 得到app的名字app:后面，默认返回"galaxy"
    /// </summary>
    public static string AppName
    {
        get
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("app"))
                {
                    return arg.Split(":"[0])[1];
                }
            }
            return appName;
        }
    }

    /// <summary>
    /// 返回define:后面的字符串，默认返回""
    /// </summary>
	public static string DefineSymbols
    {
        get
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("define"))
                {
                    return arg.Split(":"[0])[1];
                }
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// 得到bundle的名字bundle:后面
    /// </summary>
	public static string BundleId
    {
        get
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("bundle"))
                {
                    return arg.Split(":"[0])[1];
                }
            }
            return bundleName;
        }
    }

    /// <summary>
    /// 得到identifier，id:后面，默认返回"version"
    /// </summary>
	public static string Identifier
    {
        get
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("id"))
                {
                    return arg.Split(":"[0])[1];
                }
            }
            return "version";
        }
    }

    /// <summary>
    /// 得到vendorid，vendor:后面，默认返回0
    /// </summary>
	public static int VendorId
    {
        get
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("vendor"))
                {
                    return int.Parse(arg.Split(":"[0])[1]);
                }
            }
            return 0;
        }
    }
#endregion

}
