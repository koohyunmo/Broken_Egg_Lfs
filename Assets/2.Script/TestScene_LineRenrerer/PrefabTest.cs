using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{

    GameObject test;
    public string PrefabsName;

    private void Start()
    {
        test = Managers.Resource.Instantiate(PrefabsName);
        //Destroy(test, 3.0f);

    }

}
