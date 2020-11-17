using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TestBtn1 : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.DispatchEvent<bool>(EventName.OPEN_TEST_VIEW1, true);
    }
}
