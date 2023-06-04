using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestPopup : UI_Popup
{
    enum GameObjects
    {
        QuestPanel,
    }


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        BindObject(typeof(GameObjects));
        MakeQuestList();
    }



    void MakeQuestList()
    {
        GameObject gridPanel = GetObject((int)GameObjects.QuestPanel);

        // 그리드 비우기
        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < (int)Define.QuestType.End; i++)
        {
            Define.QuestType qt = Define.QuestType.Click+i;

            GameObject item = Managers.UI.MakeSubItem<UI_QuestList>(gridPanel.transform).gameObject;
            UI_QuestList questItem = item.GetOrAddComponent<UI_QuestList>();
            questItem.InitData(Managers.Quest.QuestList[i]);
        }


    }
}
