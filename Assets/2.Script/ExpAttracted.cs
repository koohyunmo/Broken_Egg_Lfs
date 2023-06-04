using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExpAttracted : MonoBehaviour
{

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void TweenIcon()
    {
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 5, 1).SetEase<Tween>(Ease.OutQuad).OnComplete(ResetScale);

    }


    // 스케일을 원래 크기로 돌려놓는 함수
    private void ResetScale()
    {
        transform.DOScale(originalScale, 0.4f);
    }

    public void RewardExp()
    {
        //int reward = Managers.Game.StageReward(Managers.Game.StageData.currentStage);
        int reward = Managers.Stage.StageReward();

        Managers.Game.AddExp(reward);
    }
}
