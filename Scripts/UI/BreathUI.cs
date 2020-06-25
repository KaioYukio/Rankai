using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathUI : MonoBehaviour
{
    public Breath breath;

    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = breath.breathPoints / breath.maxBreathPoints;
    }
}
