using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadEndGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadEndScene();
        }
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene(2);
        SceneManager.LoadScene(4);
    }
}
