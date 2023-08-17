using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MarketTimer : MonoBehaviour
{
    public Text timer;
    Coroutine co_timer;


    private void OnEnable()
    {
        if (co_timer != null)
        {
            StopCoroutine(co_timer);
            co_timer = null;
            co_timer = StartCoroutine(c_Timer());
        }
        else
        {
            co_timer = StartCoroutine(c_Timer());
        }
    }


    IEnumerator c_Timer()
    {
        while (true)
        {
            timer.text = Managers.AD.GetRemainingMarktAdTime();
            yield return new WaitForSeconds(1f);
        }
    }
}
