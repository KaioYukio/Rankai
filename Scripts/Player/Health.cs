using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public SkinnedMeshRenderer taroRenderer;
    public float health;
    public float maxHealth;
    public bool isDead;
    public Animator an;
    public CharacterMovement characterMovement;
    public ParticleSystem vidaVFX;

    public AudioSource audioSource;
    public AudioClip healthUP;
    public AudioClip morte;

    public float i;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //an = GetComponent<Animator>();
        GameManager.instance.ReAssign();
        i = 0.1f;
        taroRenderer.material.SetFloat("OpacityBlood", i);

    }

    // Update is called once per frame
    void Update()
    {
        //health = 100;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.morto);
            audioSource.PlayOneShot(morte);
            //health = maxHealth;
            an.SetTrigger("Die");

        }
        else if (health <= 0)
        {
            PlayerStates.instance.MudarEstadoPara(PlayerStates.instance.morto);
            characterMovement.StopForce();

        }
        else
        {
            //if (health > 100)
            //{
            //    health = 100;
            //}
        }
    }

    public void Heal()
    {
        vidaVFX.Play();
        health += 10;
        audioSource.PlayOneShot(healthUP);
        UpdateBlood();
    }

    public void RestartGame()
    {
        i = 0.1f;
        taroRenderer.material.SetFloat("OpacityBlood", i);
        health = maxHealth;
        isDead = false;
        GameManager.instance.RestartGame();

    }

    public void UpdateBlood()
    {
        i += 0.3f;
        taroRenderer.material.SetFloat("OpacityBlood", i);
    }
}
