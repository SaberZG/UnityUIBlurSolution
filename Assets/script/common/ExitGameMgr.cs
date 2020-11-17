using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameMgr : MonoBehaviour
{
    void Start()
    {
        Application.quitting += CollectGCManul;
    }

    void CollectGCManul()
    {
        Debug.Log("Running System.GC.Collect");
        System.GC.Collect();
    }
    void OnDestroy()
    {
        Application.quitting -= CollectGCManul;
    }
}
