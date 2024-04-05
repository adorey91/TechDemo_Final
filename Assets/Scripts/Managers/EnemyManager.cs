using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    GameManager gameManager;
    Enemy enemy;
    public GameObject enemyPrefab;
    public GameObject arena;
    int enemyMultiplier = 2;
    int level = 1;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }


    private void Update()
    {
        if (gameManager.state == GameManager.GameState.Gameplay)
        {
            int numEnemiesToSpawn = level * enemyMultiplier;
            enemy = FindAnyObjectByType<Enemy>();
            if (enemy == null)
            {
                for (int i = 0; i < numEnemiesToSpawn; i++)
                {
                    Instantiate(enemyPrefab, arena.transform);
                }
                level++;
            }
        }
    }
}