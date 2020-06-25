using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    //static int levelToLoad;
    public int levelToLoad;
    public Image img;

    AsyncOperation op;

    public Image circle;

    public GameObject levelLoaded;
    public bool passed;
    AsyncOperation async;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("StartLoading", 0.5f);
        //StartLoading();
    }

    // Update is called once per frame
    void Update()
    {
        if (op != null)
        {
            //img.fillAmount = op.progress / 1;
            Debug.Log("Op:" + op.progress);
            if (op.progress >= 0.9f)
            {
                levelLoaded.SetActive(true);
            }

            Debug.Log("Loading...");
        }


    }
    public void StartLoading()
    {

        Invoke("Load", 2f);
        //op.allowSceneActivation = false;
    }
    public void Load()
    {
        op = SceneManager.LoadSceneAsync(1);
        //op.allowSceneActivation = false;
    }


    public void StartGame()
    {


        if (op != null)
        {
            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }

        }
    }

    public static void LoadLevel(int lv)
    {
        //levelToLoad = lv;
        //SceneManager.LoadScene("LoadScene");
    }
}
