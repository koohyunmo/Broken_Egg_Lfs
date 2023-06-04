using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{

    public enum MonsterStatement
    {
        Idle,
        Angry,
        OnDamaged,
        Die,
        Reborn
    }

    [SerializeField] MonsterStatement _state;
    GameObject _monster;
    MonsterStat _stat;
    EggView _view;
    MonsterAnimController _anim;

    [SerializeField] GameObject goldEvent;
    [SerializeField] GameObject expEvent;

    [SerializeField] private float _maxTime = 30f; // 보스 스테이지 타이머 최대값
    [SerializeField] private Slider _timeSlider; // 타이머를 표시할 슬라이더
    private float _currentTime = 0; // 현재 타이머 값
    Coroutine _coBossCheck; // 보스 타이머 코루틴
    Coroutine co_autoAttack; // 자동공격 코루틴
    Coroutine co_stateCheck; // 보스 타이머 코루틴
    Coroutine co_isClick;

    bool bossTimeChecker;
    bool _disableClick;

    long _damage;

    int _stage;

    AudioClip onDamageSound;
    AudioClip[] onHitSound = new AudioClip[2];
    bool _isClick;


    string _currenWeaponSound;

    int _count;

    void InitData()
    {
        {
            Managers.Loot.SetTransform();
            Managers.Loot.GetGoldParticle(GameObject.FindGameObjectWithTag("GPS").GetComponentInChildren<ParticleSystem>());
            Managers.Loot.GetExpParticle(GameObject.FindGameObjectWithTag("EPS").GetComponentInChildren<ParticleSystem>());
        }

        if (_timeSlider == null)
        {
            _timeSlider = Managers.Stage.TimerSlider.GetComponent<Slider>();
        }
        SetMonster();

        onDamageSound = Managers.Resource.Load<AudioClip>("Sound/VOICE_Boy_6yo_Hurt_Long_01_mono");

        _currenWeaponSound = Managers.Game.UseEquipment;
        _stage = Managers.Game.StageData.currentStage; 
        SetSound();


    }

    void SetSound()
    {
        onHitSound[0] = null;
        onHitSound[1] = null;

        switch (Managers.Data.ItemDic[_currenWeaponSound].soundType)
        {
            case Define.EffectSound.NONE:
                break;
            case Define.EffectSound.Punch:
                onHitSound[0] = Managers.Resource.Load<AudioClip>("Sounds/Punch Simple 1_1");
                onHitSound[1] = Managers.Resource.Load<AudioClip>("Sounds/Punch Simple 2_1");
                break;
            case Define.EffectSound.Wood:
                onHitSound[0] = Managers.Resource.Load<AudioClip>("Sounds/Whoosh 1_3");
                onHitSound[1] = Managers.Resource.Load<AudioClip>("Sounds/Whoosh 3_2");
                break;
            case Define.EffectSound.ShortSword:
                onHitSound[0] = Managers.Resource.Load<AudioClip>("Sounds/Stab 1_1");
                onHitSound[1] = Managers.Resource.Load<AudioClip>("Sounds/Stab 2_1");
                break;
            case Define.EffectSound.Sword:
                onHitSound[0] = Managers.Resource.Load<AudioClip>("Sounds/HIt3");
                onHitSound[1] = Managers.Resource.Load<AudioClip>("Sounds/HIt3");
                break;
            case Define.EffectSound.Staff:
                onHitSound[0] = Managers.Resource.Load<AudioClip>("Sounds/Magic Element 12");
                onHitSound[1] = Managers.Resource.Load<AudioClip>("Sounds/Magic Element 12");
                break;
            case Define.EffectSound.Hammer:
                onHitSound[0] = Managers.Resource.Load<AudioClip>("Sounds/Metal Weapon Hit Metal 3_1");           
                onHitSound[1] = Managers.Resource.Load<AudioClip>("Sounds/Metal Weapon Hit Metal 3_1");           
                break;
            case Define.EffectSound.Bow:
                onHitSound[0] = Managers.Resource.Load<AudioClip>("Sounds/Arrow Fly 1_1");                
                onHitSound[1] = Managers.Resource.Load<AudioClip>("Sounds/Arrow Fly 2_5");                
                break;
            case Define.EffectSound.Claw:
                onHitSound[0] = Managers.Resource.Load<AudioClip>("Sounds/Stab 1_1");
                onHitSound[1] = Managers.Resource.Load<AudioClip>("Sounds/Stab 2_1");
                break;
            case Define.EffectSound.MAXCOUNT:
                break;
        }



        if(onHitSound == null)
        {
            Debug.Log("Managers.Data.ItemDic[_currenWeaponSound].soundType");
            Debug.Log("_currenWeaponSound");
        }

        
    }

    void SetMonster()
    {
        _monster = null;
        _monster = Managers.Resource.Instantiate("MonsterEgg");
        _monster.transform.localScale = new Vector3(0, 0, 0);
        _monster.transform.localPosition = new Vector3(0, 0, 0);
        _monster.transform.SetParent(transform);

        _view = _monster.GetOrAddComponent<EggView>();
        _stat = _monster.GetOrAddComponent<MonsterStat>();
        _anim = _monster.GetOrAddComponent<MonsterAnimController>();

        _stat.Died += OnMonsterDied;
        _anim.D_AnimEnd += DieAnimeEnd;
        _stat.MakeText += MakeText;


        if(co_autoAttack == null)
         co_autoAttack = StartCoroutine(c_AutoAttack());

        if (co_isClick == null)
            co_isClick = StartCoroutine(c_Click());

        if (co_stateCheck == null)
            co_stateCheck = StartCoroutine(c_CheckState());

        _isClick = false;

        _monster.gameObject.BindEvent((PointerEventData) =>
        {
            if (_disableClick == false)
            {
                _isClick = true;
                StartCoroutine(OnDamagedSound());
                OnDamaged();
                Managers.Game.ClickCount();
                Managers.Game.SaveGame("Click");
            }
        });

        _view.OnStageAura();

        _state = MonsterStatement.Idle;
        Managers.Game.SaveGame("SetMonster");

    }

    IEnumerator c_CheckState()
    {

        while (true)
        {
            yield return new WaitForSeconds(3f);

            if (_isClick == false && _state == MonsterStatement.OnDamaged)
            {
                _state = MonsterStatement.Angry;
            }
        }

    }

    IEnumerator OnDamagedSound()
    {
        if(_currenWeaponSound != Managers.Game.UseEquipment)
        {
            _currenWeaponSound = Managers.Game.UseEquipment;
            SetSound();
           
        }
        Managers.Sound.Play(onHitSound[_count%2]);
        Debug.Log(onHitSound[_count % 2].name);
        _count++;
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play(onDamageSound);
    }

    IEnumerator c_AutoAttack()
    {

        while ( _state != MonsterStatement.Die && _state != MonsterStatement.Reborn)
        {
            yield return new WaitForSeconds(1f);

            long d = 0;

            foreach (var item in Managers.Game.InventoryData.item.Values)
            {
                d += (long)(item.itemDamage * ((int)item.itemGrade * 0.1f));
                if (d <= 0)
                    d = 1;
            }

            _stat.OnDamaged(d, false);
        }
        co_autoAttack = null;
    }


    async Task<long> CalculateDamageAsync()
    {
        long d = 0;

        // 모든 아이템의 데미지 계산
        foreach (var item in Managers.Game.InventoryData.item.Values)
        {
            // 비동기 처리
            await Task.Yield();

            // 데미지 계산
            d += (long)(item.itemDamage * ((int)item.itemGrade * 0.1f));
        }

        // 최소 데미지 보장
        if (d <= 0)
            d = 1;

        return d;
    }



    IEnumerator c_Click()
    {

        while (true)
        {
            yield return new WaitForSeconds(1f);
            _isClick = false; 
        }
    }



    private void Start()
    {
        InitData();
    }



    private void Update()
    {


        switch (_state)
        {
            case MonsterStatement.Idle:
                IdleUpdate();
                BossChecker();
                break;
            case MonsterStatement.Angry:
                OnAngryUpdate();
                break;
            case MonsterStatement.OnDamaged:
                OnDamagedUpdate();
                break;
            case MonsterStatement.Die:
                DieUpdate();
                break;
            case MonsterStatement.Reborn:
                RebornUpdate();
                break;
        }

        if (_stage != Managers.Game.StageData.currentStage)
        {
            
            _stage = Managers.Game.StageData.currentStage;
            Clear();
            Co_Start();
            
        }
    }


    // delegate
    void OnMonsterDied()
    {
        

        _disableClick = true;
        if(co_autoAttack != null)
        {
            StopCoroutine(co_autoAttack);
            co_autoAttack = null;
        }
        //Debug.Log("Monster died");
        _state = MonsterStatement.Die;
        Managers.Game._eggDie = true;
        Managers.Loot.CreateGold();
        Managers.Loot.CreateEXP();
        _anim.DieAnim();
    }

    // delegate
    void DieAnimeEnd()
    {
        //Debug.Log("Monster Anim End");
        _state = MonsterStatement.Reborn;
        Reborn();


    }

    // reborn egg settings
    void Reborn()
    {
        DieClear();
        Managers.Game.EggKill();
        Managers.Game.EggHPCharge();
        Managers.Game.GameDataRefresh();
        SetMonster();
        _state = MonsterStatement.Idle;

        _disableClick = false;
        Managers.Game._eggDie = false;


        if (co_autoAttack == null)
            co_autoAttack = StartCoroutine(c_AutoAttack());

        if (co_isClick == null)
            co_isClick = StartCoroutine(c_Click());

        if (co_stateCheck == null)
            co_stateCheck = StartCoroutine(c_CheckState());
    }


    // clear
    void DieClear()
    {
        if (_coBossCheck != null)
        {
            StopCoroutine(TimerCoroutine());
            _coBossCheck = null;
        }

        // 상태 초기화
        {
            bossTimeChecker = false;
        }

        _view.EggDie();
        _anim.Clear();
        Destroy(_monster);
        Managers.Effect.EffectClear();
    }

    void Co_Start()
    {
        StartCoroutine(co_Start());
    }

    IEnumerator co_Start()
    {
        yield return new WaitForSeconds(0.5f);

        if (co_autoAttack == null)
            co_autoAttack = StartCoroutine(c_AutoAttack());

        if (co_isClick == null)
            co_isClick = StartCoroutine(c_Click());

        if (co_stateCheck == null)
            co_stateCheck = StartCoroutine(c_CheckState());

        _view.OnStageAura();
    }

    void Clear()
    {


        if (_coBossCheck != null)
        {
            StopCoroutine(_coBossCheck);
            StopCoroutine(TimerCoroutine());
            _coBossCheck = null;
        }

        if (co_isClick != null)
        {
            StopCoroutine(co_isClick);
            StopCoroutine(c_Click());
            co_isClick = null;
        }

        if (co_autoAttack != null)
        {
            StopCoroutine(co_autoAttack);
            StopCoroutine(c_AutoAttack());
            co_autoAttack = null;
        }

        if (co_stateCheck != null)
        {
            StopCoroutine(co_stateCheck);
            StopCoroutine(c_CheckState());
            co_stateCheck = null;
        }

        // 상태 초기화
        {
            bossTimeChecker = false;
            _state = MonsterStatement.Idle;
            _coBossCheck = null;
            _timeSlider.value = 1f;
        }

        _view.OffAura();
        _anim.Clear();

        Managers.Game.EggHPCharge();
        Managers.Effect.EffectClear();


    }




    private void OnAngryUpdate()
    {
        _state = MonsterStatement.Angry;
        _anim.monsterAnimStatement = MonsterStatement.Angry;
    }

    private void IdleUpdate()
    {
        _state = MonsterStatement.Idle;
        _anim.monsterAnimStatement = MonsterStatement.Idle;
    }
    private void OnDamagedUpdate()
    {
        _state = MonsterStatement.OnDamaged;
        _anim.monsterAnimStatement = MonsterStatement.OnDamaged;
    }

    private void DieUpdate()
    {
        _state = MonsterStatement.Die;
        _anim.monsterAnimStatement = MonsterStatement.Die;
    }
    private void RebornUpdate()
    {
        _state = MonsterStatement.Reborn;
        _anim.monsterAnimStatement = MonsterStatement.Reborn;
    }
    private void OnDamaged()
    {
        //_damage = Managers.Game.CalculateDamage();
        var dmg = CalculateDamage();
        _state = MonsterStatement.OnDamaged;
        _stat.OnDamaged(dmg.Item1, dmg.Item2);
        Managers.Effect.UseEffect(Managers.Game.UseEquipment, dmg.Item2);
        _anim.OnDamagedAnim();
    }


    public long CalculateAutoDamage()
    {


        return 0;
    }

    public Tuple<long, bool> CalculateDamage()
    {

        int isCiritical = UnityEngine.Random.Range(0, 101);
        float randomDamage = 0;
        ItemData playWeapon = Managers.Game.EquipItemData;
        bool isCritical = false;

        float percent = Mathf.Clamp(playWeapon.itemCriticalPercent, 0, 101);


        if (percent >= isCiritical)
        {
            randomDamage = UnityEngine.Random.Range(playWeapon.itemCriticalPlusDamage, playWeapon.itemCriticalPlusDamage+0.3f);
            isCritical = true;
        }
        else
        {
            randomDamage = UnityEngine.Random.Range(1, 1.3f);
        }



        long totalDamage = (long)(randomDamage * Managers.Game.TotalDamage);

        return new Tuple<long, bool>(totalDamage, isCritical); ;

    }

    private void MakeText(long damage, bool isCritical)
    {
        if (isCritical == true)
            _view.MakeCriticalDamageText(damage);
        else
            _view.MakeDamageText(damage);
    }



    public void ChangeStageAndState()
    {
        _state = MonsterStatement.Idle;
    }


    #region boss

    void BossChecker()
    {
        if (Managers.Game.StageData.currentStage % 5 == 0)
        {         
            if (bossTimeChecker == false)
            {
                _view.OnBossAura();
                bossTimeChecker = true;
                //Debug.Log("Boss Timer Start");
                _currentTime = _maxTime;
                _timeSlider.gameObject.SetActive(true);

                if (_coBossCheck != null)
                {
                    StopCoroutine(_coBossCheck); // 현재 실행중인 코루틴 중지
                }
                _coBossCheck = StartCoroutine(TimerCoroutine()); // 타이머 코루틴 시작
            }
        }
        else
        {
            _view.OffBossAura();
            if (_coBossCheck != null)
            {
                StopCoroutine(_coBossCheck); // 타이머 코루틴 중지
                _coBossCheck = null; // 코루틴 참조 초기화
            }

            _timeSlider.gameObject.SetActive(false);
            _timeSlider.value = 1f;
            bossTimeChecker = false;
        }
    }

    // 타이머를 업데이트하는 코루틴
    private IEnumerator TimerCoroutine()
    {
        _currentTime = 30;
        Managers.Game.EggHPCharge();
        _state = MonsterStatement.Idle;

        while (_currentTime > 0f)
        {
            _currentTime -= Time.deltaTime;
            _timeSlider.value = _currentTime / _maxTime; // 슬라이더에 ratio 값을 적용

            if (_state == MonsterStatement.Die)
            {
                yield break;
            }

            yield return null;
        }

        if (_state != MonsterStatement.Die)
        {
            _coBossCheck = StartCoroutine(TimerCoroutine());
        }
        else
        {
            _coBossCheck = null;
        }



    }
    #endregion
}
