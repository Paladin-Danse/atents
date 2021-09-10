using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform[] spawnPoints;

    public float damageMax = 40f;
    public float damageMin = 20f;

    public float healthMax = 200f;
    public float healthMin = 100f;

    public float speedMax = 3f;
    public float speedMin = 1f;

    public Color strongEnemyColor = Color.red;

    private List<Enemy> enemies = new List<Enemy>();
    private int wave;

    private void Update()
    {
        if(GameManager.instance != null && GameManager.instance.isGameOver)
        {
            return;
        }
        if(enemies.Count <= 0)
        {
            SpawnWave();
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        UIManager.instance.UpdateWaveText(wave, enemies.Count);
    }

    private void SpawnWave()
    {
        wave++;
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        for(int i = 0; i < spawnCount; i++)
        {
            float enemyIntensity = Random.Range(0f, 1f);
            CreateEnemy(enemyIntensity);
        }
    }

    private void CreateEnemy(float intensity)
    {
        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.Setup(health, damage, speed, skinColor);

        enemies.Add(enemy);

        //적의 onDeath이벤트에 익명 메서드 등록
        enemy.OnDeath += () => enemies.Remove(enemy);
        enemy.OnDeath += () => Destroy(enemy.gameObject, 10f);
        enemy.OnDeath += () => GameManager.instance.AddScore(100);
    }
}
