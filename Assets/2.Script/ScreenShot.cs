using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ScreenCapture.CaptureScreenshot("ScreenShot" + DateTime.Now.Second + DateTime.Now.Millisecond + ".png");
            Debug.Log("스크린샷저장");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Time.timeScale = 1;
        }
    }
#endif
}
