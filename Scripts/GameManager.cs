using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Vector3 lastPlayerPos;
    public GameObject player;
    public Health playerHealth;
    public Animator playerAnimator;
    public PlayerStates playerStates;
    public WaveMannager waveManager;

    public enum GameState { pause, cutscene, gameplay };
    public GameState gameState;

    public GameObject pauseUI;
    public GameObject audioUI;
    public GameObject comoJogarUI;
    public GameObject cameraFather;
    public GameObject enemyPreFab;


    public PlayableDirector playable1;
    public bool isInCutScene;

    //public post;
    Vignette vignette;

    public PitchChanger musicPitchChanger;

    [Header("SlowMotion")]
    public float hitStopDuration;
    private bool waiting;
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    public float t = 0.02f;
    public float gameSpeed;
    public float speedFactor;
    public bool isSlow;
    private bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameState = GameState.gameplay;
        pauseUI.SetActive(false);
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //isInCutScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        // postProcess = Post
 

        HandleInputs();
    }

    public void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState != GameState.cutscene)
            {
                PauseSwitch();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Instantiate(enemyPreFab, transform.position, Quaternion.identity);
        }
    }

    void PauseSwitch()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            gameState = GameState.pause;
            pauseUI.SetActive(true);
            cameraFather.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            gameState = GameState.gameplay;
            pauseUI.SetActive(false);
            audioUI.SetActive(false);
            comoJogarUI.SetActive(false);
            cameraFather.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public bool IsPlayable()
    {
        if (gameState == GameState.gameplay)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DoSlowMotion()
    {
        if (!isSlow)
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = t * Time.timeScale;
            gameSpeed = speedFactor;
            musicPitchChanger.DownPitch();
        }
        else
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02F;
            gameSpeed = 1;
            musicPitchChanger.RisePitch();
        }

        isSlow = !isSlow;

    }

    public void DoHitStop()
    {
        if (waiting)
        {
            return;
        }
        Time.timeScale = 0f;
        Time.fixedDeltaTime = t * Time.timeScale;
        StartCoroutine(HitStop());

    }

    IEnumerator HitStop()
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F;
        waiting = false;

    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameState = GameState.gameplay;
        cameraFather.SetActive(true);
        isPaused = false;
    }

    public void RestartGame()
    {

        SceneManager.LoadScene(1);
        gameState = GameState.gameplay;
        Invoke("MakeGameplay", 1);
        playerStates.estadoAtual = playerStates.livre;
        playerAnimator.Play("Standing Idle", 0, 0);
        HideCursor();
        pauseUI.SetActive(false); // LEMBRAR DE DEIXAR O PAUSE UI GAME OBJECT ATIVADO NA BUILD FINAL
        player.transform.position = lastPlayerPos;

    }


    void MakeGameplay()
    {
        gameState = GameState.gameplay;
    }

    public void ReAssign()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraFather = GameObject.FindGameObjectWithTag("MainCamera");
        if (pauseUI == null)
        {
            pauseUI = GameObject.FindGameObjectWithTag("Pause");
        }

        HideCursor();
        pauseUI.SetActive(false); // LEMBRAR DE DEIXAR O PAUSE UI GAME OBJECT ATIVADO NA BUILD FINAL
        player.transform.position = lastPlayerPos;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}
