using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewGachaTest : MonoBehaviour
{
    public Image ItemImage;
    public Image ChestImage;
    public Image ChestOpenImage;
    public GameObject Effect;
    public GameObject Effect2;
    public GameObject Card;
    public GameObject[] Element;

    public Tween tween1;
    public Tween tween2;


    // Start is called before the first frame update
    public void OpenEffect()
    {
        Effect.gameObject.SetActive(true);
        Effect2.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 1f);
        tween1 = ChestImage.transform.DOPunchPosition(new Vector3(10f, 10f, 10f), 1f).SetLoops(3).OnComplete(() =>
        {
            ChestImage.gameObject.SetActive(false);
            ChestOpenImage.gameObject.SetActive(true);
            Effect2.SetActive(true);

            for (int i = 0; i < Element.Length; i++)
            {
                Element[i].SetActive(true);
            }

            tween2 = ChestOpenImage.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 1f).OnComplete(() =>
            {             
                //ChestOpenImage.gameObject.SetActive(false);
                //Card.SetActive(true);
            });
        });
    }

    void Clean()
    {
        ChestImage.gameObject.SetActive(true);
        ChestOpenImage.gameObject.SetActive(false);
        Effect.gameObject.SetActive(false);
        //ChestOpenImage.gameObject.SetActive(false);
        Card.SetActive(false);

        tween1.Kill();
        tween2.Kill();
    }
}
