using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaSystem2 : MonoBehaviour
{

    int[] gradeChances = { 50, 25, 15, 8, 2 }; // 등급별 확률
    string[] grades = { "Common", "Rare", "Unique", "Legendary", "Hero" }; // 등급명
    int[] results = new int[9]; // 뽑기 결과 저장할 배열
    string[] outputList = new string[9];

    public string[] CreateGachaList()
    {
        for (int i = 0; i < results.Length; i++)
        {
            int randomValue = UnityEngine.Random.Range(1, 101); // 1~100 사이의 랜덤값 생성
            int gradeIndex = 0; // 랜덤값에 해당하는 등급 인덱스
            int chanceSum = 0; // 누적 확률값

            // 랜덤값에 해당하는 등급 인덱스 구하기
            for (int j = 0; j < gradeChances.Length; j++)
            {
                chanceSum += gradeChances[j];

                if (randomValue <= chanceSum)
                {
                    gradeIndex = j;
                    break;
                }
            }

            results[i] = gradeIndex;
        }

        for (int i = 0; i < results.Length; i++)
        {
            outputList[i] = grades[results[i]];
        }

        TestLog();

        return outputList;

    }

    void TestLog()
    {
        // 결과 출력
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log(grades[results[i]]);
        }
    }
}
