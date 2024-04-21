using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public InputActionAsset actions;
    public GameObject playerSpawnPoint;
    public Collider2D arenaCollider;
    public GameObject playerPrefab;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;
    //public TextMeshProUGUI waveUI, scoreUI, livesUI, levelUI;
    //public RectTransform pausePanel, gameOverPanel,
    public RectTransform debugUI;
    //public bool isPaused, isGameOver;
    //public AudioClip gameMusic;

    private PlayerController playerController;
    private bool isDebug;
    private InputActionMap gameplayActions;
    //private InputAction pauseAction, debugAction;
    private InputAction debugAction;
    //public void PauseGame()
    //{
    //    if (isGameOver) return;
    //    pausePanel.gameObject.SetActive(true);
    //    isPaused = true;
    //    Time.timeScale = 0;
    //}

    //public void ResumeGame()
    //{
        //isPaused = false;
        //Time.timeScale = 1;
        //pausePanel.gameObject.SetActive(false);
    //}

    //public void NewGame()
    //{
    //    Scene scene = SceneManager.GetActiveScene();
    //    SceneManager.LoadScene(scene.name);
    //    Time.timeScale = 1;
    //}

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void Awake()
    {
        actions.Enable();

        gameplayActions = actions.FindActionMap("Gameplay");
        //pauseAction = gameplayActions.FindAction("PlayerPause");
        debugAction = gameplayActions.FindAction("PlayerDebug");
        //pauseAction.performed += OnPause;
        debugAction.performed += OnDebug;
    }

    private void Start()
    {
        InitGame();
        //StartLevel(currentLevelNum);
    }

    private void Update()
    {
        if (isDebug)
        {
            debugUI.GetComponentInChildren<TextMeshProUGUI>().text = GetDebugText();
        }
    }

    private void OnDestroy()
    {
        //pauseAction.performed -= OnPause;
        //pauseAction.performed -= OnDebug;
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    //private void OnPause(InputAction.CallbackContext context)
    //{
    //    if (!isPaused)
    //    {
    //        PauseGame();
    //    }
    //    else
    //    {
    //        ResumeGame();
    //    }
    //}

    private void OnDebug(InputAction.CallbackContext context)
    {
#if DEBUG
        if (debugAction.triggered)
        {
            isDebug = !isDebug;
            debugUI.gameObject.SetActive(isDebug);
        }
#endif
    }

    private void InitGame()
    {
        //pausePanel.gameObject.SetActive(false);
        //gameOverPanel.gameObject.SetActive(false);
        debugUI.gameObject.SetActive(false);
        SpawnPlayer();
        //isGameOver = false;
        
        //UpdateScoreUI();
        //UpdateLivesUI();
    }

    private void SpawnPlayer()
    {
        
        // Player spawns from bottom spawner
        GameObject spawnPoint = playerSpawnPoint;
        Vector2 spawnPosition = spawnPoint.transform.position;

        GameObject player = GameObject.Find("Player");
        if (player)
        {
            player.transform.position = spawnPosition;
            player.transform.rotation = playerPrefab.transform.rotation;
        }
        else
        {
            Instantiate(playerPrefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), playerPrefab.transform.rotation).name = "Player";
        }
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //private IEnumerator GameOverCoroutine()
    //{
    //    isGameOver = true;
    //    yield return new WaitForSeconds(1.0f);
    //    gameOverPanel.gameObject.SetActive(true);
    //    Time.timeScale = 0;
    //}

    //private void UpdateScoreUI()
    //{
    //    scoreUI.text = score.ToString();
    //}

    //private void UpdateWaveUI()
    //{
    //    waveUI.text = currentWaveNum.ToString();
    //}
    //private void UpdateLivesUI()
    //{
    //    livesUI.text = numPlayerLives.ToString();
    //}

    //private void UpdateLevelUI()
    //{
    //    levelUI.text = currentLevelNum.ToString();
    //}

    private string GetDebugText()
    {
        return $"Level:            hello world";
        //       $"Wave:             {currentWaveNum}\\{(levels != null ? levels[currentLevelNum - 1].Waves.Count : "NA")}\n" +
        //       $"Enemies in wave:  {numEnemiesLeftInWave}\n" +
        //       $"Lives:            {numPlayerLives}\n" +
        //       $"Score:            {score}\n" +
        //       $"Is invulnerable:  {(playerController ? playerController.isInvulnerable : "NA")}\n" +
        //       $"Is paused:        {isPaused}\n" +
        //       $"Is game over:     {isGameOver}\n" +
        //       $"Aim vector:{(playerController ? gameplayActions.FindAction("PlayerAim").ReadValue<Vector2>() : "NA")}\n" +
        //       $"Move vector: {(playerController ? gameplayActions.FindAction("PlayerMove").ReadValue<Vector2>() : "NA")}\n" +
        //       $"Is mouse aim: {(playerController ? playerController.isMouseAim : "NA")}\n";
    }
}

