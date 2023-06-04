using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialGenerate : MonoBehaviour
{
    public Material Material;
    public Texture2D particleTexture { get; private set; }


    void Start()
    {
        Material = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
        Material.color = Color.red;
        Material.SetTexture("_MainTex", particleTexture);

    }

    public void GetTexture2D(Texture2D particleTexture)
    {
        this.particleTexture = particleTexture;
    }

}
