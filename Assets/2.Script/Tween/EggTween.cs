using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class EggTween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(c_DieEggAnim());
    }


     IEnumerator c_EggDamaged()
    {
        //transform.DOShakeScale(1f, 0, 90).SetEase<Tween>(Ease.InOutQuad);
        transform.DOPunchScale(new Vector3(0.4f,0.2f),1f,5,90).SetEase<Tween>(Ease.InOutQuad);
        transform.GetComponent<Image>().DOColor(Color.red, 0.5f).SetEase<Tween>(Ease.InSine);
        transform.GetComponent<Image>().DOColor(Color.white, 0.5f).SetDelay<Tween>(0.5f).SetEase<Tween>(Ease.InSine);
        transform.DOScale(new Vector3(1, 1),1).SetDelay<Tween>(1f);

        yield return null;
    }

    public void ClickEgg()
    {
        StartCoroutine(c_EggDamaged());
    }


    IEnumerator c_DieEggAnim()
    {

        transform.DOLocalMove(new Vector3(400, 800, 0), 2);
        transform.DOScale(new Vector3(0f, 0f), 2f);
        


        while (true)
        {
            
            yield return new WaitForSeconds(0.1f);
            transform.DOLocalRotate(new Vector3(0, 0, -360), 0.1f, RotateMode.LocalAxisAdd).Loops();

            if (transform.localPosition.y >= 800)
                break;
        }


        Reborn();

        yield return null;

    }

    void Reborn()
    {
        transform.localPosition = (new Vector3(0, 0, 0));
        transform.localEulerAngles = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(1f, 1f), 0.5f);
    }

}
