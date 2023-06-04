using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardOpen : MonoBehaviour
{
    public GameObject ItemCard;
    public GameObject OpenEffect;
    public Text Grade;
    public string _info;


    // Start is called before the first frame update
    void Start()
    {
        Grade.GetComponent<Text>().text = _info;
        OpenEffect.SetActive(true);
        StartCoroutine(c_OpenCard());
    }

    IEnumerator c_OpenCard()
    {
        yield return new WaitForSeconds(1f);
        ItemCard.SetActive(true);
        ItemCard.transform.DOPunchScale(new Vector3(0.3f,0.3f,0.3f),0.3f);
        OpenEffect.SetActive(false);

    }

    public void SetInfo(string info)
    {
        _info = info;
    }
}
