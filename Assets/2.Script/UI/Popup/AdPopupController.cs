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

    private void Start()
    {
        return;

        StartCoroutine(GemReward());
        StartCoroutine(GoldReward());
        StartCoroutine(ChestReward());
        StartCoroutine(ResetReward());
        StartCoroutine(ResetReward2());
    }

    public void GetReward(int index)
    {
        _isReward[index] = true;
        _buttons[index].interactable = false;
    }
    private IEnumerator GemReward()
    {
        int time = 60*15;
        while (true)
        {
            if(_isReward[0])
            {
                time = 60*15;
                _isReward[0] = false;
            }
            while (time > 0)
            {
                int min = time / 60;
                int sec = time % 60;
                _texts[0].text = $"{min}:{sec}";
                yield return new WaitForSeconds(1f);
                time--;
            }
            _texts[0].text = "Get";
            _buttons[0].interactable = true;
            yield return null;
        }
        yield return null;
    }
    private IEnumerator GoldReward()
    {
        int time = 60*30;
        while (true)
        {
            if (_isReward[1])
            {
                time = 60*30;
                _isReward[1] = false;
            }
            while (time > 0)
            {
                int min = time / 60;
                int sec = time % 60;
                _texts[1].text = $"{min}:{sec}";
                yield return new WaitForSeconds(1f);
                time--;
            }
            _texts[1].text = "Get";
            _buttons[1].interactable = true;
            yield return null;
        }
        yield return null;
    }
    private IEnumerator ChestReward()
    {
        int time = 60 * 60;
        while (true)
        {
            if (_isReward[2])
            {
                time = 60 * 60;
                _isReward[2] = false;
            }
            while (time > 0)
            {
                int min = time / 60;
                int sec = time % 60;
                _texts[2].text = $"{min}:{sec}";
                yield return new WaitForSeconds(1f);
                time--;
            }
            _texts[2].text = "Get";
            _buttons[2].interactable = true;
            yield return null;
        }
        yield return null;
    }
    private IEnumerator ResetReward()
    {
        int time = 60 * 5;
        while (true)
        {
            if (_isReward[3])
            {
                time = 60 * 5;
                _isReward[3] = false;
            }
            while (time > 0)
            {
                int min = time / 60;
                int sec = time % 60;
                _texts[3].text = $"{min}:{sec}";
                yield return new WaitForSeconds(1f);
                time--;
            }
            _texts[3].text = "Reset";
            _buttons[3].interactable = true;
            yield return null;
        }
        yield return null;
    }
    private IEnumerator ResetReward2()
    {
        int time = 60 * 5;
        while (true)
        {
            if (_isReward[4])
            {
                time = 60 * 5;
                _isReward[4] = false;
            }
            while (time > 0)
            {
                int min = time / 60;
                int sec = time % 60;
                _texts[4].text = $"{min}:{sec}";
                    yield return new WaitForSeconds(1f);
                time--;
            }
            _texts[4].text = "Reset";
            _buttons[4].interactable = true;
            yield return null;
        }
        yield return null;
    }
}
