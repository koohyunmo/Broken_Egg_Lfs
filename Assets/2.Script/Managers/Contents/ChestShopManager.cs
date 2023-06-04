using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestShopManager
{

    public void Init()
    {

    }

    public void Open(string id)
    {
        if (ChestCount(id) > 0)
            Managers.Game.MinusItem(id);
        else
            return;     
    }

    public int ChestCount(string id)
    {
        return Managers.Game.InventoryData.item[id].itemCount;
    }

}
