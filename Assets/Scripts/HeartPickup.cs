using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int healAmount = 1; // Amount of health restored

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            TankHealth tankHealth = other.GetComponent<TankHealth>();

            if (tankHealth != null)
            {
                if (tankHealth.CanHeal()) //  Only heal if health is not full
                {
                    tankHealth.Heal(healAmount);
                    Destroy(gameObject); //  Remove the heart after collection
                }
            }
        }
    }
}

