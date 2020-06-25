using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchChanger : MonoBehaviour
{
    public AudioSource audioSource;
    public float maxPitch;
    public float minPitch;
    public float actualPitch;
    public float pitchModifier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RisePitch()
    {
        StopAllCoroutines();

        StartCoroutine(RiserPitch());
    }

    public void DownPitch()
    {
        StopAllCoroutines();

        StartCoroutine(DownerPitch());
    }

    IEnumerator RiserPitch()
    {
        while (actualPitch < maxPitch)
        {
            actualPitch += pitchModifier * Time.deltaTime;
            audioSource.pitch = actualPitch;

            yield return null;
        }
    }

    IEnumerator DownerPitch()
    {
        while (actualPitch > minPitch)
        {
            actualPitch -= pitchModifier * Time.deltaTime;
            audioSource.pitch = actualPitch;

            yield return null;
        }
    }
}
