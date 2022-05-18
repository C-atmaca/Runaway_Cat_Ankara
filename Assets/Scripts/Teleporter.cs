using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform teleportPoint;

    [SerializeField] private bool isAdvancedLevel2;
    [SerializeField] private GameObject level2Camera;

    [SerializeField] private bool isAdvancedLevel4;
    [SerializeField] private GameObject level4Camera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = teleportPoint.position;
            if (isAdvancedLevel2)
            {
                level2Camera.SetActive(false);
                Invoke("EnableCamera2", 0.7f);
            }
            else if (isAdvancedLevel4)
            {
                level4Camera.SetActive(false);
                Invoke("EnableCamera3", 0.7f);
            }
        }
    }

    private void EnableCamera2()
    {
        level2Camera.SetActive(true);
    }

    private void EnableCamera3()
    {
        level4Camera.SetActive(true);
    }
}
