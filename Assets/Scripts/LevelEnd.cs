using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private GameObject finalText;
    private readonly WaitForSeconds secondsToWait = new WaitForSeconds(3);

    private void Start()
    {
        finalText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Player");
            StartCoroutine(EndLevel());
        }
    }
    
    private IEnumerator EndLevel()
    {
        finalText.SetActive(true);
        yield return secondsToWait;
        SceneManager.LoadScene(0);
    }
}
