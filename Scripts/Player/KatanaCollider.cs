using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaCollider : MonoBehaviour
{
    public Collider col;
    public CharacterAttack characterAttack;

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
        if (other.tag == "Enemy")
        {
            Enemy en = other.GetComponent<Enemy>();

            if (en != null)
            {
                if (characterAttack.perfomedHeavyAttack)
                {
                    en.TakeDamage(2);
                }
                else 
                {
                    en.TakeDamage(1);
                }

                col.enabled = false;
            }
        }
        else if (other.CompareTag("DestroyWall"))
        {
            Destroy(other.gameObject);
            col.enabled = false;
        }

    }


    public void OnCollisionEnter(Collision collision)
    {

    }
}
