using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPai : MonoBehaviour
{
    public GameObject[] tutorial;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeTutorialGO()
    {
        tutorial[index].SetActive(false);
        if (index < tutorial.Length - 1)
        {
            index++;
        }

        tutorial[index].SetActive(true);
    }
}
