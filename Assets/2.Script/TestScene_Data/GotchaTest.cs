using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotchaTest : MonoBehaviour
{

    [SerializeField] GameObject prefabs;
    [SerializeField] GachaSystem2 gachaSystem;
    [SerializeField] Transform[] _frames;
    [SerializeField] Transform _frameTr;

    bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        prefabs = Resources.Load<GameObject>("Prefabs/Card/GochaCard/GachaCard");
        gachaSystem = gameObject.GetComponent<GachaSystem2>();
    }


    public void CardOpen()
    {
        if (isOpen == false)
        {
            isOpen = true;
            string[] results = null;
            while (results == null)
            {
                results = gachaSystem.CreateGachaList();
            }

            for (int i = 0; i < results.Length; i++)
            {
                //addItem;
            }

            StartCoroutine(c_OpenDelay(results));
        }
    }


    IEnumerator c_OpenDelay(string[] itemList)
    {
        for (int i = 0; i < _frames.Length; i++)
        {
            foreach (Transform child in _frames[i])
                GameObject.Destroy(child.gameObject);
        }


        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 9; i++)
        {
            GameObject.Instantiate(prefabs, _frames[i]);
            CardOpen card = prefabs.GetComponent<CardOpen>();
            card.SetInfo(itemList[i]);
            yield return new WaitForSeconds(0.6f);

            Debug.Log(itemList[i]);
        }

        yield return new WaitForSeconds(0.3f);
        isOpen = false;
    }
}
