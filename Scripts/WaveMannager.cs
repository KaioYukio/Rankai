using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMannager : MonoBehaviour
{
    public Enemy[] enemies;
    public int enemiesActive;
    public int enemiesDead;
    public int index;
    public GameObject cutscene1AndarFim;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (enemiesDead >= enemies.Length)
        {
            if (cutscene1AndarFim != null && !isActive)
            {
                isActive = true;
                Invoke("MakeCutscene3On", 3);
            }

            return;
        }

        //if (enemiesActive < 3)
        //{
        //    enemies[index].canAct = true;
        //    enemiesActive++;

        //    if (index < enemies.Length - 1)
        //    {
        //        index++;
        //    }

        //}



    }

    public void MakeCutscene3On()
    {
        cutscene1AndarFim.SetActive(true);
    }

    public void SubtractAcive()
    {
        enemiesActive--;
        enemiesDead++;
    }
}
