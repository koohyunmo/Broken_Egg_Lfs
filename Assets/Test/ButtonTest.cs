using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    Vector3 MousePos, MousePoint;
    public GameObject Effect;
    public Transform EffectPosition;
    Animation anim;
    [SerializeField]
    GameObject Egg;
    Image EggImage;
    int EggNum = 1;
    public int EffectNum = 1;
    public int MultiPlyCount = 100;
    public GameObject NewOne;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        EggImage = Egg.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TextTest.TestCount <= 0)
        {
            EggNum += 1;
            EffectNum += 1;
            TextTest.TestCount = MultiPlyCount;
            MultiPlyCount *= 10;
            string EggNumString = EggNum.ToString();
            EggImage.sprite = Resources.Load<Sprite>("Images" + "/" + EggNumString) as Sprite;
            Effect = Resources.Load<GameObject>("Prefabs/Effect" + "/" + EffectNum) as GameObject;
            NewOne.SetActive(true);
        }
    }

    public void Count()
    {
        TextTest.TestCount -= 1;
        anim.Play();
        var Effects = Instantiate(Effect, EffectPosition.position, Quaternion.identity);
        Destroy(Effects.gameObject, 100.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "Item_ax")
		{
			Debug.Log(" 1¹ø µµ³¢ Ãæµ¹");
			TextTest.TestCount -= 1;
		}
		else if (collision.gameObject.name == "Item_ax1")
		{
			Debug.Log(" 2¹ø µµ³¢ Ãæµ¹");
			TextTest.TestCount -= 2;
		}

	}
}
