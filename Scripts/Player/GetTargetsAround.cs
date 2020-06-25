using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTargetsAround : MonoBehaviour
{
    public float radius;

    public float dist;
    public float absluteDist;

    public int targetIndex;

    public CharacterMovement characterMovement;

    public List<Enemy> enemiesOnRange;

    // Start is called before the first frame update
    void Start()
    {
        dist = Mathf.Infinity; 
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.IsPlayable())
        {
            GetTargetInRadius();
            RemoveTargetFromList();

            GetScrollInput();
        }

       

           

    }

    void  GetTargetInRadius()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < hitColliders.Length; i ++)
        {

            Enemy en = hitColliders[i].GetComponent<Enemy>();

            if (en != null)
            {
                if (!enemiesOnRange.Contains(en))
                {
                    if (en.IsPlayerVisibleToEnemy())
                    {
                        enemiesOnRange.Add(en);
                    }

                }

            }
        }


    }

    void RemoveTargetFromList()
    {
        for (int i = 0; i < enemiesOnRange.Count; i++)
        {
            if (enemiesOnRange[i] == null)
            {
                enemiesOnRange.Remove(enemiesOnRange[i]);
                return;
            }
            float myDist = Vector3.Distance(enemiesOnRange[i].transform.position, transform.position);

            if (myDist <= dist && !characterMovement.isLockedOn)
            {
                characterMovement.bestTarget = enemiesOnRange[i].transform;

            }

            dist = myDist;


            if (myDist > radius || !enemiesOnRange[i].IsPlayerVisibleToEnemy())
            {
                if (characterMovement.bestTarget != null)
                {
                    if (enemiesOnRange[i].name == characterMovement.bestTarget.name)
                    {
                        characterMovement.bestTarget = null;
                    }
                }

                enemiesOnRange.Remove(enemiesOnRange[i]);

            }


        }
    }

    void GetScrollInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") >= 0.1f)
        {
            if (targetIndex >= enemiesOnRange.Count - 1)
            {
                targetIndex = 0;
            }
            else
            {
                targetIndex++;
            }

            AssignCycledTargets();
            characterMovement.AssignBestTargetToTarget();
        }

        if (Input.GetAxis("Mouse ScrollWheel") <= -0.1f)
        {
            if (targetIndex <= 0)
            {
                targetIndex = enemiesOnRange.Count - 1;
            }
            else
            {
                targetIndex--;
            }

            AssignCycledTargets();
            characterMovement.AssignBestTargetToTarget();
        }

    }

    public void AssignCycledTargets()
    {
        
        if (enemiesOnRange.Count != 0 && targetIndex < enemiesOnRange.Count)
        {
            characterMovement.bestTarget = enemiesOnRange[targetIndex].transform;
            Debug.Log("New Best Target: " + enemiesOnRange[targetIndex].transform);
        }

    }

}
