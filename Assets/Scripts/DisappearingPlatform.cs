using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    private bool enabled = true;
    private Animator anim;

    [SerializeField] private float platformLifeTime = 0.5f;


    void Start()
    {
        enabled = true;
        anim = GetComponent<Animator>();
    }

    private void TogglePlatform()
    {
        enabled = !enabled;
        gameObject.GetComponent<BoxCollider2D>().enabled = enabled;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            anim.SetTrigger("Break");
            Invoke("TogglePlatform", platformLifeTime);
            Invoke("TogglePlatform", 1.5f);
        }
    }


}
