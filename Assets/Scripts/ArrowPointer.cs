using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform tank; // The tank this arrow belongs to
    private Transform enemyTank; // The opponent tank

    void Start()
    {
        FindEnemyTank();
    }

    void Update()
    {
        if (enemyTank == null || !enemyTank.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false); // Hide the arrow when the enemy is destroyed
            return;
        }

        // Calculate direction to the enemy
        Vector3 direction = enemyTank.position - tank.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void FindEnemyTank()
    {
        if (tank.CompareTag("Player1"))
        {
            GameObject enemy = GameObject.FindWithTag("Player2");
            enemyTank = enemy != null ? enemy.transform : null;
        }
        else if (tank.CompareTag("Player2"))
        {
            GameObject enemy = GameObject.FindWithTag("Player1");
            enemyTank = enemy != null ? enemy.transform : null;
        }
    }
}

