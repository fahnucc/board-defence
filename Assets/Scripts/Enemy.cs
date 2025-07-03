using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData enemyData;
    private float moveSpeedInUnits;
    public float currentHealth;
    Action onDieCallback;

    public void Initialize(EnemyData data, float worldCellSize, Action onDieCallback)
    {
        this.enemyData = data;
        this.currentHealth = data.health;
        this.moveSpeedInUnits = data.speed * worldCellSize;
        this.onDieCallback = onDieCallback;
    }

    void Update()
    {
        if (enemyData == null || !gameObject.activeSelf) return;

        transform.Translate(-Vector3.forward * moveSpeedInUnits * Time.deltaTime, Space.World);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        onDieCallback?.Invoke();
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
