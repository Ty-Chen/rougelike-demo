using System;
using System.Collections.Generic;
using UnityEngine;

public class Facade 
{
    static GameObject m_GameManager;
    static Dictionary<string, object> m_Managers = new Dictionary<string, object>();

    private static Facade _instance;
    private ObjManager m_objMgr;
    private ResourceManager m_ResMgr;

    GameObject AppGameManager
    {
        get
        {
            if (m_GameManager == null)
                m_GameManager = GameObject.Find("GameManager");
            return m_GameManager;
        }
    }

    public static Facade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Facade();
            }
            return _instance;
        }
    }

    public void AddManager(string typeName, object obj)
    {
        if (!m_Managers.ContainsKey(typeName))
        {
            m_Managers.Add(typeName, obj);
        }
    }

    public T AddManager<T> (string typeName) where T : Component
    {
        object result = null;
        m_Managers.TryGetValue(typeName, out result);
        if (null != result)
            return (T)result;

        Component c = AppGameManager.AddComponent<T>();
        m_Managers.Add(typeName, c);
        return default(T);
    }

    public T GetManager<T>(string typeName) where T : class
    {
        if (!m_Managers.ContainsKey(typeName))
            return default(T);

        object manager = null;

        m_Managers.TryGetValue(typeName, out manager);
        return (T)manager;
    }

    public void RemoveManager(string typeName)
    {
        if (!m_Managers.ContainsKey(typeName))
            return;

        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        Type type = manager.GetType();
        if (type.IsSubclassOf(typeof(MonoBehaviour)))
            UnityEngine.Object.Destroy((Component)manager);

        m_Managers.Remove(typeName);
    }

    public ResourceManager ResManager
    {
        get
        {
            if (m_ResMgr == null)
            {
                m_ResMgr = GetManager<ResourceManager>("ResourceManager");
            }
            return m_ResMgr;
        }
    }

    public ObjManager objManager
    {
        get
        {
            if (m_objMgr == null)
            {
                m_objMgr = GetManager<ObjManager>("ObjManager");
            }
            return m_objMgr;
        }
    }


}
