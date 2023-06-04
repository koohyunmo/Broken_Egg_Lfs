using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldAttracted : MonoBehaviour
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


    // �������� ���� ũ��� �������� �Լ�
    private void ResetScale()
    {
        transform.DOScale(originalScale, 0.4f);
    }

    public void RewardGold()
    {
        int reward = Managers.Stage.StageReward();

        Managers.Game.AddGold(reward);
    }
}
