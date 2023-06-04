using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ChestPopup : UI_Popup
{

    public delegate void ItemUIUpdate();
    ItemUIUpdate itemUIUpdate;

    enum Buttons
    {
        Chest_Open_Button,
    }

    enum Gameobjects
    {
        UpgradeContentGrid,
        
    }

    GameObject chestItem;
   

    public override void Init()
    {
        BindButton(typeof(Buttons));
        BindObject(typeof(Gameobjects));



        SetChestItems();

        chestItem = Resources.Load<GameObject>("Prefabs/UI/SubItem/UI_ChestPopupItem");




    }

    private void Start()
    {
        //TODO

        Init();


    }

    private async void SetChestItems()
    {
        GameObject infoPanel = GetObject((int)Gameobjects.UpgradeContentGrid).gameObject;

        if (infoPanel == null)
            return;

        foreach (Transform child in infoPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        var chestList = await GetFilteredInventoryAsync();


        foreach (string id in chestList)
        {

            GameObject item = Instantiate(chestItem, infoPanel.transform);
            UI_ChestPopupItem inven_Item = item.GetOrAddComponent<UI_ChestPopupItem>();
            inven_Item.InitData(id, infoPanel, ref itemUIUpdate, ItemUpdater);
           // SetSize(iteminven_Itemtransform);

        }
    }


    void ItemUpdater()
    {
        itemUIUpdate?.Invoke();
    }


    async Task<IEnumerable<string>> GetFilteredInventoryAsync()
    {
        return await Task.Run(() =>
        {
            return from i in Managers.Data.ItemDic.Keys
                   where Managers.Data.ItemDic[i].itemType == Define.ItemType.Chest
                   orderby Managers.Data.ItemDic[i].Grade descending
                   select i;
        });
    }
}
