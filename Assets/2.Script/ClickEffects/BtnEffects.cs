using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEffects : MonoBehaviour
{
	//��ġ�ϸ� �߽��� �������� ����Ʈ �߻�
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
