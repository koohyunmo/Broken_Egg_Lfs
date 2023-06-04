using System.Collections.Generic;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    // 등급별 가중치 리스트
    private List<(Define.Grade, float)> weightList = new List<(Define.Grade, float)>
    {
        (Define.Grade.Hero, 0.01f),
        (Define.Grade.Legend, 0.1f),
        (Define.Grade.Unique, 0.2f),
        (Define.Grade.Rare, 0.29f),
        (Define.Grade.Common, 0.4f),
    };

    // 등급 선택 함수
    public Define.Grade GetGrade()
    {
        // 총 가중치 계산
        float totalWeight = 0f;
        foreach ((Define.Grade grade, float weight) in weightList)
        {
            totalWeight += weight;
        }

        // 가중치에 따른 등급 선택
        float randomWeight = Random.Range(0f, totalWeight);
        foreach ((Define.Grade grade, float weight) in weightList)
        {
            if (randomWeight < weight)
            {
                return grade;
            }
            randomWeight -= weight;
        }

        // 선택 실패
        return Define.Grade.Common;
    }

    // 등급별 확률 표시 함수
    public void ShowGradeProbabilities()
    {
        Debug.Log("Grade Probabilities:");
        foreach ((Define.Grade grade, float weight) in weightList)
        {
            Debug.Log($"{grade}: {weight * 100f}%");
        }
    }
}