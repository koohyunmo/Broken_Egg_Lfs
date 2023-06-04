using System.Collections.Generic;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    // ��޺� ����ġ ����Ʈ
    private List<(Define.Grade, float)> weightList = new List<(Define.Grade, float)>
    {
        (Define.Grade.Hero, 0.01f),
        (Define.Grade.Legend, 0.1f),
        (Define.Grade.Unique, 0.2f),
        (Define.Grade.Rare, 0.29f),
        (Define.Grade.Common, 0.4f),
    };

    // ��� ���� �Լ�
    public Define.Grade GetGrade()
    {
        // �� ����ġ ���
        float totalWeight = 0f;
        foreach ((Define.Grade grade, float weight) in weightList)
        {
            totalWeight += weight;
        }

        // ����ġ�� ���� ��� ����
        float randomWeight = Random.Range(0f, totalWeight);
        foreach ((Define.Grade grade, float weight) in weightList)
        {
            if (randomWeight < weight)
            {
                return grade;
            }
            randomWeight -= weight;
        }

        // ���� ����
        return Define.Grade.Common;
    }

    // ��޺� Ȯ�� ǥ�� �Լ�
    public void ShowGradeProbabilities()
    {
        Debug.Log("Grade Probabilities:");
        foreach ((Define.Grade grade, float weight) in weightList)
        {
            Debug.Log($"{grade}: {weight * 100f}%");
        }
    }
}