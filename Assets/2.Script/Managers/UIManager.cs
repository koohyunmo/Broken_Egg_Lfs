using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//구현중

public class UIManager 
{
    // [canvas]sort order 관리

    int _order = 2;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    /// <summary>
    /// Root 오브젝트, UI 정렬 역활
    /// </summary>
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    /// <summary>
    /// UI를 화면에 띄어줌
    /// </summary>
    /// <param name="go"></param>
    /// <param name="sort"></param>
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = CUtil.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // 캔버스 중첩시 부모상관없이 무조건 sort order 갖기

        // sort가 필요한 팝업
        if (sort)
        {
            canvas.sortingOrder = (_order);
            _order++;
        }
        // sort가 필요없는 UI
        else
        {
            canvas.sortingOrder = 0;
        }
            


    }

    /// <summary>
    /// 팝업 띄우기,T는 스크립트, 아무나 받는 것이아니라 UI_Popup을 상속받은 스크립트만 이 함수 사용가능
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    // 옵션 name 이 없으면 T의 이름사용
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        // 까먹었을 경우 자동으로 컴포넌트 연결
        T popup = CUtil.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);


        return popup;

    }

    /// <summary>
    /// 팝업 띄우기,부모 설정 버전
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    // 옵션 name 이 없으면 T의 이름사용

    public T ShowPopupUI<T>(string name, Transform parent = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (parent == null)
            return null;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        // 까먹었을 경우 자동으로 컴포넌트 연결
        T popup = CUtil.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(parent.transform);

        return popup;

    }

    /// <summary>
    /// 팝업 띄우기,부모 설정 버전
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    // 옵션 name 이 없으면 T의 이름사용

    public T ShowInfoPopupUI<T>(string name, Transform parent = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (parent == null)
            return null;

        if (parent.Find(name) != null)
            ClosePopupUI();

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        // 까먹었을 경우 자동으로 컴포넌트 연결
        T popup = CUtil.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(parent.transform);

        return popup;

    }


    public T ShowTestPopupUI<T>(string name, Transform parent = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (parent == null)
            return null;

        if (parent.Find(name) != null)
            ClosePopupUI();

        GameObject go = Managers.Resource.Instantiate($"UI/TestUI/{name}");

        // 까먹었을 경우 자동으로 컴포넌트 연결
        T popup = CUtil.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(parent.transform);

        return popup;

    }

    /// <summary>
    /// 씬 UI 띄우기,T는 스크립트, 아무나 받는 것이아니라 UI_Scene 상속받은 스크립트만 이 함수 사용가능
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T showSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        // 까먹었을 경우 자동으로 컴포넌트 연결
        T sceneUI = CUtil.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;

    }
    /// <summary>
    /// 기생하는 UI, ItemUI 만들기,ex)인벤토리 속 아이템같은것들
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public T MakeSubItem<T>(Transform parent = null,string name = null) where T:UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;


        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();

        //return Util.GetOrAddComponent<T>(go);
    }

    /// <summary>
    /// HP나 MP같은 Bar UI 동적 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public T MakeBarUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;


        GameObject go = Managers.Resource.Instantiate($"UI/BarUI/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;


        return go.GetOrAddComponent<T>();

        //return Util.GetOrAddComponent<T>(go);
    }

    /// <summary>
    /// 일반 팝업닫기
    /// </summary>
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;
        
        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    /// <summary>
    /// 안전한 팝업닫기
    /// </summary>
    /// <param name="popup"></param>
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if(_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    /// <summary>
    /// 팝업 다 지우기
    /// </summary>
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    /// <summary>
    /// 생명주기관리
    /// </summary>
    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
