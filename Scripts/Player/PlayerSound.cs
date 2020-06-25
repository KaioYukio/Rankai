using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip blink;

    public AudioClip block;

    public AudioClip parry;

    public AudioClip hurt;
    public AudioClip[] hurtVoice;

    public AudioClip[] hit;

    public AudioClip[] step;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBlink()
    {
        audioSource.PlayOneShot(blink);
    }

    public void PlayBlock()
    {
        audioSource.PlayOneShot(block);
    }

    public void PlayParry()
    {
        audioSource.PlayOneShot(parry);
    }

    public void PlayHurt()
    {
        audioSource.PlayOneShot(hurt);

        int rand = Random.Range(0, hurtVoice.Length);

        audioSource.PlayOneShot(hurtVoice[rand]);
    }

    public void PlayHitKatana()
    {
        int rand = Random.Range(0, hit.Length);

        audioSource.PlayOneShot(hit[rand]);
    }

    public void PlayStep()
    {
        int rand = Random.Range(0, step.Length);

        audioSource.PlayOneShot(step[rand]);
    }
}
