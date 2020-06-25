using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaColliderEnemy : MonoBehaviour
{
    public Collider col;

    public Enemy enemy;

    public KatanaColliderEnemy katanaColliderEnemy;

    public bool isLightAttack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();

            if (characterMovement != null)
            {
                characterMovement.TakeDamage(other, enemy, enemy.damage, katanaColliderEnemy); // other serve para pegar o ponto onde a espada do inimigo acertou o jogador; enemy serve para causar o stagger no inimigo que atacou durante o parry do player
                col.enabled = false;
            }
        }
    }
}
