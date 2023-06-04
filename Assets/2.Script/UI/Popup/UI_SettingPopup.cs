using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SettingPopup : UI_Base
{

    enum Sliders
    {
        BGMSlider,
        EffectSoundSlider
    }

    enum Buttons
    {
        GameQuitButton,
        HomeQuitButton,
        OptionQuitButton
    }

    Slider _bgmSlider;
    Slider _effectSlider;

    public override void Init()
    {

        Bind<Slider>(typeof(Sliders));
        BindButton(typeof(Buttons));

        _bgmSlider = Get<Slider>((int)Sliders.BGMSlider);
        _effectSlider = Get<Slider>((int)Sliders.EffectSoundSlider);


        _bgmSlider.onValueChanged.AddListener(ChageBVolume);
        _effectSlider.onValueChanged.AddListener(ChageEVolume);

        GetButton((int)Buttons.OptionQuitButton).gameObject.BindEvent((PointerEventData data) => { this.gameObject.SetActive(false); });
        GetButton((int)Buttons.HomeQuitButton).gameObject.BindEvent((PointerEventData data) => { this.gameObject.SetActive(false); });
        GetButton((int)Buttons.GameQuitButton).gameObject.BindEvent((PointerEventData data) => { Application.Quit(); });

        UpdateVolume();
    }

    public void ChageBVolume(float volume)
    {
        Managers.Sound.SetBGMVolume(volume);
        _bgmSlider.value = Managers.Sound.GetBGMVolume();
    }

    public void ChageEVolume(float volume)
    {
        Managers.Sound.SetEFVolume(volume);
        _effectSlider.value = Managers.Sound.GetEFVolume();
    }

    public void UpdateVolume()
    {
        _bgmSlider.value = Managers.Sound.GetBGMVolume();
        _effectSlider.value = Managers.Sound.GetEFVolume();
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

}
