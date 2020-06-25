using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialTrigger : MonoBehaviour
{
    public Animator tutorialAn;

    public float timeToDestroy;

    public bool isUsed;

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
        if (other.CompareTag("Player") && !isUsed)
        {
            isUsed = true;
            tutorialAn.SetTrigger("GO");
        }
    }
}
