using System.Collections;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private bool isInvincible = false;
    public float invincibilityDuration = 2f;
    public float blinkInterval = 0.2f;

    private SpriteRenderer[] spriteRenderers;
    public GameObject arrow; // Assign the arrow in the Inspector
    public GameObject[] hearts; // Assign the heart GameObjects in the Inspector

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage! Health: " + currentHealth);

        UpdateHearts(); // Hide a heart when taking damage

        if (currentHealth <= 0)
        {
            DestroyTank();
        }
        else
        {
            StartCoroutine(BecomeInvincible());
        }
    }

    void UpdateHearts()
    {
        // Disable a heart GameObject when losing health
        if (currentHealth < hearts.Length && currentHealth >= 0)
        {
            hearts[currentHealth].SetActive(false); // Hide the last active heart
        }
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            ToggleSpriteVisibility(false);
            yield return new WaitForSeconds(blinkInterval);
            ToggleSpriteVisibility(true);
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval * 2;
        }

        ToggleSpriteVisibility(true);
        isInvincible = false;
    }

    void ToggleSpriteVisibility(bool visible)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = visible;
        }
    }

    void DestroyTank()
    {
        Debug.Log(gameObject.name + " has been destroyed!");

        // Turn off the arrow when the tank is destroyed
        if (arrow != null)
        {
            arrow.SetActive(false);
        }

        Destroy(gameObject);
    }
}
