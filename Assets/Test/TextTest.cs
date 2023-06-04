using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{

	public Text TestText;

    public static int TestCount = 50;


    // Start is called before the first frame update
    void Start()
    {
		TestText.text = TestCount.ToString();
	}

    // Update is called once per frame
    void Update()
    {
		TestText.text = TestCount.ToString();
	}

	
}
