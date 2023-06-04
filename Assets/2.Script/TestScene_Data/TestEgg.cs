using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestEgg : MonoBehaviour
{
    public int currentHp;
    public int maxHp;
    public Camera uiCam;
    [SerializeField]string id;
    public Animator anim;

    private void Start()
    {
        init();
    }
    void init()
    {
        id = TestManger.id;
        gameObject.BindEvent(OnHitMakeEffect);
        anim = GetComponent<Animator>();
        anim.CrossFade("Idle", 0.1f);
    }

    void OnHitMakeEffect(PointerEventData data)
    {
        id = TestManger.id;
        Vector2 mouse = uiCam.ScreenToWorldPoint(Input.mousePosition);
        GameObject go = TestManger.effectObject;
        go.transform.position = mouse;
        GameObject effect = Managers.Resource.Instantiate(go);

        StartCoroutine(EffectDestroy(effect));
        anim.Play("EggOnDamaged");

    }


    IEnumerator EffectDestroy(GameObject go)
    {
        yield return new WaitForSeconds(3f);
        Managers.Resource.Destroy(go);
        StopCoroutine("EffectDestroy");
    }


}
