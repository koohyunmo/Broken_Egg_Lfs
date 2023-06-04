using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������

public class UIManager 
{
    // [canvas]sort order ����

    int _order = 2;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    /// <summary>
    /// Root ������Ʈ, UI ���� ��Ȱ
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
    /// UI�� ȭ�鿡 �����
    /// </summary>
    /// <param name="go"></param>
    /// <param name="sort"></param>
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = CUtil.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // ĵ���� ��ø�� �θ������� ������ sort order ����

        // sort�� �ʿ��� �˾�
        if (sort)
        {
            canvas.sortingOrder = (_order);
            _order++;
        }
        // sort�� �ʿ���� UI
        else
        {
            canvas.sortingOrder = 0;
        }
            


    }

    /// <summary>
    /// �˾� ����,T�� ��ũ��Ʈ, �ƹ��� �޴� ���̾ƴ϶� UI_Popup�� ��ӹ��� ��ũ��Ʈ�� �� �Լ� ��밡��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    // �ɼ� name �� ������ T�� �̸����
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        // ��Ծ��� ��� �ڵ����� ������Ʈ ����
        T popup = CUtil.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);


        return popup;

    }

    /// <summary>
    /// �˾� ����,�θ� ���� ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    // �ɼ� name �� ������ T�� �̸����

    public T ShowPopupUI<T>(string name, Transform parent = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (parent == null)
            return null;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        // ��Ծ��� ��� �ڵ����� ������Ʈ ����
        T popup = CUtil.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(parent.transform);

        return popup;

    }

    /// <summary>
    /// �˾� ����,�θ� ���� ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    // �ɼ� name �� ������ T�� �̸����

    public T ShowInfoPopupUI<T>(string name, Transform parent = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (parent == null)
            return null;

        if (parent.Find(name) != null)
            ClosePopupUI();

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        // ��Ծ��� ��� �ڵ����� ������Ʈ ����
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

        // ��Ծ��� ��� �ڵ����� ������Ʈ ����
        T popup = CUtil.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(parent.transform);

        return popup;

    }

    /// <summary>
    /// �� UI ����,T�� ��ũ��Ʈ, �ƹ��� �޴� ���̾ƴ϶� UI_Scene ��ӹ��� ��ũ��Ʈ�� �� �Լ� ��밡��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T showSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        // ��Ծ��� ��� �ڵ����� ������Ʈ ����
        T sceneUI = CUtil.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;

    }
    /// <summary>
    /// ����ϴ� UI, ItemUI �����,ex)�κ��丮 �� �����۰����͵�
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
    /// HP�� MP���� Bar UI ���� ����
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
    /// �Ϲ� �˾��ݱ�
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
    /// ������ �˾��ݱ�
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
    /// �˾� �� �����
    /// </summary>
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    /// <summary>
    /// �����ֱ����
    /// </summary>
    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
