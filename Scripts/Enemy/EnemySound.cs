using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip block;
    public AudioClip parry;

    public AudioClip hurt;
    public AudioClip[] hurtVoice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
