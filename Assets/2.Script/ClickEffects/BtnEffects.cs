using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEffects : MonoBehaviour
{
	//터치하면 중심을 바탕으로 이펙트 발생
	public GameObject star;
    public List<GameObject> btns;
    

    void Start()
    {
        foreach (GameObject btn in btns)
        {
            btn.GetComponent<Button>().onClick.AddListener(call: () => ClickStarEffect(btn));
        }
    }

    void ClickStarEffect(GameObject _gameObject)
    {

		Instantiate(star,_gameObject.transform.position, rotation: (Quaternion)Quaternion.identity);
	}
}
