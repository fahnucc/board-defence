using System.Collections.Generic;
using UnityEngine;

public class DefenceItemController : MonoBehaviour
{
    private DefenceItemData data;
    private float attackCooldownTimer;
    private float worldRange;
    private int myX;
    private int myY;
    private GridManager gridManager;

    public void Initialize(DefenceItemData itemData, int cellX, int cellY)
    {
        this.data = itemData;
        this.myX = cellX;
        this.myY = cellY;
        this.attackCooldownTimer = 0f;

        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        worldRange = data.range * gridManager.GetCellSize();
    }

    void Update()
    {
        if (data == null) return;

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (attackCooldownTimer <= 0)
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, worldRange, LayerMask.GetMask("Enemy"));
        if (enemiesInRange.Length == 0) return;

        Transform closestTarget = null;
        float minDistance = float.MaxValue;

        List<Transform> validTargets = new List<Transform>();

        foreach (var enemyCollider in enemiesInRange)
        {
            Vector2Int enemyCoords = gridManager.WorldToGrid(enemyCollider.transform.position);

            bool isDirectionValid = false;
            if (data.direction == AttackDirection.Forward)
            {
                if (enemyCoords.x == myX && enemyCoords.y > myY)
                {
                    isDirectionValid = true;
                }
            }
            else if (data.direction == AttackDirection.All)
            {
                if ((enemyCoords.x == myX && enemyCoords.y != myY) || (enemyCoords.y == myY && enemyCoords.x != myX))
                {
                    isDirectionValid = true;
                }
            }

            if (isDirectionValid)
            {
                validTargets.Add(enemyCollider.transform);
            }
        }

        if (validTargets.Count == 0) return;

        foreach (var validTarget in validTargets)
        {
            float distance = Vector3.Distance(transform.position, validTarget.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = validTarget;
            }
        }

        if (closestTarget != null)
        {
            Attack(closestTarget);
            attackCooldownTimer = data.interval;
        }
    }

    private void Attack(Transform target)
    {
        if (data.projectilePrefab != null)
        {
            // GameObject projectileObj = Instantiate(data.projectilePrefab, transform.position, Quaternion.identity);
            GameObject projectileObj = ObjectPool.Instance.SpawnFromPool(data.projectilePrefab.name, transform.position, Quaternion.identity);
            Projectile projectileScript = projectileObj.GetComponent<Projectile>();
            
            if (projectileScript != null)
            {
                projectileScript.Initialize(target, data.projectileSpeed * gridManager.GetCellSize(), data.damage);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, worldRange);
    }
}