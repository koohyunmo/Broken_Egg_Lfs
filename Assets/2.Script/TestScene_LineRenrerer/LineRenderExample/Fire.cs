using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject prefabs;
    public Transform Spwan;
    public LineRenderer lineRenderer;

    GameObject go;
    Transform end;

    Rigidbody2D rigid;


    private void Start()
    {
        Spwan.right = new Vector2(2, 1);
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TryFire();
        }
    }

    void TryFire()
    {
        go = Instantiate(prefabs, Spwan.localPosition, Spwan.localRotation,gameObject.transform);
        go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, 0);
        go.GetComponent<Rigidbody2D>().velocity = Spwan.transform.right * Random.Range(5f,10f);

        StartCoroutine(YCheckValue(go));

        Destroy(go, 5f);
    }


    IEnumerator YCheckValue(GameObject go)
    {
        float y = go.transform.position.y;
        Debug.Log(y);

        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (y < go.transform.position.y)
                y = go.transform.position.y;

            if (y > go.transform.position.y)
            {
                GameObject go1 = new GameObject { name = "High" };
                go1.transform.SetParent(gameObject.transform);
                go1.transform.localPosition = go.transform.localPosition;
                DrawLine(go1);
                Debug.Log("y°¨¼Ò");
                break;
            }
                
        }

        yield return null; 
    }

    void DrawLine(GameObject go)
    {
        lineRenderer.SetPosition(0, Spwan.transform.position);
        lineRenderer.SetPosition(1, go.transform.position);
    }


}
