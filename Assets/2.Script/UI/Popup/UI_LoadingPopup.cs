using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingPopup : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI text;
    void Start()
    {
        StartCoroutine(c_LoadingBar());
    }


    IEnumerator c_LoadingBar()
    {
        while (Managers.Data.INIT == false)
        {
            yield return null;

            float ratio = Managers.Data.count / Managers.Data.maxCount;
            slider.value = ratio;
        }

        Destroy(gameObject);
    }


}
