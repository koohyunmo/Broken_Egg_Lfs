using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{

    // 원하는 모바일 해상도
    int setWidth = 720;
    int setHeight = 1280;

    protected override void Init()
    {

        base.Init();
        SceneType = Define.Scene.Login;
#if UNITY_EDITOR_WIN
        //SetResolution();
#endif

        StartCoroutine(co_WaitManagerLoading());

    }

    IEnumerator co_WaitManagerLoading()
    {
        while (true)
        {
            

            if (Managers.Data.INIT == true)
            {
                Managers.UI.ShowPopupUI<UI_PlayPopup>();

#if UNITY_EDITOR
                Managers.UI.ShowPopupUI<UI_Button>();
#endif
                break;
            }
            else
            {
                yield return null;
            }
               
        }

        yield break;
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        bool isFull = true;
#if UNITY_EDITOR_WIN
        isFull = false;
#endif

        int setWidth = 1080; // 사용자 설정 너비
        int setHeight = 1920; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), isFull); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
