using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMnagner 
{
    // Resource 매니저 보조


    #region Pool
    class Pool
    {
        public GameObject Orignal { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 10)
        {
            Orignal = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for(int i = 0; i < count; i++)
                Push(Create());
        }


        /// <summary>
        /// 새로운 객체 만듬
        /// </summary>
        /// <returns></returns>
        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Orignal);
            go.name = Orignal.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            //poolable.transform.parent = Root;
            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            // DontDestroyOnLoad 해제용도
            if(parent == null)
            {
                //poolable.transform.parent = Managers.Scene.CurrentScene.transform;
                poolable.transform.SetParent(Managers.Scene.CurrentScene.transform);
            }

            //poolable.transform.parent = parent;
            poolable.transform.SetParent(parent);
            poolable.isUsing = true;

            return poolable;

        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;
    public void Init()
    {
        if (_root == null)
        {
            // 대기실 만들기
            _root = new GameObject { name = "Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);   
        }
    }
    private void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        //pool.Root.parent = _root;
        pool.Root.SetParent(_root);

        _pool.Add(original.name, pool);

    }

    /// <summary>
    ///  다 사용후 반환
    /// </summary>
    /// <param name="poolable"></param>
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if(_pool.ContainsKey(name) == false)
        {
            // 예외경우
            GameObject.Destroy(poolable.gameObject);
            return;
        }


        _pool[name].Push(poolable);
    }

    /// <summary>
    /// Pool에서 꺼내서 쓰기
    /// </summary>
    /// <param name="original"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);
    }



    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Orignal;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);

            _pool.Clear();
        }
    }
}
