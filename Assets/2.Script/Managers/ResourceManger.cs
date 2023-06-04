using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManger
{
    /// <summary>
    /// 프리팹 폴더 안에있는 파일을 주소를 통해 찾기
    /// </summary>
    /// <typeparam name="T">제너릭</typeparam>
    /// <param name="path">해당 파일 주소</param>
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
    /// 프리팹 파일 주소를 통해 해당 오브젝트 복제
    /// </summary>
    /// <param name="path">주소</param>
    /// <param name="parent">부모 오브젝트</param>
    /// <returns></returns>
    public GameObject Instantiate(string path, Transform parent = null)
    {
        
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if(original == null)
        {
            Debug.Log($"Failed to load prefabs : {path}");
            return null;
        }
        // 2. 풀링된 애가 있을까?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Instantiate ,clone 문자열 제거
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
        // 2. 풀링된 애가 있을까?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Instantiate ,clone 문자열 제거
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
        // 2. 풀링된 애가 있을까?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Instantiate ,clone 문자열 제거
        GameObject go = Object.Instantiate(original, parent);
        go.transform.parent = parent;
        go.transform.position = pos;
        go.transform.rotation = rotation;
        go.name = original.name;
        return go;
    }
    /// <summary>
    /// 위치 받는 버전
    /// </summary>
    /// <param name="path">주소</param>
    /// <param name="parent">부모 오브젝트</param>
    /// <returns></returns>
    public GameObject Instantiate(string path, Vector3 position,Transform parent = null)
    {

        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if (original == null)
        {
            Debug.Log($"Failed to load prefabs : {path}");
            return null;
        }
        // 2. 풀링된 애가 있을까?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Instantiate ,clone 문자열 제거
        GameObject go = Object.Instantiate(original, position, Quaternion.identity);
        go.name = original.name;
        return go;
    }


    /// <summary>
    /// 생명주기 관리
    /// </summary>
    /// <param name="go"></param>
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // 만약에 풀링이 필요한 아이라면 -> 풀링매니저로 관리
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

        // 만약에 풀링이 필요한 아이라면 -> 풀링매니저로 관리
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go, time);
    }
}
