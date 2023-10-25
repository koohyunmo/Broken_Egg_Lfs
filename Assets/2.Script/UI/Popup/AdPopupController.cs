using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdPopupController : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private TextMeshProUGUI[] _texts;

    private bool[] _isReward = { false, false, false, false, false };

    DateTime bonusTime;

    private void Start()
    {
        bonusTime = ES3.Load<DateTime>("BonusDelay", DateTime.Now);


        StartCoroutine(GemReward());
        StartCoroutine(GoldReward());
        StartCoroutine(ChestReward());
        StartCoroutine(ResetReward());
        StartCoroutine(ResetReward2());
    }


    public string FreeRTimeDisplay()
    {
        TimeSpan timeRemaining; 

        timeRemaining = bonusTime - DateTime.Now;

        string remainTimeText = "";

        if (timeRemaining.TotalSeconds <= 0)
        {
            return remainTimeText = "00:00:00";
        }

        return remainTimeText = string.Format("{0:D2}:{1:D2}", timeRemaining.Minutes, timeRemaining.Seconds);
    }

    public bool CanGetReward()
    {

        TimeSpan timeRemaining;

        timeRemaining = bonusTime - DateTime.Now;

        if (timeRemaining.TotalSeconds <= 0)
        {
            return true;
        }
        else return false;
    }

    public void GetReward(int index)
    {
        _isReward[index] = true;
        _buttons[index].interactable = false;

#if UNITY_EDITOR
        bonusTime = DateTime.Now.AddSeconds(10);
#else
        bonusTime = DateTime.Now.AddMinutes(5);
#endif


        ES3.Save<DateTime>("BonusDelay", bonusTime);

    }
    private IEnumerator GemReward()
    {
        while (true)
        {
            if(CanGetReward() == false)
            {
                _texts[0].text = FreeRTimeDisplay();
                _buttons[0].interactable = false;
            }
            else
            {
                _texts[0].text = "Get";
                _buttons[0].interactable = true;
            }
            yield return new WaitForSeconds(1f);
        }

    }
    private IEnumerator GoldReward()
    {
        while (true)
        {
            if(CanGetReward() == false)
            {
                _texts[1].text = FreeRTimeDisplay();
                _buttons[1].interactable = false;
            }
            else
            {
                _texts[1].text = "Get";
                _buttons[1].interactable = true;
            }

            yield return new WaitForSeconds(1f);
        }

    }
    private IEnumerator ChestReward()
    {
        while (true)
        {
            if (CanGetReward() == false)
            {
                _texts[2].text = FreeRTimeDisplay();
                _buttons[2].interactable = false;
            }
            else
            {
                _texts[2].text = "Get";
                _buttons[2].interactable = true;
            }

            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator ResetReward()
    {
        while (true)
        {
            if (CanGetReward() == false)
            {
                _texts[3].text = FreeRTimeDisplay();
                _buttons[3].interactable = false;
            }
            else
            {
                _texts[3].text = "Reset";
                _buttons[3].interactable = true;
            }

            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator ResetReward2()
    {
        while (true)
        {
            if (CanGetReward() == false)
            {
                _texts[4].text = FreeRTimeDisplay();
                _buttons[4].interactable = false;
            }
            else
            {
                _texts[4].text = "Reset";
                _buttons[4].interactable = true;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
