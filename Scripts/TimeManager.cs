using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    public float t = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
     //   DoSlowMotion();
    }

    private void Update()
    {
        //DoSlowMotion();
        if (Input.GetKeyDown(KeyCode.T))
        {
            DoSlowMotion();
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = t * Time.timeScale;
    }

}
