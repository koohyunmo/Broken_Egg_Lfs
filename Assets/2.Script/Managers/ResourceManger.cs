using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManger
{
    /// <summary>
    /// ������ ���� �ȿ��ִ� ������ �ּҸ� ���� ã��
    /// </summary>
    /// <typeparam name="T">���ʸ�</typeparam>
    /// <param name="path">�ش� ���� �ּ�</param>
    /// <returns></returns>
    public  T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }


    /// <summary>
    /// ������ ���� �ּҸ� ���� �ش� ������Ʈ ����
    /// </summary>
    /// <param name="path">�ּ�</param>
    /// <param name="parent">�θ� ������Ʈ</param>
    /// <returns></returns>
    public GameObject Instantiate(string path, Transform parent = null)
    {
        
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if(original == null)
        {
            Debug.Log($"Failed to load prefabs : {path}");
            return null;
        }
        // 2. Ǯ���� �ְ� ������?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Instantiate ,clone ���ڿ� ����
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }


    public GameObject Instantiate(GameObject obj, Transform parent = null)
    {

        GameObject original = obj;

        if (original == null)
        {
            Debug.Log($"Failed to load prefabs : {obj.name}");
            return null;
        }
        // 2. Ǯ���� �ְ� ������?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Instantiate ,clone ���ڿ� ����
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public GameObject Instantiate(GameObject obj, Vector3 pos, Quaternion rotation, Transform parent=null)
    {

        GameObject original = obj;

        if (original == null)
        {
            Debug.Log($"Failed to load prefabs : {obj.name}");
            return null;
        }
        // 2. Ǯ���� �ְ� ������?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Instantiate ,clone ���ڿ� ����
        GameObject go = Object.Instantiate(original, parent);
        go.transform.parent = parent;
        go.transform.position = pos;
        go.transform.rotation = rotation;
        go.name = original.name;
        return go;
    }
    /// <summary>
    /// ��ġ �޴� ����
    /// </summary>
    /// <param name="path">�ּ�</param>
    /// <param name="parent">�θ� ������Ʈ</param>
    /// <returns></returns>
    public GameObject Instantiate(string path, Vector3 position,Transform parent = null)
    {

        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if (original == null)
        {
            Debug.Log($"Failed to load prefabs : {path}");
            return null;
        }
        // 2. Ǯ���� �ְ� ������?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Instantiate ,clone ���ڿ� ����
        GameObject go = Object.Instantiate(original, position, Quaternion.identity);
        go.name = original.name;
        return go;
    }


    /// <summary>
    /// �����ֱ� ����
    /// </summary>
    /// <param name="go"></param>
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // ���࿡ Ǯ���� �ʿ��� ���̶�� -> Ǯ���Ŵ����� ����
        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    public void Destroy(GameObject go,float time)
    {
        if (go == null)
            return;

        // ���࿡ Ǯ���� �ʿ��� ���̶�� -> Ǯ���Ŵ����� ����
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go, time);
    }
}
