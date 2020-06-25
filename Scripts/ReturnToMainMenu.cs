using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public GameObject targetIndicator;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.GoToMenu();
        Destroy(targetIndicator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
