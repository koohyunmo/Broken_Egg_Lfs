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
    /// 해당 오브젝트에 이벤트(Action<PointerEventData>)를 매핑해줌
    /// </summary>
    /// <param name="go"></param>
    /// <param name="action"></param>
    /// <param name="type">defalut type은 클릭</param>
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
    /// IsValid 사용하여 파괴된 객체확인, 오브젝트 풀링은 null 체크만으로 생명주기 관리가 힘듬
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
    

}
