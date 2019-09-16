using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickMidPanel()
    {
        ObjManager objmgr = Facade.Instance.GetManager<ObjManager>("ObjManager");
        GameObject panel = objmgr.GetPanel("firstpanel");
        panel.SetActive(false);

        GameObject obj = objmgr.CreateAndLoadObj("secondpanel", "secondpanel");
        objmgr.SetPanel(obj);
    }

    public void OnBackFrontPanel()
    {
        ObjManager objmgr = Facade.Instance.GetManager<ObjManager>("ObjManager");
        GameObject panel = objmgr.GetPanel("firstpanel");
        panel.SetActive(true);

        GameObject secondpanel = objmgr.GetPanel("secondpanel");
        secondpanel.SetActive(false);
    }
}
