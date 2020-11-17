using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestController : BaseController
{
    GameObject test_view1;
    GameObject test_view2;
    protected override void InitEvents()
    {
        EventManager.AddEvent<bool>(EventName.OPEN_TEST_VIEW1, OpenTestView1);
        EventManager.AddEvent<bool>(EventName.OPEN_TEST_VIEW2, OpenTestView2);
    }

    void OpenTestView1(bool show)
    {
        if(show)
        {
            if(test_view1 == null)
            {
                OpenView("TestView1", out test_view1);
            }
        }
        else
        {
            if(test_view1 != null)
            {
                Destroy(test_view1);
                test_view1 = null;
            }
        }
    }

    void OpenTestView2(bool show)
    {
        if(show)
        {
            if(test_view2 == null)
            {
                OpenView("TestView2", out test_view2);
            }
        }
        else
        {
            if(test_view2 != null)
            {
                Destroy(test_view2);
                test_view2 = null;
            }
        }
    }

    protected override void RemoveAllEvent()
    {
        EventManager.RemoveEvent<bool>(EventName.OPEN_TEST_VIEW1, OpenTestView1);
        EventManager.RemoveEvent<bool>(EventName.OPEN_TEST_VIEW2, OpenTestView2);
    }
}
