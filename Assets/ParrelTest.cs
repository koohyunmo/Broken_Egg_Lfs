using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ParrelTest : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        ItemScriptbale[] itemSO = Resources.LoadAll<ItemScriptbale>("Prefabs/SO/ItemSO");
        Dictionary<string, ItemScriptbale> dic = new Dictionary<string, ItemScriptbale>();

        int max = itemSO.Length;

        Parallel.For(0, max, (i) => {
            Debug.Log($"{Thread.CurrentThread.ManagedThreadId}: {itemSO[i].itemName}");
            dic.Add(itemSO[i].itemID, itemSO[i]);
        });

        Debug.Log(dic.Count);
    }
   
}
