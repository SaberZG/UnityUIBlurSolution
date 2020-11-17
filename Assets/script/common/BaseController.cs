using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public UINodeData[] ui_list_data;
    Dictionary<string, UINodeData> view_dic = new Dictionary<string, UINodeData>();
    Transform temp_parent_node;
    void Awake()
    {
        for(int i = 0; i < ui_list_data.Length; i++)
        {
            view_dic.Add(ui_list_data[i].prefab.name, ui_list_data[i]);
        }
        InitEvents();
    }
    void OnDestroy(){
        RemoveAllEvent();
    }

    protected void OpenView(string view_name, out GameObject target_go)
    {   
        Debug.Log(view_name);
        if(view_dic.ContainsKey(view_name))
        {
            temp_parent_node = PanelMgr.Instance.GetBaseViewParentNode(view_dic[view_name].uiLayer);
            target_go = GameObject.Instantiate(view_dic[view_name].prefab, temp_parent_node);
            target_go.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
        else
        {
            target_go = null;
        }
    }
    protected virtual void InitEvents(){}
    protected virtual void RemoveAllEvent(){}
}

[System.Serializable]
public class UINodeData{
    public GameObject prefab;
    public UILayer uiLayer;
}