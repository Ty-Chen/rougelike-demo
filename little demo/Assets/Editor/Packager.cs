using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Diagnostics;

public class Packager 
{
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    [MenuItem("Game/Build iPhone Resource", false, 1)]
    public static void BuildPhoneResource()
    {
        BuildTarget target;
        target = BuildTarget.iOS;
        BuildAssetResource(target, false);
    }

    [MenuItem("Game/Build Android Resource", false, 1)]
    public static void BuildAndroidResource()
    { 
        BuildAssetResource(BuildTarget.Android, true);
    }

    [MenuItem("Game/Build windows Resource", false, 1)]
    public static void BuildWindowsResource()
    {
        BuildAssetResource(BuildTarget.StandaloneWindows64, true);
    }

    public static void BuildAssetResource(BuildTarget target, bool isWwin)
    {
        string dataPath = Util.DataPath;
        if (Directory.Exists(dataPath))
        {
            Directory.Delete(dataPath, true);
        }
        string assetfile = string.Empty;

        string resPath = AppDataPath + "/" + AppConst.AssetDirname + "/";

        if (!Directory.Exists(resPath))
        {
            Directory.CreateDirectory(resPath);
        }

        BuildPipeline.BuildAssetBundles(resPath, BuildAssetBundleOptions.None, target);

        AssetDatabase.Refresh();
    }

    static string AppDataPath
    {
        get
        {
            return Application.dataPath.ToLower();
        }
    }

    static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach(string filename in names)
        {
            string ext = System.IO.Path.GetExtension(filename);
            if (ext.Equals(".meta"))
                continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach(string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }

    static void UpdateProgress(int progress, int progressMax, string desc)
    {
        string title = "Process..[" + progress + "-" + progressMax + "]";
        float value = (float)progress / (float)progressMax;
        EditorUtility.DisplayProgressBar(title, desc, value);
    }
}
