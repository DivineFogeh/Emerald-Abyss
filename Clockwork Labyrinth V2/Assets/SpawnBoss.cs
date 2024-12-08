using System.Collections;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public static SpawnBoss Instance;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject boss;
    [SerializeField] Vector2 exitDirection;
    bool callOnce;
    BoxCollider2D col;

    private void Awake()
    {
        // Initialize BoxCollider2D
        col = GetComponent<BoxCollider2D>();

        // Handle destruction of existing instance
        if (TheHollowKnight.Instance != null)
        {
            Destroy(TheHollowKnight.Instance);
            callOnce = false;

            // Ensure the collider is a trigger
            col.isTrigger = true;
        }

        // Check if the boss has been defeated
        if (GameManager.Instance.THKDefeated)
        {
            callOnce = true;
        }

        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Additional setup if required in Start()
        // Kept for future logic or additional checks
    }

    void Update()
    {
        // Empty Update method; you can remove it if not used
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        // Check if the player enters the trigger and the boss has not been defeated
        if (_other.CompareTag("Player") && !callOnce && !GameManager.Instance.THKDefeated)
        {
            StartCoroutine(WalkIntoRoom());
            callOnce = true;
        }
    }

    IEnumerator WalkIntoRoom()
    {
        // Player walks into the new scene
        StartCoroutine(PlayerController.Instance.WalkIntoNewScene(exitDirection, 1));
        PlayerController.Instance.pState.cutscene = true;

        // Wait for a moment before spawning the boss
        yield return new WaitForSeconds(1f);
        col.isTrigger = false;
        Instantiate(boss, spawnPoint.position, Quaternion.identity);
    }

    public void IsNotTrigger()
    {
        // Make the collider a trigger
        col.isTrigger = true;
    }
}
