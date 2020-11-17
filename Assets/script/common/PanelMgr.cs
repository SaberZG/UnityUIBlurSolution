using UnityEngine;

public enum UILayer
{
    UI = 1,
    Top = 2,
}
public class PanelMgr : MonoBehaviour
{
    private static PanelMgr _instance;
    public static PanelMgr Instance{
        get{
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PanelMgr)) as PanelMgr;
            }
            return _instance;
        }
    }

    private BaseView temp_baseview;
    private Transform UI;
    private Transform Top;
    void Awake()
    {
        UI = GameObject.Find("Canvas/UI").transform;
        Top = GameObject.Find("Canvas/Top").transform;
    }

    public Transform GetBaseViewParentNode(UILayer ui_layer)
    {
        switch(ui_layer)
        {
            case UILayer.UI:
                return UI;
            case UILayer.Top:
                return Top;
        }
        // 默认返回UI层
        return UI;
    }
}
