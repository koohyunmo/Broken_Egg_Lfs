using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<LootScriptable> lootList = new List<LootScriptable>();


    LootScriptable GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<LootScriptable> possibleItems = new List<LootScriptable>();

        foreach(LootScriptable item in lootList)
        {
            if(randomNumber <= item._dropWeight)
            {
                possibleItems.Add(item);
            }
        }
        if(possibleItems.Count > 0)
        {
            LootScriptable droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        LootScriptable droppedItem = GetDroppedItem();

        if(droppedItem != null)
        {
            GameObject go = Instantiate(droppedItemPrefab, spawnPosition,Quaternion.identity);
            go = null;


            float dropForce = 300f;
            Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            go.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }
    }


}
