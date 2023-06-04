using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePos : MonoBehaviour
{

    public Transform levelIcon;
    public Transform goldIcon;

    public GameObject goldParticle;
    public UIParticle goldUiParticle;
    public GameObject ExpParticle;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        levelIcon = GameObject.Find("LevelIcon").transform;
        goldIcon = GameObject.Find("GoldIcon").transform;

        goldParticle = Managers.Resource.Load<GameObject>("Prefabs/Particle/GoldParticle");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
