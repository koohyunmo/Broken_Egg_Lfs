using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestManger : UI_Base
{

    public static TestManger s_instance = null; // ?????? ????
    public static TestManger Instance { get { return s_instance; } } // ?????? ???????? ????????. ????????

    public static List<ItemScriptbale> effectList { get; private set; } = new List<ItemScriptbale>();

    public static int index = 0;

    public static int damage { get; private set; }
    public static int cost { get; private set; }
    public static string id;
    public static string desc { get; private set; }
    public static string itemType { get; private set; }
    public static GameObject effectObject { get; private set; }
    public static Sprite eggIcon { get; set; }

    enum Buttons
    {
        PrevEffectButton,
        NextEffectButton,
        
    }
    enum Texts
    {
        EffectData,
        EffectTransformData,
        EffectName,
        EffectCode1,
        EffectCode2,
        EffectNameInputField,
    }

    enum Images
    {
        EggIcon,
    }

    private void Awake()
    {
        Init();
    }

    void ClickNextButton(PointerEventData data )
    {
        if (index >= effectList.Count-1)
        {
            return;
        }            
        index++;
        GetData(index);
    }
    void ClickPrevButton(PointerEventData data)
    {
        if (index <= 0)
        {
            return;
        }        
        index--;
        GetData(index);
    }

    void GetData(int index)
    {
        name = effectList[index].itemName;
        id = effectList[index].itemID;
        desc = effectList[index].itemDescription;
        damage = effectList[index].damage;
        cost = effectList[index].itemCost;
        itemType = effectList[index].itemType.ToString();
        effectObject = effectList[index].itemObject;
        eggIcon = effectList[index].itemIcon;

        GetImage((int)Images.EggIcon).sprite = eggIcon;
        GetText((int)Texts.EffectCode1).text = id;
        GetText((int)Texts.EffectCode2).text = id;
        GetText((int)Texts.EffectName).text = name;
        GetText((int)Texts.EffectData).text =
            $"damage : {damage}\n" +
            $"price : {cost}\n" +
            $"none :  \n" +
            $"none :  \n" +
            $"type :  {itemType}\n" +
            $"desc :  {desc}\n";

        GetText((int)Texts.EffectTransformData).text = $"object name : {effectObject.name}\nobject scale : {effectObject.transform.localScale.ToString()}";

        //GetText((int)Texts.EffectNameInputField).text = name;


    }

    public override void Init()
    {


        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

            ItemScriptbale[] itemSO = Resources.LoadAll<ItemScriptbale>("Prefabs/SO/ItemSO/SO");

        Debug.Log(itemSO.Length);

            for (int i = 0; i < itemSO.Length; i++)
            {
                effectList.Add(itemSO[i]);
            }


            GetButton((int)Buttons.NextEffectButton).gameObject.BindEvent(ClickNextButton);
            GetButton((int)Buttons.PrevEffectButton).gameObject.BindEvent(ClickPrevButton);
            GetData(index);

    }
}
