using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;
        //StartCoroutine(co_WaitManagerLoading());
        //StartCoroutine(c_StartCo());

        LoadUI();
    }

    IEnumerator c_StartCo()
    {
        while (true) 
        {
            if (Managers.Data.INIT == true)
            {
                Managers.Data.DataLoaded(LoadUI);
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void LoadUI()
    {
        Managers.UI.ShowPopupUI<UI_PlayPopup>();

#if UNITY_EDITOR
        Managers.UI.ShowPopupUI<UI_Button>();
#endif
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
                yield return new WaitForSeconds(0.1f);
            }
               
        }

        yield break;
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }

}
