using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string transitionTo; //Represents the scene to transition to

    [SerializeField] private Transform startPoint; //Defines the player's entry point in the scene

    [SerializeField] private Vector2 exitDirection; //Specifies the direction for the player's exit

    [SerializeField] private float exitTime; //Determines the time it takes for the player to exit the scene transition

    // Start is called before the first frame update

    private bool isTransitioning = false;
    private void Start()
    {
        if(GameManager.Instance.transitionedFromScene == transitionTo)
        {
            PlayerController.Instance.transform.position = startPoint.position;

            PlayerController.Instance.transform.position += (Vector3)(exitDirection.normalized * 0.5f);

            StartCoroutine(PlayerController.Instance.WalkIntoNewScene(exitDirection, exitTime));
        }
                   StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            GameManager.Instance.transitionedFromScene = SceneManager.GetActiveScene().name;

            PlayerController.Instance.pState.cutscene = true;
            PlayerController.Instance.pState.invincible = true;

            StartCoroutine(UIManager.Instance.sceneFader.FadeAndLoadScene(SceneFader.FadeDirection.In, transitionTo));
        }
    }
}