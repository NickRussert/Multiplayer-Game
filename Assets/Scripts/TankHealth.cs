using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // Set full health at start
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage! Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyTank(); // Destroy the tank when health reaches 0
        }
    }

    void DestroyTank()
    {
        Debug.Log(gameObject.name + " has been destroyed!");
        Destroy(gameObject); // Destroy the tank
    }
}

