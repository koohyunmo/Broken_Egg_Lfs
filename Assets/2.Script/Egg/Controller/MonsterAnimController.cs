using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterAnimController : UI_Popup
{

    public MonsterController.MonsterStatement monsterAnimStatement;

    private bool _isIdleAnimTweening = false;
    private bool _isDieAnim = false;
    private Tween _idleAnimTween;
    private Tween _onDamagedTween;
    private Tween _onDamagedTween2;

    private Coroutine CO_onDamaged;
    private Coroutine CO_DieEggAnim;
    private Coroutine c_Emotion;

    public delegate void DieAnimEvent();
    public event DieAnimEvent D_AnimEnd;

    private Image face;
    //public Sprite[] eyes = new Sprite[3];
    //public Sprite[] hiteyes = new Sprite[2];
    //public Sprite[] mouse = new Sprite[3];


    private Tween _monsterTween;


    public Sprite[] emotion;

    private Vector3 original;
    private Image monster;



    private void Update()
    {
        switch (monsterAnimStatement)
        {
            case MonsterController.MonsterStatement.Idle:
                IdleFace();
                break;
            case MonsterController.MonsterStatement.Angry:
                AngryFace();
                break;
            case MonsterController.MonsterStatement.OnDamaged:
                OnDamagedFace();
                break;
            case MonsterController.MonsterStatement.Die:
                OnDieFace();
                break;
            case MonsterController.MonsterStatement.Reborn:
                break;

        }

    }

    private void Start()
    {
        monsterAnimStatement = MonsterController.MonsterStatement.Idle;
        original = transform.localPosition;
        monster = transform.GetComponent<Image>();

        for (int i = 0; i < transform.childCount; i++)
        {
            face = transform.GetChild(i).gameObject.GetComponent<Image>();

            if (face)
                break;

            Debug.Log(face.name);
            // 자식 게임 오브젝트를 사용하는 코드 작성
        }

        LoadEmotions();
    }

    void LoadEmotions()
    {
        emotion = Resources.LoadAll<Sprite>("Images/EggEmotion/Emotion");

    }

    private void IdleFace()
    {
        if (monsterAnimStatement == MonsterController.MonsterStatement.Idle && face.sprite != emotion[0])
            face.sprite = emotion[0];

        if (_monsterTween != null)
            return;
        else
        {
            float dir = UnityEngine.Random.Range(-2, 2);
            dir = Mathf.Clamp(dir, -1, 1);

            monster.DOColor(Color.white, 0.2f);
            _monsterTween = transform.DOLocalJump(new Vector3(50, 50)*dir, 20, 2, 1f).OnComplete(() => { transform.DOLocalMove(original, 0.3f); _monsterTween = null; });
        }
    }

    private void AngryFace()
    {
        if (monsterAnimStatement == MonsterController.MonsterStatement.Angry && face.sprite != emotion[2])
            face.sprite = emotion[2];

        if (_monsterTween != null)
            return;
        else
        {
            float dir = UnityEngine.Random.Range(-4, 4);

            dir = Mathf.Clamp(dir, -3, 3);

            monster.DOColor(Color.white, 0.2f);
            _monsterTween = transform.DOLocalJump(new Vector3(50, 50) * dir, 20, 2, 1f).OnComplete(() => { transform.DOLocalMove(original, 0.3f); _monsterTween = null; });
        }
    }


    private void OnDieFace()
    {
        if (monsterAnimStatement == MonsterController.MonsterStatement.Die && face.sprite != emotion[1])
            face.sprite = emotion[1];
    }
    private void OnDamagedFace()
    {
        if (monsterAnimStatement == MonsterController.MonsterStatement.OnDamaged && face.sprite != emotion[1])
            face.sprite = emotion[1];

        if (_monsterTween != null)
            return;
        else
        {
            float dir = UnityEngine.Random.Range(-3, 3);

            dir = Mathf.Clamp(dir, -2, 2);

            monster.DOColor(Color.red, 0.1f);
            _monsterTween = transform.DOLocalJump(new Vector3(50, 50) * dir, 20, 2, 1f).OnComplete(() => { transform.DOLocalMove(original, 0.3f); _monsterTween = null; });
        }
    }

    public void DieAnim()
    {
        if (_isDieAnim == false)
        {
            _isDieAnim = true;
            //CO_DieEggAnim = StartCoroutine(c_DieEggAnim(() => { _isDieAnim = false; D_AnimEnd(); CO_DieEggAnim = null;  }));
            CO_DieEggAnim = StartCoroutine(c_DieEggAnim2());
        }
    }

    public void OnDamagedAnim()
    {

        CO_onDamaged = StartCoroutine(c_EggDamagedAnim(() => 
        {
            Debug.Log("코루틴 종료");
        }));
    }

    IEnumerator c_EggDamagedAnim(Action onComplete = null)
    {       
        _onDamagedTween = transform.DOPunchScale(new Vector3(-0.3f, -0.3f), 0.2f, 5, 90).SetEase<Tween>(Ease.InOutQuad);
        _onDamagedTween2 = transform.DOScale(new Vector3(1, 1), 0.2f).SetDelay<Tween>(0.02f);


        yield return new WaitForFixedUpdate();
    }

    IEnumerator c_DieEggAnim2()
    {
        CO_onDamaged = null;

        if (_isDieAnim == true)
            yield return null;

        if (_monsterTween != null)
            _monsterTween.Kill();

        Managers.Game.NotifiedEggDie();

        int x = UnityEngine.Random.Range(-400, 400);
        Quaternion currentRotation = transform.localRotation;
        Quaternion returnRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0f);

        Tween _twDieDoMove = transform.DOLocalMove(new Vector3(x, 800, 0), 0.8f);
        Tween _twDieDoScale = transform.DOScale(new Vector3(0f, 0f), 1f);
        Tween _twDieDoRoatrion = transform.DOLocalRotate(new Vector3(0, 0, -1440), 1f, RotateMode.LocalAxisAdd).SetLoops(4, LoopType.Restart);


        yield return new WaitForSeconds(1f);

        _twDieDoMove.Kill();
        _twDieDoScale.Kill();
        _twDieDoRoatrion.Kill();
        _monsterTween.Kill();

        _twDieDoMove = null;
        _twDieDoRoatrion = null;
        _twDieDoScale = null;
        _monsterTween = null;

        _isDieAnim = false;

        if (D_AnimEnd != null)
        {
            D_AnimEnd();
        }
    }



    public void Clear()
    {
        if (_idleAnimTween != null)
            _idleAnimTween.Kill();

        if (_onDamagedTween != null)
            _onDamagedTween.Kill();

        if (_onDamagedTween2 != null)
            _onDamagedTween2.Kill();

        _idleAnimTween = null;
        _onDamagedTween = null;
        _onDamagedTween2 = null;

        CO_onDamaged = null;
        CO_DieEggAnim = null;
        c_Emotion = null;
 
    }

}
