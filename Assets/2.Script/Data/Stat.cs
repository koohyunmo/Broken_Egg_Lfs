using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Base
{
    public class Stat :MonoBehaviour
    {
        [SerializeField]
        protected int _maxHp;
        [SerializeField]
        protected int _level;
        [SerializeField]
        public  Define.WorldObject _contentType = Define.WorldObject.None;

        public int Level { get { return _level; } set { _level = value; } }
        public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
        public Define.WorldObject PlayType { get { return _contentType; } protected set { _contentType = value; } }

        private void Start()
        {

        }
    }

}