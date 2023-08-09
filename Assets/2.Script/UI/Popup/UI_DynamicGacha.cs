using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DynamicGacha : UI_PopupWithItem
{

    [SerializeField] Transform[] cardEndTr;

    string _chesthId;

    Image _chestImage;
    Image _chesIcontImage;

    Sprite _chestOpenSprite;
    Sprite _chestSprite;

    Tween _punchChest;


    [SerializeField] GameObject _shineEffectLoad;
    GameObject _shineEffect;

    [SerializeField] GameObject _openEffectLoad;
    GameObject _openEfect;

    [SerializeField] GameObject _elementEffectLoad;
    GameObject[] _elementEffect = new GameObject[10];

    [SerializeField] GameObject _cardObjectLoad;

    [SerializeField] GameObject _button;

    [SerializeField] List<string> _itemList = new List<string>();
    [SerializeField] List<GameObject> _itemObjects = new List<GameObject>();


    Coroutine c_load;

    TextMeshProUGUI _countTMP;

    enum TMPS
    {
        ChestCount
    }


    enum Images
    {
        Chest,
        ChestIconImage,
    }

    enum Buttons
    {
        OpenButton,
        OutButton,

    }

    DynamicGachSystem _gacha;
    bool _isOpen;
    bool _isOpening;
    Image _buttonImage;

    Action _updateUI;
    float _originalVolume;

    AudioClip _chestHitSound;
    AudioClip _chestOpenSound;
    AudioClip _elementSound;

    Coroutine c_Sound;

    public void InitData(string chestId,Action updateUI)
    {
        _chesthId = chestId;

        _updateUI = null;
        _updateUI = updateUI;

        _originalVolume = Managers.Sound.GetEFVolume();
        Managers.Sound.SetEFVolume(0f);
        Managers.Sound.SetCHVolume(_originalVolume);


        _chestHitSound = Managers.Resource.Load<AudioClip>("Sounds/Chest Close 2");
        _chestOpenSound = Managers.Resource.Load<AudioClip>("Sounds/Chest Open Creak 2_1");
        _elementSound = Managers.Resource.Load<AudioClip>("Sounds/Healing 02");
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();


        _gacha = gameObject.GetOrAddComponent<DynamicGachSystem>();

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPS));

        _isOpen = false;
        _isOpening = false;

        // nullcheck
        if (_chesthId == null || _chesthId == "")
        {
            _chesthId = "CH0001";
        }

        // Load
        //_chestSprite = Resources.Load<Sprite>($"Images/Chest/{_chesthId}");
        //_chestOpenSprite = Resources.Load<Sprite>($"Images/Chest/{_chesthId}_OPEN");

        _chestSprite = Managers.Data.ItemDic[_chesthId].itemIcon;
        _chestOpenSprite = Managers.Data.ItemDic[_chesthId].chesOpenImage;

        _shineEffectLoad = Resources.Load<GameObject>($"Prefabs/Effect/BG_Effects/Sun_Shines_01");
        _openEffectLoad = Resources.Load<GameObject>($"Prefabs/Effect/BG_Effects/Confetti_02");
        _elementEffectLoad = Resources.Load<GameObject>($"Prefabs/Effect/BG_Effects/Sun_Shines_02");
        _cardObjectLoad = Resources.Load<GameObject>($"Prefabs/Card/GochaCard/GachaCard");


        // Bind
        _chestImage = GetImage((int)Images.Chest);
        _chesIcontImage = GetImage((int)Images.ChestIconImage);
        _countTMP = Get<TextMeshProUGUI>((int)TMPS.ChestCount);

        _chestImage.sprite = _chestSprite;
        _chesIcontImage.sprite = _chestSprite;
        _countTMP.text = $"x{Managers.Chest.ChestCount(_chesthId)}";



        // Event

        // 나중에 버튼 변경
        _button.gameObject.BindEvent((PointerEventData) =>
        {
            if (Managers.Chest.ChestCount(_chesthId) <= 0)
                return;

            if (_isOpening == false)
            {
                OpenItemList();
                _isOpening = true;
                OpenEffect2();

            }

        });

        GetButton((int)Buttons.OpenButton).gameObject.BindEvent((PointerEventData) =>
        {
            if (Managers.Chest.ChestCount(_chesthId) <= 0)
                return;

            if (_isOpening == false)
            {
                OpenItemList();
                _isOpening = true;
                OpenEffect2();
            }

        });

        GetButton((int)Buttons.OutButton).gameObject.BindEvent((PointerEventData) =>
        {

            Managers.Sound.SetEFVolume(_originalVolume);
            Managers.Sound.SetCHVolume(0f);
            _updateUI.Invoke();
            Managers.UI.ClosePopupUI(this);

        });

        _buttonImage = GetButton((int)Buttons.OpenButton).GetComponent<Image>();

        SetCanvas();


    }


    IEnumerator co_Sound()
    {

        // 3초 닫힘
        // 2초 열림

        Managers.Sound.Play(_chestHitSound, Define.Sound.Chest);
        yield return new WaitForSeconds(1f);
        Managers.Sound.Play(_chestHitSound, Define.Sound.Chest);
        yield return new WaitForSeconds(1f);
        Managers.Sound.Play(_chestHitSound, Define.Sound.Chest);
        yield return new WaitForSeconds(0.92f);
        Managers.Sound.Play(_chestOpenSound, Define.Sound.Chest);
        yield return new WaitForSeconds(0.04f);
        Managers.Sound.Play(_elementSound, Define.Sound.Chest);
        yield return new WaitForSeconds(1.8f);
        Managers.Sound.Play(_elementSound, Define.Sound.Chest);
        c_Sound = null;
    }

    public void OpenItemList()
    {

        _itemList.Clear();
        _itemList = _gacha.SetData(_chesthId);


        foreach (string s in _itemList)
        {
            Managers.Game.Additem(s);
        }

        Debug.Log(_itemList.Count);


    }

    // 빛나는 이펙트 활성화
    // 상자 흔들림
    // 상자 열림
    // 원소들 발사
    // 원소들 위치에 도착
    // 원소들 카드로 변함

    #region 재귀버전
    public void OpenEffect()
    {

        if (_isOpen == true)
        {
            StartCoroutine(c_Clear());
        }
        else
        {
            _shineEffect = GameObject.Instantiate(_shineEffectLoad, _chestImage.transform);
            _punchChest = _chestImage.transform.DOPunchPosition(new Vector3(10f, 10f, 10f), 1f).SetLoops(3).OnComplete(() =>
            {
                _chestImage.sprite = _chestOpenSprite;


                _isOpening = false;
                _isOpen = true;
            });
        }

        // 빛나는 이펙트 활성화
        // 상자 흔들림
        // 상자 열림
        // 원소들 발사
        // 원소들 위치에 도착
        // 원소들 카드로 변함

    }
    IEnumerator c_Clear()
    {

        // 상자 초기화
        _chestImage.sprite = _chestSprite;
        _isOpen = false;

        // 빛나는 이펙트 제거
        Destroy(_shineEffect);

        yield return new WaitForSeconds(0.5f);

        // 다시 열릴 수 있도록 재귀 호출
        OpenEffect();
    }
    #endregion

    #region 비 재귀 버전
    public void OpenEffect2()
    {
        Managers.Chest.Open(_chesthId);
        UpdateTMP();

        if (c_Sound != null)
        {
            StopCoroutine(co_Sound());
            c_Sound = StartCoroutine(co_Sound());
        }
        else
        {
            c_Sound = StartCoroutine(co_Sound());
        }

        if (_isOpen == true)
        {     
            StartCoroutine(c_Clear2(() =>
            {
                _shineEffect = GameObject.Instantiate(_shineEffectLoad, _chestImage.transform);
                _punchChest = _chestImage.transform.DOPunchPosition(new Vector3(10f, 10f, 10f), 1f).SetLoops(3).OnComplete(() =>
                {
                    _chestImage.sprite = _chestOpenSprite;

                    _openEfect = GameObject.Instantiate(_openEffectLoad, _chestImage.transform);


                    for (int i = 0; i < _elementEffect.Length; i++)
                    {
                        _elementEffect[i] = GameObject.Instantiate(_elementEffectLoad, transform);
                        _elementEffect[i].AddComponent<ItemMovement>().InitData(_chestImage.transform, cardEndTr[i]);
                    }


                    StartCoroutine(c_LoadCard());

                    //_isOpening = false;
                    _isOpen = true;
                });
            }));
        }
        else
        {
            _shineEffect = GameObject.Instantiate(_shineEffectLoad, _chestImage.transform);

            _punchChest = _chestImage.transform.DOPunchPosition(new Vector3(10f, 10f, 10f), 1f).SetLoops(3).OnComplete(() =>
            {
                _chestImage.sprite = _chestOpenSprite;

                _openEfect = GameObject.Instantiate(_openEffectLoad, _chestImage.transform);

                for (int i = 0; i < _elementEffect.Length; i++)
                {
                    _elementEffect[i] = GameObject.Instantiate(_elementEffectLoad, transform);
                    _elementEffect[i].AddComponent<ItemMovement>().InitData(_chestImage.transform, cardEndTr[i]);
                }

                StartCoroutine(c_LoadCard());

                //_isOpening = false;
                _isOpen = true;
            });
        }


    }

    IEnumerator c_LoadCard()
    {
        GachaCard gachaCard = null;

        yield return new WaitForSeconds(1f);
        

        for (int i = 0; i < _elementEffect.Length; i++)
        {
            GameObject go = Instantiate(_cardObjectLoad, _elementEffect[i].transform.parent);
            GachaCard gc = go.GetOrAddComponent<GachaCard>();
            gc.InitData(_itemList[i], _elementEffect[i]);
            go.transform.position = cardEndTr[i].position;
            go.transform.localScale = new Vector3(1, 1, 1);
            _itemObjects.Add(go);
            go.SetActive(true);
            _elementEffect[i].transform.GetChild(0).gameObject.SetActive(false);

            if(gachaCard == null)
            {
                gachaCard = go.GetComponent<GachaCard>();
                gachaCard.End += AnimEnd;
            }
                
        }


    }

    private void UpdateTMP()
    {
        _countTMP.text = $"x{Managers.Chest.ChestCount(_chesthId)}";

        if(Managers.Chest.ChestCount(_chesthId) <= 0)
        {
            _buttonImage.color = Color.gray;
        }
        else
            _buttonImage.color = Color.green;

    }

    public void AnimEnd()
    {
        _isOpening = false;
        UpdateTMP();
        Debug.Log("AnimEnd");
    }


    private IEnumerator c_Clear2(Action onComplete = null)
    {
        Destroy(_shineEffect);
        Destroy(_openEfect);

        foreach (GameObject go in _elementEffect)
        {
            Destroy(go);
        }

        foreach (GameObject go in _itemObjects)
        {
            Destroy(go);
        }
        _chestImage.sprite = _chestSprite;

        if(c_Sound != null)
        {
            StopCoroutine(co_Sound());
            c_Sound = null;
        }

        yield return new WaitForSeconds(0.3f);

        _isOpen = false;
        _punchChest = null;
        onComplete?.Invoke();
    }
    #endregion
}
