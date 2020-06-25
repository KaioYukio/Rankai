using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
    public bool hidetoraPassed;
    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hidetora"))
        {
            hidetoraPassed = true;
        }
        else if (other.CompareTag("Player") && hidetoraPassed)
        {
            fade.SetActive(true);
        }
    }
}
