using System.Collections;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private bool isInvincible = false;
    public float invincibilityDuration = 2f; // How long the tank is immune
    public float blinkInterval = 0.2f; // Speed of blinking

    private SpriteRenderer[] spriteRenderers; // Store all sprite renderers

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(); // Get all child sprites
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Ignore damage if currently invincible

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage! Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyTank();
        }
        else
        {
            StartCoroutine(BecomeInvincible());
        }
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            ToggleSpriteVisibility(false); // Hide all sprites
            yield return new WaitForSeconds(blinkInterval);
            ToggleSpriteVisibility(true); // Show all sprites
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval * 2;
        }

        ToggleSpriteVisibility(true); // Ensure tank is visible after blinking
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
        Destroy(gameObject);
    }
}
