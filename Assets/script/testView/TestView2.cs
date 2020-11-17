using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestView2 : BaseView
{
    new void Awake()
    {
        need_blur_bg = true;
        blur_type = BlurType.Normal;
        base.Awake();
    }

    GameObject close_btn_obj;
    void Start()
    {
        close_btn_obj = transform.Find("close_btn").gameObject;
        close_btn_obj.GetComponent<Button>().onClick.AddListener(OnCloseBtnClick);
    }
    public void OnCloseBtnClick()
    {
        EventManager.DispatchEvent<bool>(EventName.OPEN_TEST_VIEW2, false);
    }
}
