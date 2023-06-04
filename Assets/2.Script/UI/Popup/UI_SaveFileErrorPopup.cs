using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SaveFileErrorPopup : MonoBehaviour
{

    public Button StartButton;
    public Button QuitButton;
    public GameObject popup;
    public string _path;
    public Action _action;

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    public void InitData(string p, Action action)
    {
        _path = p;
        _action = action;
    }

    public void OnClickNew()
    {
        File.Delete(_path);
        _action?.Invoke();
       // GameObject.Destroy(popup);
    }

    public void OnClickQuit()
    {
        GameObject.Destroy(popup); 
        Application.Quit();
    }


}
