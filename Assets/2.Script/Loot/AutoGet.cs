using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGet : MonoBehaviour
{
    [SerializeField] GameObject _target;
    [SerializeField]Vector3 dir;
    [SerializeField]string _name;
    [SerializeField] TrailRenderer _tr;

    Vector3 start, end;

    Gold _gold;
    EXP _exp;

    [SerializeField] int _targetY = 825;

    private void Start()
    {
        Init();
    }

    void Init()
    {

        _tr = GetComponent<TrailRenderer>();
        _name = gameObject.name;

        switch(_name)
        {
            case "Gold": 
                _target = GameObject.Find("GoldIcon");
                _gold = GetComponent<Gold>();
                break;
            case "EXP":
                _target = GameObject.Find("LevelIcon");
                _exp = GetComponent<EXP>();
                break;
            default:
                Debug.Log("no target");
                return;
        }

        start = new Vector2(0, 0);
        end = _target.transform.position;

    }

    private void Update()
    {
        if (transform.localPosition.y >= _targetY)
        {

            switch (_name)
            {
                case "Gold":
                    //_gold.AutoGetGold(this.gameObject);
                    _tr.enabled = false;
                    break;
                case "EXP":
                    //_exp.AutoGetExp(this.gameObject);
                    _tr.enabled = false;
                    break;
            }
            return;
        }

        Vector3 dir = end - start;

        Vector3 move = new Vector3(dir.x, dir.y, 0) * Time.deltaTime;
        transform.Translate(move, Space.World);

        if (transform.localPosition.y <= -10)
        {
            _tr.enabled = false;
            transform.localPosition = new Vector2(0, 0);
            _tr.enabled = true;
        }


    }


}
