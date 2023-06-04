using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    protected override void Init()
    {
        base.Init();

        // 씬타입, UI매니저
        SceneType = Define.Scene.Game;

        //Managers.UI.showSceneUI<UI_Inven>();
        //Managers.Resource.Instantiate("Test1");
        //Dictionary<int, Stat> dict = Managers.Data.StatDict;
        
    }

    public override void Clear()
    {
       
    }
}
