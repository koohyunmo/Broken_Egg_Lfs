using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{

    // ���ϴ� ����� �ػ�
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

    /* �ػ� �����ϴ� �Լ� */
    public void SetResolution()
    {
        bool isFull = true;
#if UNITY_EDITOR_WIN
        isFull = false;
#endif

        int setWidth = 1080; // ����� ���� �ʺ�
        int setHeight = 1920; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), isFull); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }
}
