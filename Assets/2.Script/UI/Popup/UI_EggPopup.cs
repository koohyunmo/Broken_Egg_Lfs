using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EggPopup : UI_Popup
{

    enum GameObjects
    {

    }

    enum Buttons
    {
        SaveTestButton,
        Egg,
    }
    enum Images
    {
        TestImage,
    }

    public override void Init()
    {
        base.Init();

        BindButton(typeof(GameObjects));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));


        //GetButton((int)Buttons.SaveTestButton).gameObject.BindEvent(SaveTest);
        //SetEggImage();

        Managers.UI.MakeBarUI<UI_HpBar>(transform);
    }

    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("Egg Click");
        GameObject effect = Managers.Resource.Instantiate("Effect/Item_ax", gameObject.transform);
        //Sprite sprite = Managers.Resource.Load<Sprite>($"Images/{_score}");
        //GetButton((int)Buttons.TestEgg).image.sprite = Managers.Resource.Load<Sprite>($"Images/{_score}");
        //GetButton((int)Eggs.Egg).image.sprite = sprite;
    }

    public void SaveTest(PointerEventData data)
    {
        Managers.Game.SaveGame("SaveTest");
    }

}
