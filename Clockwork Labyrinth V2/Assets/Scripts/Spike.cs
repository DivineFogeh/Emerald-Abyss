using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D _other) 
    {
          if(_other.CompareTag("Player"))
          {
            StartCoroutine(RespawnPoint());
          } 
    }

    IEnumerator RespawnPoint()
    {
        PlayerController.Instance.pState.cutscene = true;
        PlayerController.Instance.pState.invincible = true;
        PlayerController.Instance.rb.velocity = Vector2.zero;
        Time.timeScale = 0f;
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        PlayerController.Instance.TakeDamage(1);
        yield return new WaitForSecondsRealtime(1f);
        PlayerController.Instance.transform.position = GameManager.Instance.platformingRespawnPoint;
        StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
        yield return new WaitForSecondsRealtime(UIManager.Instance.sceneFader.fadeTime);
        PlayerController.Instance.pState.cutscene = false;
        PlayerController.Instance.pState.invincible = false;
        Time.timeScale = 1;

    }
}