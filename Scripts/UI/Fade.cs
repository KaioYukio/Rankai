using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image image;
    public float alpha;
    public float multilpier;
    public Transform fadePos;
    public Transform playerTr;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        while(alpha < 1)
        {
            alpha += Time.deltaTime * multilpier;
            image.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        playerTr.position = fadePos.position;
        Debug.Log("Pos");

        yield return new WaitForSeconds(1);
         
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * multilpier;
            image.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

    }
}
