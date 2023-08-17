using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Timer : MonoBehaviour
{
    public TextMeshProUGUI[] timer;
    Coroutine co_timer;


    private void OnEnable()
    {
        if(co_timer != null)
        {
            StopCoroutine(co_timer);
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
           
            foreach (var item in timer)
            {
                item.text = Managers.AD.GetRemainingAdTime();
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
