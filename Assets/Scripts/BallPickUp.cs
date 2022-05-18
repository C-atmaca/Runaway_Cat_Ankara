using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPickUp : MonoBehaviour
{
    private PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.ActivateGrapple();
            Destroy(gameObject);
        }
    }
}
