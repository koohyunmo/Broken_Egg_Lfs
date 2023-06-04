using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    [SerializeField]
    public GameObject objectPregab;
    public Transform _parent;

    
    IEnumerator Start()
    {
        while(true)
        {
            //Vector3 position = new Vector3(Random.Range(-5.0f, 5.0f), 5, 0);
            Instantiate(objectPregab, transform.localPosition, Quaternion.identity, _parent);

            yield return new WaitForSeconds(1);
        }
    }
    


}
