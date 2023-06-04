using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension 
{

    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return CUtil.GetOrAddComponent<T>(go);
    }

    /// <summary>
    /// �ش� ������Ʈ�� �̺�Ʈ(Action<PointerEventData>)�� ��������
    /// </summary>
    /// <param name="go"></param>
    /// <param name="action"></param>
    /// <param name="type">defalut type�� Ŭ��</param>
    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }


    /*
    public static void BindEventAction(this GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEventAction(go, action, type);
    }
    */

    /// <summary>
    /// IsValid ����Ͽ� �ı��� ��üȮ��, ������Ʈ Ǯ���� null üũ������ �����ֱ� ������ ����
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
    

}
