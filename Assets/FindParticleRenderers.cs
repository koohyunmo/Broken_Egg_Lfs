using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindParticleRenderers : MonoBehaviour
{

    enum Name
    {
        RenderMaterial,
        EffecMaterial
    }

    Transform renderTran;
    Transform effectTran;


    [SerializeField] ParticleSystem psRender;
    [SerializeField] Renderer rendererRender;
    [SerializeField] ParticleSystem psEffect;
    [SerializeField] Renderer rendererEffect;

    public Material IconMaterial;

    MaterialGenerate materialGenerate;

    private void Start()
    {
        Init();
    }

    void Init()
    {

        materialGenerate = GetComponentInParent<MaterialGenerate>();

        IconMaterial = materialGenerate.Material;

        // RenderMaterial
        if(renderTran == null)
            renderTran = transform.Find(Name.RenderMaterial.ToString());

        psRender = renderTran.gameObject.GetComponent<ParticleSystem>();

        rendererRender = renderTran.gameObject.GetComponent<Renderer>();
        rendererRender.material = IconMaterial;

        // EffecMaterial
        if(effectTran == null)
         effectTran = renderTran.Find(Name.EffecMaterial.ToString());

        psEffect = effectTran.gameObject.GetComponent<ParticleSystem>();

        rendererEffect = effectTran.gameObject.GetComponent<Renderer>();
        rendererEffect.material = IconMaterial;

    }
}
