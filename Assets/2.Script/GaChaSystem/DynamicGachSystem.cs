using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGachSystem : MonoBehaviour
{
    List<Define.Grade> grades = new List<Define.Grade>();
    List<Define.Grade> chooseGrades = new List<Define.Grade>();
    List<string> resultList = new List<string>();
    public List<string> SetData(string id)
    {
        grades.Clear();
        chooseGrades.Clear();
        resultList.Clear();

        for (int i = 0; i < Managers.Data.ItemDic[id].chestPercent[0]; i++)
        {
            grades.Add(Define.Grade.Common);
        }
        for (int i = 0; i < Managers.Data.ItemDic[id].chestPercent[1]; i++)
        {
            grades.Add(Define.Grade.Rare);
        }
        for (int i = 0; i < Managers.Data.ItemDic[id].chestPercent[2]; i++)
        {
            grades.Add(Define.Grade.Unique);
        }
        for (int i = 0; i < Managers.Data.ItemDic[id].chestPercent[3]; i++)
        {
            grades.Add(Define.Grade.Legend);
        }
        for (int i = 0; i < Managers.Data.ItemDic[id].chestPercent[4]; i++)
        {
            grades.Add(Define.Grade.Hero);
        }

        for (int i = 0; i < 10; i++)
        {
            int rand = Random.Range(0, grades.Count - 1);
            chooseGrades.Add(grades[rand]);
        }

        for (int i = 0; i < chooseGrades.Count; i++)
        {
            switch (chooseGrades[i])
            {
                case Define.Grade.None:
                    break;
                case Define.Grade.Common:
                    resultList.Add(ItemGetID(Managers.Data.CommonLIST));
                    break;
                case Define.Grade.Rare:
                    resultList.Add(ItemGetID(Managers.Data.RareLIST, "Rare"));
                    break;
                case Define.Grade.Unique:
                    resultList.Add(ItemGetID(Managers.Data.UniqueLIST, "Unique"));
                    break;
                case Define.Grade.Legend:
                    resultList.Add(ItemGetID(Managers.Data.LegendLIST, "Legend"));
                    break;
                case Define.Grade.Hero:
                    resultList.Add(ItemGetID(Managers.Data.HerorLIST, "Hero"));
                    break;
            }
        }

        return resultList;

    }


    string ItemGetID(List<string> list, string check = "Common")
    {
        if (list.Count <= 0)
        {
            Debug.Log($"{check} List is NULL");
            string cid = (ItemGetID(Managers.Data.CommonLIST));
            return cid;
        }
            

        int index = Random.Range(0, list.Count - 1);
        index = Mathf.Clamp(index, 0, list.Count - 1);
        string s = list[index];

        return s;
    }


}
