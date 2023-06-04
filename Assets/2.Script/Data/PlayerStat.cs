using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Base.Stat
{

    [SerializeField]
    protected int exp;
    [SerializeField]
    protected int gold;
    [SerializeField]
    protected int damage;

    public int Exp { get { return exp; } set { exp = value; } }
    public int Gold { get { return gold; } set { gold = value; } }
    public int Damage { get { return damage;} set { damage = value; } }

    private void Start()
    {
        _contentType = Define.WorldObject.Player;
        TestStat();
    }

    public void TestStat()
    {
        _level = 1;
        _maxHp = 200;
        gold = 10;
        exp = 0;
        damage = 20;
        
    }


}
