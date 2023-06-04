using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceTest : MonoBehaviour
{


    [SerializeField] private GameObject ReinCard;
    [SerializeField] private GameObject successEffect;
    [SerializeField] private GameObject failEffect;
    [SerializeField] private float successEffectDuration = 1f;
    [SerializeField] private float failEffectDuration = 1f;

    private int enhanceLevel = 0;
    bool _isRein;

    private void Start()
    {
        successEffect = Resources.Load<GameObject>("Prefabs/Card/ReinCard/UpgradeEffect");
        failEffect = Resources.Load<GameObject>("Prefabs/Card/ReinCard/UpgradeFailedEffect");
    }

    private int GetEnhanceSuccessRate()
    {
        if (enhanceLevel < 10)
            return 50;
        else if (enhanceLevel < 20)
            return 40;
        else if (enhanceLevel < 25)
            return 30;
        else
            return 0; // 강화 불가능
    }

    public void Enhance()
    {
        if (enhanceLevel >= 25)
            return; // 이미 최대 강화 레벨에 도달함

        int successRate = GetEnhanceSuccessRate();
        bool isSuccess = Random.Range(1, 101) <= successRate;

        if (isSuccess)
        {
            // 강화 성공
            enhanceLevel++;
            ReinCard.transform.DOPunchScale(new Vector3(0.4f, 0.4f, 0.4f), 0.8f);
            StartCoroutine(PlayEffect(successEffect, successEffectDuration));
            
        }
        else
        {

            ReinCard.transform.DOShakeScale(0.3f, 0.6f);
            // 강화 실패
            StartCoroutine(PlayEffect(failEffect, failEffectDuration));
        }
    }

    IEnumerator PlayEffect(GameObject effectPrefab, float duration)
    {
        if (effectPrefab == null)
            yield return null;

        GameObject effectObject = Instantiate(effectPrefab, ReinCard.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Destroy(effectObject);
        _isRein = false;
    }

    public void DoReinforce()
    {
        if(_isRein == false)
        {
            _isRein = true;
            Enhance();
        }
        
    }

}
