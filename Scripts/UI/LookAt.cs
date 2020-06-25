using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform playerTR;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTR == null)
        {
            playerTR = GameObject.FindGameObjectWithTag("Player").transform;

        }

        transform.LookAt(playerTR);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
