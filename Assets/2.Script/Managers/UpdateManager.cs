using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{

    // 싱글톤
    private static UpdateManager s_instance = null; // 유일성 보장
    public static UpdateManager Instance { get { return s_instance; } } // 유일한 매니저를 갖고온다. 프로퍼티


    public Action customUpdate { get; private set; }


    public void PushAction(Action act)
    {
        customUpdate -= act;
        customUpdate += act;
    }

    public void PopAction(Action act)
    {
        customUpdate -= act;
    }

    private void Update()
    {
        if(customUpdate != null)
            customUpdate();
    }

}
