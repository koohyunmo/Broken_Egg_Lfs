using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu (fileName="New Item", menuName = "ScriptableObject/Item")]
public class ItemScriptbale : ScriptableObject
{
    [Header("아이템 ID")]
    public string itemID;
    [Header("아이템 이름")]
    public string itemName;
    [Header("아이템 설명")]
    [TextArea]
    public string itemDescription;
    [Header("가격")]
    public int itemCost;
    [Header("아이콘")]
    public Sprite itemIcon;
    [Header("백그라운드 이미지")]
    public Sprite itemBackGroundImage;
    [Header("아이템 타입")]
    public Define.ItemType itemType;
    [Header("아이템 얻는 루트")]
    public Define.GetRouteItemType getRouteItemType;
    [Header("등급")]
    public Define.Grade Grade;
    [Header("아이템 얻는 스테이지 이상")]
    public int canDropStage;
    [Header("아이템 얻는 스테이지 이하")]
    public int cantDropStage;
    [Header("상점/제작 해금 스테이지")]
    public int getItemStage;
    [Header("이펙트 타입")]
    public Define.EffectType effectType;
    [Header("데미지")]
    public int damage;
    [Header("크리티컬 확률")]
    public float criticalPercent;
    [Header("크리티컬 데미지 배율")]
    public float criticalDamage;
    [Header("이펙트 오브젝트")]
    public GameObject itemObject;
    [Header("이펙트 오브젝트 위치")]
    public string itemObjectPath;
    [Header("아이콘 위치")]
    public string itemIconPath;
    [Header("상자 확률")]
    public List<int> chestPercent;
    [Header("열린 상자 이미지")]
    public Sprite chesOpenImage;
    [Header("방어력 무시")]
    public int shieldAttack;
    [Header("골드 보상")]
    public int rewardGold;
    [Header("보석 보상")]
    public int rewardGem;
    [Header("보석")]
    public int gem;
    [Header("사운드 타입")]
    public Define.EffectSound soundType;


    public List<string> FirstItemData()
    {
        List<string> list = new List<string>();

        list.Add("ID : " + itemID.ToString());
        list.Add("Name : " + itemName.ToString());
        list.Add("Description : " + itemDescription.ToString());
        list.Add("Price : " + itemCost.ToString());
        list.Add("Icon : " + itemIcon.ToString());
        list.Add("Type : " + itemType.ToString());
        list.Add("Route : " + getRouteItemType.ToString());
        list.Add("Grade : " + Grade.ToString());
        list.Add("Damage : " + damage.ToString());

        return list;

    }

    public GameObject GetClickEffect()
    {
        return Managers.Resource.Load<GameObject>(itemObjectPath+$"{itemID}");
    }
}



#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(ItemScriptbale))]
public class AssetPreviewer : Editor
{
    ItemScriptbale item;
    Texture2D icon;

    public override void OnInspectorGUI()
    {
        item = (ItemScriptbale)target;

        GUILayout.BeginHorizontal();
        icon = AssetPreview.GetAssetPreview(item.itemIcon);

        GUILayout.Label(icon);

        GUILayout.EndHorizontal();
        DrawDefaultInspector();

    }
}

#endif
