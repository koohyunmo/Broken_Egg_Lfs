using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{

    // �̱���
    private static UpdateManager s_instance = null; // ���ϼ� ����
    public static UpdateManager Instance { get { return s_instance; } } // ������ �Ŵ����� ����´�. ������Ƽ


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
