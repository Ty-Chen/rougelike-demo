using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjManager : MonoBehaviour
{

    Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();

    public GameObject GetPanel(string name)
    {
        if (objDic.ContainsKey(name))
        {
            return objDic[name];
        }
        else
        {
            return null;
        }
    }

    public void SetPanel(GameObject obj)
    {
        GameObject canvas = GameObject.Find("Canvas");
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.parent = canvas.transform;
        rect.localScale = Vector3.one;
        rect.localPosition = Vector3.zero;
        rect.anchorMax = Vector2.one;
        rect.anchorMin = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }

    public static UnityEngine.Object getMainAsset(AssetBundle assetBundle)
    {
        return assetBundle.LoadAsset(assetBundle.GetAllAssetNames()[0]);
    }

    public GameObject CreateAndLoadObj(string bundleName, string objName = null)
    {
        ResourceManager resMgr = Facade.Instance.GetManager<ResourceManager>("ResourceManager");
        AssetBundle bundle = resMgr.LoadBundle(bundleName);
        GameObject prefab = null;
        if (bundle == null)
        {
            return null;
        }
        if (objName != null)
        {
            prefab = bundle.LoadAsset(objName, typeof(GameObject)) as GameObject;
        }
        else
        {
            prefab = getMainAsset(bundle) as GameObject;
        }

        GameObject go = Instantiate(prefab) as GameObject;
        go.name = bundleName;
        bundle.Unload(false);
        if (!abDic.ContainsKey(bundleName))
            abDic.Add(bundleName, bundle);
        if (!objDic.ContainsKey(bundleName))
            objDic.Add(bundleName, go);

        return go;
    }

    public void UnloadALLAB()
    {
        foreach(AssetBundle ab in abDic.Values)
        {
            if (ab)
                ab.Unload(false);
        }
    }

    public Material CreateAndLoadMat(string bundleName, string objName = null)
    {
        ResourceManager resMgr = Facade.Instance.GetManager<ResourceManager>("ResourceManager");
        AssetBundle bundle = resMgr.LoadBundle(bundleName);
        Material prefab = null;
        if (bundle == null)
        {
            return null;
        }
        if (objName != null)
        {
            prefab = bundle.LoadAsset(objName, typeof(Material)) as Material;
        }
        else
        {
            prefab = getMainAsset(bundle) as Material;
        }

        Material go = Instantiate(prefab) as Material;
        bundle.Unload(false);
        if (!abDic.ContainsKey(bundleName))
            abDic.Add(bundleName, bundle);
        return go;
    }

    public Sprite CreateAndLoadSprite(string bundleName, string objName = null)
    {
        ResourceManager resMgr = Facade.Instance.GetManager<ResourceManager>("ResourceManager");
        AssetBundle bundle = resMgr.LoadBundle(bundleName);
        Sprite prefab = null;
        if (bundle == null)
        {
            return null;
        }
        if (objName != null)
        {
            prefab = bundle.LoadAsset(objName, typeof(Sprite)) as Sprite;
        }
        else
        {
            prefab = getMainAsset(bundle) as Sprite;
        }

        Sprite go = Instantiate(prefab) as Sprite;
        bundle.Unload(false);
        if (!abDic.ContainsKey(bundleName))
            abDic.Add(bundleName, bundle);
        return go;
    }

    public void AddSpriteTexture(GameObject obj, string bundleName, string objName = null)
    {
        Sprite sprite = CreateAndLoadSprite(bundleName, objName);
        obj.GetComponent<Image>().sprite = sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
