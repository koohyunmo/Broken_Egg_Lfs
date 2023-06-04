using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Item : UI_Base
{

    enum Images
    {
        ItemIcon,
        IsUse,
        ItemFrame
    }
    enum GameObjects
    {
        GradeTMP,
        ReinforceCountTMP,
        CardCountTMP,
        CountSlider,

    }

    public string Id { get { return _id; } set { _id = value; } }
    [SerializeField] string _id;

    TextMeshProUGUI _gradeTMP;
    TextMeshProUGUI _reinforceCountTMP;
    TextMeshProUGUI _cardCountTMP;
    Image _icon;
    Image _isUse;
    Image _frameImage;
    UI_InfoPanel _infoPanel;
    Camera TDUIcam;
    Sprite _frameSprite;
    Color _color;
    [SerializeField]Slider _countSlider;

    Tween _iconPunch;
    Tween _framePunch;

    Coroutine _updateUI;

    AudioClip clickSOund;

    private void OnEnable()
    {
        if (_gradeTMP = null)
            return;
        else
        {
            if (_updateUI != null)
                StopCoroutine(_updateUI);
            _updateUI = StartCoroutine(C_UpdateUI());
        }
            
    }

    private void OnDisable()
    {
        _updateUI = null;
        _iconPunch = null;
        _framePunch = null;
    }



    private void Start()
    {
        Init();
    }



    public override void Init()
    {
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));
        //BindObject(typeof(GameObjects));

        TDUIcam = Managers.Effect.TDUICam;

        clickSOund = Managers.Resource.Load<AudioClip>("Sounds/Weapon_Draw_Metal_7");


        _gradeTMP = GetObject((int)GameObjects.GradeTMP).gameObject.GetComponent<TextMeshProUGUI>();
        _reinforceCountTMP = GetObject((int)GameObjects.ReinforceCountTMP).gameObject.GetComponent<TextMeshProUGUI>();
        _cardCountTMP = GetObject((int)GameObjects.CardCountTMP).gameObject.GetComponent<TextMeshProUGUI>();
        _icon = GetImage((int)Images.ItemIcon);
        _isUse = GetImage((int)Images.IsUse);
        _frameImage = GetImage((int)Images.ItemFrame);
        _countSlider = GetObject((int)GameObjects.CountSlider).gameObject.GetComponent<Slider>();


        _isUse.gameObject.SetActive(false);
        _gradeTMP.text = Managers.Data.ItemDic[_id].Grade.ToString();

        
        SetUI();


        _icon.gameObject.BindEvent((PointerEventData data) =>
        {

            _infoPanel.GetID(_id);

            if (Managers.Sound.IsPlaying() == false)
                Managers.Sound.Play(clickSOund);

        });

        _icon.gameObject.BindEvent((PointerEventData data) =>
        {
            // 드래그 이벤트 처리
        });


        switch (Managers.Data.ItemDic[_id].Grade)
        {
            case Define.Grade.None:
                _color = Color.green;
                _frameSprite = Managers.Data.Frames[0];
                break;
            case Define.Grade.Common:
                _color = Color.green;
                _frameSprite = Managers.Data.Frames[1];
                break;
            case Define.Grade.Rare:
                _color = Color.cyan;
                _frameSprite = Managers.Data.Frames[2];
                break;
            case Define.Grade.Unique:
                _color = Color.magenta;
                _frameSprite = Managers.Data.Frames[3];
                break;
            case Define.Grade.Legend:
                _color = Color.yellow;
                _frameSprite = Managers.Data.Frames[4];
                break;
            case Define.Grade.Hero:
                _color = Color.red;
                _frameSprite = Managers.Data.Frames[5];
                break;
        }

        _frameImage.sprite = _frameSprite;
        _gradeTMP.color = _color;


        _updateUI = StartCoroutine(C_UpdateUI());

    }

    private void SetUI()
    {

        UpdateUI();

        _icon.sprite = Managers.Data.ItemDic[_id].itemIcon;
    }

    void UpdateUI()
    {
        Managers.Game.InventoryData.item.TryGetValue(_id, out ItemData ItemData);

        float ratio = 0;

        if (ItemData.reinforce == 0)
        {
            ratio = ((ItemData.itemCount - 1) / 1f); // 분모에 1f를 사용하여 실수형 연산을 수행합니다.
        }
        else
        {
            ratio = ((ItemData.itemCount - 1) / (ItemData.reinforce + 1f)); // 분모에 1f를 사용하여 실수형 연산을 수행합니다.
        }

        _countSlider.value = ratio;

        _reinforceCountTMP.text = "+" + ItemData.reinforce.ToString();
        _cardCountTMP.text = (ItemData.itemCount - 1).ToString() + "/" + (ItemData.reinforce + 1).ToString();
    }


    private void Update()
    {
        if (_id == Managers.Game.UseEquipment)
        {
            _isUse.gameObject.SetActive(true);
        }
        else
        {
            _isUse.gameObject.SetActive(false);
        }
    }


    IEnumerator C_UpdateUI()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            UpdateUI();
        }

        
    }

    public void ReinforceEffect()
    {


        SetUI();

        {
            GameObject go = Managers.Resource.Instantiate(Managers.Reinforce.UpgradeEffect);
            go.transform.SetParent(_icon.transform);
            go.transform.localPosition = Vector3.zero;
            PunchEffect();
            Destroy(go, 1f);
        }


    }


    void PunchEffect()
    {
        // 1. 크기 줄이기
        // GetImage((int)Images.ItemIcon).transform.DOScale(new Vector3(0.7f, 0.7f, 0.3f), 0.5f)
        //.SetEase(Ease.InOutQuad)
        //.OnComplete(() =>
        //{
        // 2. 펀치 스케일 효과 적용
        _iconPunch = _icon.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 1, 1f)
        .SetEase(Ease.OutElastic)
        .OnComplete(() =>
        {
            // 3. 원래 크기로 돌아가기
            _icon.transform.localScale = new Vector3(1, 1, 1);
            //GetImage((int)Images.ItemIcon).transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutQuad);
        });

        // 2. 펀치 스케일 효과 적용
        _framePunch = _frameImage.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f, 1, 1f)
        .SetEase(Ease.OutElastic)
        .OnComplete(() =>
        {
            _frameImage.transform.localScale = new Vector3(1, 1, 1);
            // 3. 원래 크기로 돌아가기
            //GetImage((int)Images.ItemIcon).transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutQuad);
        });
        //});

    }




    public void InitData(string id, GameObject infoPanel)
    {
        _id = id;
        _infoPanel = infoPanel.GetComponent<UI_InfoPanel>();
    }
}
