using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEventsFromEnemyAnimation : MonoBehaviour
{
    public Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyScript == null)
        {
            return;
            //Destroy(gameObject);
        }
    }

    public void Attack()
    {
        enemyScript.Attack();
    }

    public void ChanceToHeavyAttack()

    {
        enemyScript.ChanceToHeavyAttack();
    }

    public void RotationToTarget()
    {
        enemyScript.RotationToTarget();
    }

    void ExitBlocking()
    {
        enemyScript.ExitBlocking();
    }

    public void DoParry()
    {
        enemyScript.DoParry();
    }

    public void MakeColliderOn()
    {
        enemyScript.MakeColliderOn();
    }

    public void MakeColliderOff()
    {
        enemyScript.MakeColliderOff();
    }

    public void ReturnToIdle()
    {
        enemyScript.ReturnToIdle();
    }

    public void AddForceForward()
    {
        enemyScript.AddForceForward();
    }

    public void AddForceFlinch()
    {
        enemyScript.AddForceFlinch();
    }

    public void StopForce()
    {
        if (enemyScript.rb == null)
        {
            return;
        }
        enemyScript.rb.isKinematic = true;
    }

    public void Morte()
    {
        enemyScript.Morte();
    }

}
