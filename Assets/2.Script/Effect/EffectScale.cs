using System.Collections;
using UnityEngine;

public class EffectScale : MonoBehaviour
{
    [SerializeField] Transform originScale;
    [SerializeField] float scale;
    [SerializeField] bool recycle;

    // Start is called before the first frame update
    void Start()
    {
        originScale = this.gameObject.transform;
        StartCoroutine(LerpScale());
        recycle = true;
    }


    IEnumerator LerpScale()
    {
        while (transform.localScale.x > 0)
        {
            if (transform.localScale.x < 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.03f);
            transform.localScale -= new Vector3(1, 1, 1);
        }
        

    }

    private void OnEnable()
    {
        if(recycle)
            Start();
    }

    private void OnDisable()
    {
        transform.localScale = new Vector3(10f, 10f, 10f);
    }



}
