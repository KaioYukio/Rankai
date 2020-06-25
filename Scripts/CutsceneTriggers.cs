using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTriggers : MonoBehaviour
{

    public Animator playerAn;
    public bool used;

    public PlayableDirector cutscene;

    

    // Start is called before the first frame update
    void Start()
    {
        used = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (used)
        {
            GameManager.instance.gameState = GameManager.GameState.gameplay;

            Destroy(cutscene.gameObject);
            //Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !used)
        {
            //if  (playerAn == null)
            //{
            //    playerAn = GameObject.FindGameObjectWithTag("TaroApose").GetComponent<Animator>();
            //}


            //cutscene.Play();
            if (cutscene != null)
            {
                playerAn.Play("Standing Idle", 0, 0);
                cutscene.gameObject.SetActive(true);
                used = true;
                GameManager.instance.gameState = GameManager.GameState.cutscene;
            }

            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Player") && used)
        {
            GameManager.instance.gameState = GameManager.GameState.gameplay;
        }
    }
}
