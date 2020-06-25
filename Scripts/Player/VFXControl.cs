using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXControl : MonoBehaviour
{
    public ParticleSystem parrySpark;
    public ParticleSystem blockSpark;
    public ParticleSystem lighAttackTrail;
    public ParticleSystem heavyAttackTrail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySpark()
    {
        blockSpark.Play();
    }
    public void PlayParrySpark()
    {
        parrySpark.Play();
    }

    public void PlayLightAttack()
    {
        lighAttackTrail.Play();
    }

    public void PlayHeavyAttack()
    {
        heavyAttackTrail.Play();
    }
}
