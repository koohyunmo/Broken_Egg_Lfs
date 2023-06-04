using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExpBar : UI_Base
{

    enum GameObjects
    {
        UI_ExpBar,
    }
    
    enum Texts
    {
    }

    private void Start()
    {
        Init();
    }

    Slider expBar;

    public override void Init()
    {
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));

        expBar = GetObject((int)GameObjects.UI_ExpBar).GetComponent<Slider>();
    }

    private void Update()
    {
        float _ratio = (float)Managers.Game.CurrentExp / (float)Managers.Game.MaxExp;
        expBar.value = _ratio;
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.UI_ExpBar).GetComponent<Slider>().value = ratio;
        
    }

}
