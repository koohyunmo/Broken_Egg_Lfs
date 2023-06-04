using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    public List<QuestScriptable> QuestList = new List<QuestScriptable>();

    public void Init()
    {
        QuestScriptable[] qsSO = Resources.LoadAll<QuestScriptable>("Prefabs/SO/QuestSO/");

        for (int i = 0; i < qsSO.Length; i++)
        {
            QuestList.Add(qsSO[i]);
        }

        if (Managers.Game.QuestData.clickRewardId == null)
            SetFirstReward();
    }

    public int ClickCountQuest()
    {
        int questPoint = (Managers.Game.QuestData.clickCountQuestLevel + 1) * 100;

        return questPoint;
    }

    public int UseGoldQuest()
    {
        int questPoint = (Managers.Game.QuestData.usingGoldQuestLevel + 1) * (int)Mathf.Pow( 1000,1.4f);

        return questPoint;
    }

    public int UseGemQuest()
    {
        int questPoint = (Managers.Game.QuestData.usingGemQuestLevel + 1) * 10;

        return questPoint;
    }

    public int ReinforceCountQuest()
    {
        int questPoint = (Managers.Game.QuestData.reinforceCountGoldQuestLevel + 1) * 5;

        return questPoint;
    }

    private void SetFirstReward()
    {
        Managers.Game.QuestData.clickRewardId = Managers.Data.GoldLIST[0];
        Managers.Game.QuestData.usingGoldRewardId = Managers.Data.GemLIST[0];
        Managers.Game.QuestData.reinforceRewardId = Managers.Data.WeaponQuestList[1];
        Managers.Game.QuestData.usingGemRewardId = Managers.Data.ChestList[0];

        Managers.Game.SaveGame("SetFirstReward");
    }

    public void RewardCheck(int level, Define.QuestType qt)
    {
        int index = level / 10;
        int i = 0;


        switch (qt)
        {
            case Define.QuestType.Click:
                i = Mathf.Clamp(index, 0, Managers.Data.GoldLIST.Count-1);
                Managers.Game.QuestData.clickRewardId = Managers.Data.GoldLIST[i];
                break;
            case Define.QuestType.Gold:
                i = Mathf.Clamp(index, 0, Managers.Data.GemLIST.Count-1);
                Managers.Game.QuestData.usingGoldRewardId = Managers.Data.GemLIST[i];
                break;
            case Define.QuestType.Gem:
                i = Mathf.Clamp(index, 0, Managers.Data.ChestList.Count-1);
                Managers.Game.QuestData.usingGemRewardId = Managers.Data.ChestList[i];
                break;
            case Define.QuestType.Reinforce:
                i = Mathf.Clamp(index, 0, Managers.Data.WeaponQuestList.Count - 1);
                Managers.Game.QuestData.reinforceRewardId = Managers.Data.WeaponQuestList[i];
                break;
        }

    }


}
