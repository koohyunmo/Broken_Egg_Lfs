using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaSystem2 : MonoBehaviour
{

    int[] gradeChances = { 50, 25, 15, 8, 2 }; // ��޺� Ȯ��
    string[] grades = { "Common", "Rare", "Unique", "Legendary", "Hero" }; // ��޸�
    int[] results = new int[9]; // �̱� ��� ������ �迭
    string[] outputList = new string[9];

    public string[] CreateGachaList()
    {
        for (int i = 0; i < results.Length; i++)
        {
            int randomValue = UnityEngine.Random.Range(1, 101); // 1~100 ������ ������ ����
            int gradeIndex = 0; // �������� �ش��ϴ� ��� �ε���
            int chanceSum = 0; // ���� Ȯ����

            // �������� �ش��ϴ� ��� �ε��� ���ϱ�
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
        // ��� ���
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log(grades[results[i]]);
        }
    }
}
