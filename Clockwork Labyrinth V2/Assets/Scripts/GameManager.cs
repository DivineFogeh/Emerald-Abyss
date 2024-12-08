using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;

    public Vector2 platformingRespawnPoint;
    public Vector2 respawnPoint;
    [SerializeField] Vector2 defaultRespawnPoint;
    [SerializeField] Bench bench;

    [SerializeField] private FadeUI pauseMenu;
    [SerializeField] private float fadeTime;
    public bool gameIsPaused;

    public bool THKDefeated = false;
    
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        SaveData.Instance.Initialize();
        
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        SaveScene();
        
        DontDestroyOnLoad(gameObject);

        bench = FindObjectOfType<Bench>();

        SaveData.Instance.LoadBossData();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SaveData.Instance.SavePlayerData();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && !gameIsPaused)
        {
            pauseMenu.FadeUIIn(fadeTime);
            Time.timeScale = 0;
            gameIsPaused = true;
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
    }
    public void SaveScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SaveData.Instance.sceneNames.Add(currentSceneName);
    }

    public void SaveGame()
    {
        SaveData.Instance.SavePlayerData();
    }
    public void RespawnPlayer()
    {
        SaveData.Instance.LoadBench();
        if(SaveData.Instance.benchSceneName != null) //load the bench's scene if it exists.
        {
            SceneManager.LoadScene(SaveData.Instance.benchSceneName);
        }

        if(SaveData.Instance.benchPos != null) //set the respawn point to the bench's position.
        {
            respawnPoint = SaveData.Instance.benchPos;
        }
        else
        {
            respawnPoint = defaultRespawnPoint;
        }

        PlayerController.Instance.transform.position = respawnPoint;

        StartCoroutine(UIManager.Instance.DeactivateDeathScreen());
        PlayerController.Instance.Respawned();
    }
}
