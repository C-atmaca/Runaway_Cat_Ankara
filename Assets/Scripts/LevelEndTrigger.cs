using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public event Action level1Done;
    public event Action level2Done;
    public event Action level3Done;
    public event Action level4Done;

    [SerializeField] private int currentLevel;
    [SerializeField] private int nextLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentLevel == 1)
            {
                if (level1Done != null)
                {
                    level1Done();
                }
            }
            else if (currentLevel == 2)
            {
                if (level2Done != null)
                {
                    level2Done();
                }
            }
            else if (currentLevel == 3)
            {
                if (level3Done != null)
                {
                    level3Done();
                }
            }
            else if (currentLevel == 4)
            {
                if (level4Done != null)
                {
                    level4Done();
                }
            }
        }
    }
}
