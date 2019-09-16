using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
        ObjManager objmgr = Facade.Instance.GetManager<ObjManager>("ObjManager");
        GameObject obj = objmgr.CreateAndLoadObj("firstpanel", "firstpanel");
        objmgr.SetPanel(obj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        Facade.Instance.AddManager<ObjManager>("ObjManager");
        Facade.Instance.AddManager<ResourceManager>("ResourceManager");
    }
}
