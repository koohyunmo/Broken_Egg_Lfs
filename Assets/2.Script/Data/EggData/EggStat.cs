using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggStat : Base.Stat
{

    [SerializeField] public EggType _eggType  = EggType.Defalut;
    [SerializeField] public int _currentHp;

    public enum EggType
    {
        Defalut,
        Nomal,
        Fire,
    }


    private void Start()
    {
        TestStat();
    }

    public void TestStat()
    {
        _level = 10;
        _maxHp = 200;
        _currentHp = _maxHp;
        _eggType = EggType.Nomal;
        _contentType = Define.WorldObject.Monster;
    }


}
