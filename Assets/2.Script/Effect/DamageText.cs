using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshPro _text;

    private void OnEnable()
    {
        if (_text == null)
        {
            Init();
        }
        else
        {
            _text.color = Color.white;
        }
     
    }

    private void OnDisable()
    {
        _text.text = string.Empty;
        transform.position = Vector3.zero;
    }

    private void LateUpdate()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void Init()
    {
        _text = GetComponent<TextMeshPro>();

        if (_text == null)
        {
            Debug.LogError("Text component not found.");
            return;
        }
    }

    public void InitData(long damage)
    {
        if (_text == null)
            Init();

            MakeText(damage);
    }

    public void InitDataCri(long damage)
    {
        if (_text == null)
            Init();

        MakeCriticalText(damage);
    }

    void MakeCriticalText(long damage)
    {

        (string numStr, string code) = CUtil.GetNumberAndText(damage);

        //_text.text = "-" + Util.FormatNumber(damage);
        _text.text = $"<color=red> -{numStr}</color> <color=red>{code}</color>";

        // Set the text of the TMP_Text component to "10M"




        float x = UnityEngine.Random.Range(-100, 100);

        Vector3 startPos = new Vector3(transform.localPosition.x + x, 400, transform.localPosition.y);
        Vector3 endPos = new Vector3(startPos.x, 600f, startPos.z) + new Vector3(0, 300f, 0);

        transform.localPosition = startPos;
        transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);

        // 2초 동안 위로 올리고, 사라지는 애니메이션
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMove(endPos, 2f).SetEase(Ease.OutCubic));
        seq.Join(_text.DOFade(0f, 2f));
        seq.OnComplete(() => {
            Managers.Resource.Destroy(gameObject);
        });
    }


    void MakeText(long damage)
    {

        (string numStr, string code) = CUtil.GetNumberAndText(damage);

        //_text.text = "-" + Util.FormatNumber(damage);
        string color = CUtil.GetGradeColorDamageString(Managers.Game.UseEquipment);
        _text.text = $"<color={color}>-{numStr}</color> <color={color}>{code}</color>";

        // Set the text of the TMP_Text component to "10M"




        float x = UnityEngine.Random.Range(-100, 100);

        Vector3 startPos = new Vector3(transform.localPosition.x + x, 400, transform.localPosition.y);
        Vector3 endPos = new Vector3 (startPos.x,600f,startPos.z) + new Vector3(0, 300f, 0);

        transform.localPosition = startPos;

        // 2초 동안 위로 올리고, 사라지는 애니메이션
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMove(endPos, 2f).SetEase(Ease.OutCubic));
        seq.Join(_text.DOFade(0f, 2f));
        seq.OnComplete(() => {
            Managers.Resource.Destroy(gameObject);
        });
    }


    /*
    void SetColor(string id, Define.DamgeType damgeType = Define.DamgeType.Basic)
    {

        if (damgeType == Define.DamgeType.Basic)
        {
            // 기본
            switch (Managers.Data.ItemSO[id].effectType)
            {
                case Define.EffectType.Hit:
                    _effectColor = new Color(150 / 255f, 150 / 255f, 150 / 255f, 1f);
                    break;
                case Define.EffectType.Fire:
                    _effectColor = new Color(255 / 255f, 0 / 255f, 0 / 255f, 1f);
                    break;
                case Define.EffectType.Water:
                    _effectColor = new Color(0 / 255f, 106 / 255f, 255 / 255f, 1f);
                    break;
                case Define.EffectType.Wind:
                    _effectColor = new Color(136 / 255f, 255 / 255f, 136 / 255f, 1f);
                    break;
                case Define.EffectType.Electricity:
                    _effectColor = new Color(255 / 255f, 255 / 255f, 0 / 255f, 1f);
                    break;
                default:
                    _effectColor = Color.white;
                    break;
            }
        }
        else if (damgeType == Define.DamgeType.Dot)
        {
            // 도트
            switch (Managers.Data.ItemSO[id].effectType)
            {
                case Define.EffectType.Hit:
                    _effectColor = new Color(150 / 255f, 150 / 255f, 150 / 255f, 1f);
                    break;
                case Define.EffectType.Fire:
                    _effectColor = new Color(255 / 255f, 153 / 255f, 0 / 255f, 1f);
                    break;
                case Define.EffectType.Water:
                    _effectColor = new Color(0 / 255f, 190 / 255f, 255 / 255f, 1f);
                    break;
                case Define.EffectType.Wind:
                    _effectColor = new Color(204 / 255f, 255 / 255f, 153 / 255f, 1f);
                    break;
                case Define.EffectType.Electricity:
                    _effectColor = new Color(255 / 255f, 255 / 255f, 153 / 255f, 1f);
                    break;
                default:
                    _effectColor = Color.white;
                    break;
            }
        }
    }
    */

}