using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public Enemy[] enemys;
    public bool isDone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsWaveDone();
    }

    public void GoWave()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].canAct = true;
        }
    }

    public void IsWaveDone()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i])
            {
                return;
            }
            else
            {
                isDone = true;
            }
        }
    }
}
