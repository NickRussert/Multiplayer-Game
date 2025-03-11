using System.Collections;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private bool isInvincible = false;
    public float invincibilityDuration = 2f;
    public float blinkInterval = 0.2f;
    public float explosionDuration = 0.5f; // Explosion duration before destruction

    private SpriteRenderer[] spriteRenderers;
    private SpriteRenderer mainSpriteRenderer;
    private Sprite originalSprite; // Store original tank sprite

    public GameObject arrow;
    public GameObject[] hearts; // ✅ Heart UI array (Assign in Inspector)

    public AudioClip hitSound; // Assign in Inspector
    public AudioClip explosionSound; // Assign in Inspector
    public AudioClip pickupSound; // ✅ Assign healing sound in Inspector
    public float soundVolume = 0.7f; // Adjust volume

    public Sprite explosionSprite; // Assign explosion sprite in Inspector

    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;

        // ✅ Fix NullReferenceException for spriteRenderers
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers.Length == 0)
        {
            Debug.LogError(gameObject.name + " is missing SpriteRenderers!");
        }

        // ✅ Fix NullReferenceException for mainSpriteRenderer
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        if (mainSpriteRenderer != null)
        {
            originalSprite = mainSpriteRenderer.sprite; // Store original sprite
        }
        else
        {
            Debug.LogError(gameObject.name + " is missing a SpriteRenderer component!");
        }

        // ✅ Fix NullReferenceException for audioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError(gameObject.name + " is missing an AudioSource!");
        }

        // ✅ Fix NullReferenceException for explosionSprite
        if (explosionSprite == null)
        {
            Debug.LogError(gameObject.name + " has no explosion sprite assigned!");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage! Health: " + currentHealth);

        // ✅ Play hit sound if assigned
        if (hitSound != null && audioSource != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position, soundVolume);
        }

        UpdateHearts(); // ✅ Remove a heart sprite when hit

        if (currentHealth <= 0)
        {
            StartCoroutine(ExplosionEffect()); // Show explosion before destroying
        }
        else
        {
            StartCoroutine(BecomeInvincible());
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth < maxHealth) // ✅ Only heal if not at max health
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth; // Prevent overhealing
            }
            Debug.Log(gameObject.name + " healed! Health: " + currentHealth);
            UpdateHearts(); // ✅ Restore heart UI when healing

            // ✅ Play healing sound
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
            }
        }
    }

    public bool CanHeal()
    {
        return currentHealth < maxHealth; // ✅ Returns true if healing is possible
    }

    void UpdateHearts()
    {
        if (hearts == null || hearts.Length == 0)
        {
            Debug.LogWarning(gameObject.name + " has no hearts assigned in the Inspector!");
            return;
        }

        // ✅ Hide all hearts first
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }

        // ✅ Show hearts up to the current health
        for (int i = 0; i < currentHealth; i++)
        {
            hearts[i].SetActive(true);
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

    IEnumerator ExplosionEffect()
    {
        Debug.Log(gameObject.name + " exploded!");

        // ✅ Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, soundVolume);
        }

        // ✅ Ensure sprite changes to explosion sprite
        if (explosionSprite != null)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.sprite = explosionSprite;
            }
        }

        // ✅ Disable movement & collision
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        yield return new WaitForSeconds(explosionDuration); // Wait for explosion effect

        DestroyTank();
    }

    void DestroyTank()
    {
        Debug.Log(gameObject.name + " has been destroyed!");

        if (arrow != null)
        {
            arrow.SetActive(false);
        }

        //  Show Game Over Screen and Declare Winner
        if (gameObject.CompareTag("Player1"))
        {
            GameOverManager.ShowGameOver("Player 2 Wins!");
        }
        else if (gameObject.CompareTag("Player2"))
        {
            GameOverManager.ShowGameOver("Player 1 Wins!");
        }

        Destroy(gameObject);
    }
}

