using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health playerHealth;
    public Image img;
    public Image bgImg;

    public float a;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.cutscene)
        {
            a = 0;
            img.color = new Color(img.color.r, img.color.g, img.color.b, a);
            bgImg.color = new Color(bgImg.color.r, bgImg.color.g, bgImg.color.b, a);
        }
        else
        {
            if (a != 255)
            {
                a = 255;
                img.color = new Color(img.color.r, img.color.g, img.color.b, a);
                bgImg.color = new Color(bgImg.color.r, bgImg.color.g, bgImg.color.b, a);
            }
        }

        img.fillAmount = playerHealth.health / playerHealth.maxHealth;
    }
}
