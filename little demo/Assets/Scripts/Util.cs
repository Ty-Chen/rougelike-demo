using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Util
{ 
    public static int Int(object o)
    {
        return Convert.ToInt32(o);
    }

    public static float Float(object o)
    {
        return (float)Math.Round(Convert.ToSingle(o), 2);
    }

    public static long Long(object o)
    {
        return Convert.ToInt64(o);
    }

    public static string Uid(string uid)
    {
        int position = uid.LastIndexOf('_');
        return uid.Remove(0, position + 1);
    }

    public static long GetTime()
    {
        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }

    public static T Get<T> (GameObject go, string subnode) where T : Component
    {
        if ( go != null)
        {
            Transform sub = go.transform.Find(subnode);
            if (sub != null)
            {
                return sub.GetComponent<T>();
            }
        }
        return null;
    }

    public static T Get<T>(Component go, string subnode) where T : Component
    {
        return go.transform.Find(subnode).GetComponent<T>();
    }

    public static T Add<T>(GameObject go) where T : Component
    {
        if (go != null)
        {
            T[] ts = go.GetComponents<T>();
            for (int i = 0; i < ts.Length; i++)
            {
                if (ts[i] != null)
                {
                    GameObject.Destroy(ts[i]);
                }

            }
            return go.gameObject.AddComponent<T>();
        }
        return null;
    }

    public static T Add<T>(Transform go) where T : Component
    {
        return Add<T>(go.gameObject);
    }

    public static GameObject Child(GameObject go, string subnode)
    {
        return Child(go.transform, subnode);
    }

    public static GameObject Child(Transform go, string subnode)
    {
        Transform trans = go.Find(subnode);
        if (trans == null)
        {
            return null;
        }
        return trans.gameObject;
    }

    public static GameObject Peer(GameObject go, string subnode)
    {
        return Peer(go.transform, subnode);
    }

    public static GameObject Peer(Transform go, string subnode)
    {
        Transform trans = go.parent.Find(subnode);
        if (trans == null)
        {
            return null;
        }
        return trans.gameObject;
    }

    public static string Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }

    public static string Decode(string message)
    {
        byte[] bytes = Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }

    public static bool isNumeric(string str)
    {
        if (str == null || str.Length == 0)
        {
            return false;
        }
        for (int i = 0; i < str.Length; i++)
        {
            if (!Char.IsNumber(str[i]))
                return false;
        }
        return true;
    }

    public static string HashToMD5Hex(string sourceStr)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] result = md5.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                builder.Append(result[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static string md5(string src)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(src);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string dst = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            dst += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        dst = dst.PadLeft(32, '0');
        return dst;
    }

    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retval = md5.ComputeHash(fs);
            fs.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retval.Length; i++)
            {
                sb.Append(retval[i].ToString("x2"));
            }

            return sb.ToString();
        }
        catch(Exception ex)
        {
            throw new Exception("md5file() fail, error " + ex.Message);
        }
    }

    public static string GetKey(string key)
    {
        return AppConst.AppPrefix + "_" + key;
    }

    public static int GetInt(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.GetInt(name);
    }

    public static bool HasKey(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.HasKey(name);
    }


    public static void SetInt(string key, int val)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetInt(name, val);
    }

    public static string GetString(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.GetString(name);
    }

    public static void SetString(string key, string val)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetString(name, val);
    }

    public static void RemoveData(string key)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
    }

    public static void ClearMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    public static bool IsNumber(string strNumber)
    {
        Regex regex = new Regex("[^0-9]");
        return !regex.IsMatch(strNumber);
    }

    public static string DataPath
    {
        get
        {
            string game = AppConst.AppName.ToLower();
            if (Application.isMobilePlatform)
                return Application.persistentDataPath + "/" + game + "/";
            else
                return Application.dataPath + "/" + AppConst.AssetDirname + "/";
        }
    }

    public static bool NetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }

    public static string AppContentPath()
    {
        string path = string.Empty;
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;

            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;

            default:
                path = Application.dataPath + "/" + AppConst.AssetDirname + "/";
                break;
        }
        return path;
    }

    public static void Log(string str)
    {
        Debug.Log(str);
    }

    public static void LogWarning(string str)
    {
        Debug.LogWarning(str);
    }

    public static void LogError(string str)
    {
        Debug.LogError(str);
    }
}
